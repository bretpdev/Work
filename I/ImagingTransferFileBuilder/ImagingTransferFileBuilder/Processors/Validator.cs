using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ImagingTransferFileBuilder
{
    public class NoIndexFileException : Exception { }
    public class MultipleIndexFilesException : Exception { }
    public static class Validator
    {
        public static void Validate(string zipPath, string excelLocation, string masterName)
        {
            Progress.Start("Validator");
            using (ZipFile zip = new ZipFile(zipPath))
            {
                #region IndexSetup
                ZipEntry index = null;
                var indexFiles = zip.Where(z => z.FileName.ToLower().EndsWith(".idx"));
                if (indexFiles.Count() > 1)
                    throw new MultipleIndexFilesException();
                if (indexFiles.Count() == 0)
                    throw new NoIndexFileException();

                index = indexFiles.Single();
                MemoryStream stream = new MemoryStream();
                index.Extract(stream);
                stream.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(stream);
                string[] lines = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                stream.Dispose();
                sr.Dispose();
                #endregion

                List<Borrower> borrowers = ExcelHelper.GetAllBorrowers(excelLocation, masterName, true);

                string[] fileNames = zip.Select(z => Path.GetFileName(z.FileName)).ToArray();
                string[] lowerFileNames = fileNames.Select(f => f.ToLower()).ToArray();

                int lineNumber = 0;
                List<KeyValuePair<int, string>> previousDataFileNames = new List<KeyValuePair<int, string>>();
                string previousDealID = null;
                string previousSaleDate = null;
                string previousProgramType = null;
                Progress.Increments = lines.Length;
                
                foreach (string line in lines)
                {
                    lineNumber++;
                    string[] fields = line.Split('|');
                    if (fields.Length != 11)
                    {
                        InvalidNumberOfElements(lineNumber, fields.Length, 11);
                        continue;
                    }
                    string ssn = fields[0];
                    string firstName = fields[2];
                    string lastName = fields[1];
                    string loanID = fields[4];
                    string programType = fields[6];
                    string guarantyDate = fields[7];
                    string saleDate = fields[8];
                    string dealID = fields[9];


                    string dataFileName = fields[10];
                    KeyValuePair<int, string>? dupe = previousDataFileNames.Where(kvp => kvp.Value == dataFileName).OfType<KeyValuePair<int, string>?>().DefaultIfEmpty(null).Single();
                    if (dupe != null)
                        DuplicateDataFileName(dupe.Value.Key, lineNumber, dataFileName);
                    previousDataFileNames.Add(new KeyValuePair<int, string>(lineNumber, dataFileName));
                    IEnumerable<string> matchingFiles = lowerFileNames.Where(l => l == dataFileName.ToLower());
                    if (!lowerFileNames.Contains(dataFileName.ToLower()))
                        DataFileNotFound(lineNumber, dataFileName);
                    else if (!fileNames.Contains(dataFileName))//case doesn't match
                        WrongFileCase(lineNumber, dataFileName, fileNames.Where(f => f.ToLower() == dataFileName.ToLower()).Single());

                    if (previousProgramType != null && previousProgramType != programType)
                        NotIdenticalField(lineNumber, "Program Type", previousProgramType, programType);
                    if (previousSaleDate != null && previousSaleDate != saleDate)
                        NotIdenticalField(lineNumber, "Sale Date", previousSaleDate, saleDate);
                    if (previousDealID != null && previousDealID != dealID)
                        NotIdenticalField(lineNumber, "Deal ID", previousDealID, dealID);

                    
                    List<Borrower> ssnRows = borrowers.Where(b => b.SSN == ssn).ToList();
                    if (!ssnRows.Select(row => row.SSN).Contains(ssn))
                        NotInExcel(lineNumber, "SSN", ssn, ssn);
                    if (!ssnRows.Select(row => row.FirstName.ToLower()).Contains(firstName.ToLower()))
                        NotInExcel(lineNumber, "First Name", firstName, ssn);
                    if (!ssnRows.Select(row => row.LastName.ToLower()).Contains(lastName.ToLower()))
                        NotInExcel(lineNumber, "Last Name", lastName, ssn);
                    if (!ssnRows.Select(row => row.LoanID).Contains(loanID))
                        NotInExcel(lineNumber, "Loan ID", loanID, ssn);
                    if (!ssnRows.Select(row => row.DealID).Contains(dealID))
                        NotInExcel(lineNumber, "Deal ID", dealID, ssn);
                    if (!ssnRows.Select(row => row.GuarantyDate).Contains(guarantyDate))
                    {
                        DateTime outer = new DateTime();
                        DateTime g = new DateTime();
                        if (DateTime.TryParse(guarantyDate, out g))
                        {
                            List<DateTime?> dates = ssnRows.Select(row => DateTime.TryParse(row.GuarantyDate, out outer) ? outer : (DateTime?)null).ToList();
                            if (!dates.Contains(g))
                                NotInExcel(lineNumber, "Guaranty Date", guarantyDate, ssn);
                        }
                        else
                            NotInExcel(lineNumber, "Guaranty Date", guarantyDate, ssn);
                    }

                    bool foundMatchingRow = false;
                    foreach (Borrower er in ssnRows)
                    {
                        if (er.SSN == ssn && er.DealID == dealID && er.FirstName.ToLower() == firstName.ToLower() && er.LastName.ToLower() == lastName.ToLower() && er.LoanID == loanID)
                        {
                            if (er.GuarantyDate == guarantyDate)
                            {
                                foundMatchingRow = true;
                                break;
                            }
                            DateTime g = new DateTime();
                            DateTime o = new DateTime();
                            if (DateTime.TryParse(guarantyDate, out g))
                                if (DateTime.TryParse(er.GuarantyDate, out o))
                                    if (o == g)
                                    {
                                        foundMatchingRow = true;
                                        break;
                                    }
                        }
                    }
                    if (!foundMatchingRow)
                        InvalidRow(lineNumber, ssn, dealID, firstName, lastName, loanID, guarantyDate);

                    previousProgramType = programType;
                    previousSaleDate = saleDate;
                    previousDealID = dealID;

                    Progress.Increment();
                }

                foreach (ZipEntry ze in zip.Where(z => !previousDataFileNames.Select(p => p.Value).Contains(Path.GetFileName(z.FileName))))
                {
                    if (!ze.FileName.Contains('.'))
                        continue;//doesn't apply to root folder
                    if (ze.FileName.ToLower().EndsWith(".idx"))
                        continue; //doesn't apply to index file
                    ExtraneousDataFile(ze.FileName);
                }
                Progress.Finish();
            }
        }

        private static void InvalidRow(int lineNumber, string ssn, string dealID, string firstName, string lastName, string loanID, string guarantyDate)
        {
            Results.LogError(string.Format("Line {0}: Could not find an exact match in the Excel document for SSN ({1}) Deal ID ({2}) First Name ({3}) Last Name ({4}) Loan ID ({5}) Guaranty Date ({6})",
                lineNumber, ssn, dealID, firstName, lastName, loanID, guarantyDate));
        }

        private static void NotInExcel(int line, string field, string value, string ssn)
        {
            Results.LogError(string.Format("Line {0}: {1} ({2}) was not found in the excel document for the given ssn ({3}).", line, field, value, ssn));
        }

        private static void WrongFileCase(int line, string dataFileName, string correctName)
        {
            Results.LogError(string.Format("Line {0}: Data file is listed as {1} when the correct casing is {2}.", line, dataFileName, correctName));
        }

        private static void NotIdenticalField(int line, string fieldName, string previousValue, string value)
        {
            Results.LogError(string.Format("Line {0}: {1} is set as {2} when the previous line's {1} is {3}.  {1} should be identical across all lines.", line, fieldName, value, previousValue));
        }

        private static void ExtraneousDataFile(string fileName)
        {
            Results.LogError(string.Format("File {0} was not found in the index file.", fileName));
        }

        private static void DataFileNotFound(int lineNumber, string dataFileName)
        {
            Results.LogError(string.Format("Line {1}: Data file {0} was not found.", dataFileName, lineNumber));
        }


        private static void InvalidNumberOfElements(int line, int number, int expectedElements)
        {
            Results.LogError(string.Format("Line {0}: Line has {1} element{3} when it should have {2} elements.", line, number, expectedElements, number == 1 ? "" : "s"));
        }

        private static void DuplicateDataFileName(int firstLine, int secondLine, string dataFileName)
        {
            Results.LogError(string.Format("Line {1}: Data File Name {2} is a duplicate of line {0}", firstLine, secondLine, dataFileName));
        }

    }

}
