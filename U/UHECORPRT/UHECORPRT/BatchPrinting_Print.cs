using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common;
using System.IO;


namespace UHECORPRT
{
    partial class BatchPrinting
    {
        public void Print(ScriptData file, string dir, bool isCoBorrower)
        {
            List<PrintProcessingData> ppd;
            if (!isCoBorrower)
            {
                ppd = file.LetterDataForBorrowers;
            }
            else
            {
                ppd = file.LetterDataForCoBorrowers;
            }
            string accountNumberFieldName = file.FileHeaderConst.SplitAndRemoveQuotes(",")[file.AccountNumberIndex];
            string stateFieldName = file.FileHeaderConst.SplitAndRemoveQuotes(",")[file.StateIndex];
            string costCenterFieldName = file.FileHeaderConst.SplitAndRemoveQuotes(",")[file.CostCenterCodeIndex];

            CreateIndividualFiles(file, dir, accountNumberFieldName, stateFieldName, costCenterFieldName, file.FileHeader, isCoBorrower);
            if (file.LetterDataForBorrowers.Where(p => p.InValidAddress).Any())//We only add arcs for these files, there is no address information in the file.
                return;

            MassPrint(file, dir, accountNumberFieldName, stateFieldName, costCenterFieldName, isCoBorrower);
        }

        private void MassPrint(ScriptData file, string dir, string accountNumberFieldName, string stateFieldName, string costCenterFieldName, bool isCoBorrower)
        {
            List<string> fileData = new List<string>();
            List<PrintProcessingData> ppd;
            if (!isCoBorrower)
            {
                ppd = file.LetterDataForBorrowers;
            }
            else
            {
                ppd = file.LetterDataForCoBorrowers;
            }

            var printBwr = ppd.Where(p => !p.OnEcorr && !p.PrintedAt.HasValue && !p.InValidAddress);
            //fileData.Add(file.FileHeader);
            //fileData.AddRange(printBwr.Select(p => p.LetterData.Replace(Environment.NewLine, "")).ToList());
            int count = printBwr.Select(p => p.LetterData.Replace(Environment.NewLine, "")).ToList().Count;
            if (count > 0)//Always has a header line
            {
                List<List<string>> splitFiles = new List<List<string>>();
                //If < 600 then it will take everything, if more the 600 will be broken into groups of 6
                int take = count / (count > 600 ? 6 : 1);
                for (int skip = 0; count > skip; skip += take)
                {
                    List<string> fileToAdd = new List<string>() { file.FileHeader };
                    //Remove extra carrage returns
                    var bwrs = printBwr.Select(p => p.LetterData.Replace(Environment.NewLine, "")).ToList();
                    fileToAdd.AddRange((bwrs.Skip(skip).Take(take).ToList()));
                    splitFiles.Add(fileToAdd);
                }

                Parallel.ForEach(splitFiles, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, printFile =>
                {
                    WriteFileAndPrint(file, dir, accountNumberFieldName, stateFieldName, costCenterFieldName, printFile);
                });
            }


            Parallel.ForEach(printBwr, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, borrower =>
            {
                borrower.MarkPrintingDone(DA, isCoBorrower);
            }); //marking the records printed this way is faster than converting to a data table and sending it up as table table variable.

        }

        private void CreateIndividualFiles(ScriptData file, string dir, string accountNumberFieldName, string stateFieldName, string costCenterFieldName, string header, bool isCoBorrower)
        {
            string createDate = "";
            string billSeq = "";
            string TotalDue = "";
            string dueDate = "";
            if (file.BillDueDateIndex.HasValue)//This is a billing file so extra data is needed to be stored in the EcorrUheaa table
            {
                createDate = file.FileHeaderConst.SplitAndRemoveQuotes(",")[file.BillCreateDateIndex.Value];
                billSeq = file.FileHeaderConst.SplitAndRemoveQuotes(",")[file.BillSeqIndex.Value];
                TotalDue = file.FileHeaderConst.SplitAndRemoveQuotes(",")[file.TotalDueIndex.Value];
                dueDate = file.FileHeaderConst.SplitAndRemoveQuotes(",")[file.BillDueDateIndex.Value];
            }
            bool onlyDoStateMail = file.Recipient == "Other";
            if (file.AddBarCodes)
                file.FileHeader = GetBarcodeData(file.Letter, "", onlyDoStateMail).First().Key + file.FileHeaderConst;

            List<PrintProcessingData> ppd;
            if (!isCoBorrower)
            {
                ppd = file.LetterDataForBorrowers;
            }
            else
            {
                ppd = file.LetterDataForCoBorrowers;
            }

            Parallel.ForEach(ppd, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, borrower =>
            {
                Console.WriteLine("Creating imaging file for PrintProcessingId:{0}", borrower.PrintProcessingId);
                //Creates the temp files for word to process
                if (file.AddBarCodes)//Add State and return mail bar codes if needed.
                {
                    Dictionary<string, string> barcodes = new Dictionary<string, string>();
                    barcodes = GetBarcodeData(file.Letter, borrower.AccountNumber, onlyDoStateMail);
                    borrower.LetterData = barcodes.First().Value + borrower.LetterDataConst;
                }

                if (createDate.IsPopulated())
                {
                    borrower.BillSeq = borrower.LetterData.SplitAndRemoveQuotes(",")[file.FileHeader.SplitAndRemoveQuotes(",").IndexOf(billSeq)].ToIntNullable();
                    borrower.BillCreateDate = borrower.LetterData.SplitAndRemoveQuotes(",")[file.FileHeader.SplitAndRemoveQuotes(",").IndexOf(createDate)].ToDateNullable();
                    borrower.BillTotalDue = borrower.LetterData.SplitAndRemoveQuotes(",")[file.FileHeader.SplitAndRemoveQuotes(",").IndexOf(TotalDue)].ToDecimalNullable();
                    borrower.DueDate = borrower.LetterData.SplitAndRemoveQuotes(",")[file.FileHeader.SplitAndRemoveQuotes(",").IndexOf(dueDate)].ToDateNullable();
                }

                borrower.WordDataFile = Path.Combine(dir, string.Format("{0}_{1}_{2}.txt", borrower.PrintProcessingId, file.Letter, Guid.NewGuid().ToBase64String()));
                File.WriteAllLines(borrower.WordDataFile, new string[] { file.FileHeader, borrower.LetterData.Replace(Environment.NewLine, "") });
            });
        }

        private static void WriteFileAndPrint(ScriptData file, string dir, string accountNumberFieldName, string stateFieldName, string costCenterFieldName, List<string> fileData)
        {
            string printingFile = string.Format(@"{0}\Printing_{1}{2}.txt", dir, file.Letter, Guid.NewGuid().ToBase64String());
            int acctIndex = fileData.First().SplitAndRemoveQuotes(",").IndexOf(accountNumberFieldName);
            string beginingAcct = fileData[1].SplitAndRemoveQuotes(",")[acctIndex];
            string endingAcct = fileData.Last().SplitAndRemoveQuotes(",")[acctIndex];
            DocumentPathAndName docInfo = Uheaa.Common.DocumentProcessing.DataAccess.GetDocumentPathAndName(file.Letter);
            string templatePath = Path.Combine(docInfo.CalculatedPath, docInfo.CalculatedFileName);
            string tempFile = Path.Combine(dir, string.Format("{0}_{1}-{2}{3}{4}", Path.GetFileNameWithoutExtension(templatePath), beginingAcct, endingAcct, Guid.NewGuid().ToBase64String(), Path.GetExtension(templatePath)));//Create a unique file.
            File.Copy(templatePath, tempFile, true);
            File.WriteAllLines(printingFile, fileData);
            DocumentProcessing.PrintDocs(Path.GetDirectoryName(tempFile) + @"\", Path.GetFileName(tempFile), printingFile);
        }
    }
}
