using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace FINALREV
{
    public class PrintSchoolLetters
    {
        public DataAccess DA { get; set; }
        public BorrowerRecord Borrower { get; set; }
        public ReflectionInterface RI { get; set; }

        public PrintSchoolLetters(DataAccess da, BorrowerRecord bor, ReflectionInterface ri)
        {
            DA = da;
            Borrower = bor;
            RI = ri;
        }

        /// <summary>
        /// Creates the data files and calls the PrintDocs common method to send the jobs to the printer
        /// </summary>
        public void PrintLetters()
        {
            List<SchoolLetterData> letters = DA.GetSchoolLetterData();
            if (letters.Count == 0)
                return;
            WriteLine("Printing school letters and closing queues");
            GetCostCenterCode(letters);
            foreach (SchoolLetterData letterData in letters)
            {
                SchoolLetterData.Populate(DA, letterData);
                letterData.Schools = new List<string>();
                letterData.Schools.AddRange(DA.GetSchools(letterData.BorrowerRecordId));
                AddKlsltComment(letterData);
            }

            Dictionary<SchoolData, List<SchoolLetterData>> schoolsData = new Dictionary<SchoolData, List<SchoolLetterData>>();
            foreach (string school in letters.SelectMany(p => p.Schools).Distinct().ToList())
            {
                foreach (string code in new List<string>() { "MA2324", "MA2327" })
                {
                    if (letters.Any(p => p.CostCenterCode == code))
                    {
                        SchoolData sData = GetSchoolInfo(school);
                        sData.CostCenterCode = code;
                        schoolsData.Add(sData, letters.Where(p => p.Schools.Contains(school) && p.CostCenterCode == code).ToList());
                    }
                }
            }

            foreach (KeyValuePair<SchoolData, List<SchoolLetterData>> sd in schoolsData)
            {
                if (sd.Key.OpeStatus != "C")
                {
                    var list = new List<KeyValuePair<SchoolData, List<SchoolLetterData>>> { sd };
                    CloseQueue(list.ToDictionary(x => x.Key, x => x.Value));
                    string coverLetter = $"{EnterpriseFileSystem.TempFolder}{sd.Key.SchName}_cover.txt";
                    using (StreamW sw = new StreamW(coverLetter))
                    {
                        sw.WriteLine("SchName, SchAdd, SchAdd2, SchAdd3, SchCity, SchST, SchZip");
                        sw.Write($"{sd.Key.SchName},{sd.Key.SchAdd},{sd.Key.SchAdd2},{sd.Key.SchAdd3},{sd.Key.SchCity},{sd.Key.SchST},{sd.Key.SchZip}");
                    }
                    string dataFile = $"{EnterpriseFileSystem.TempFolder}{sd.Key.SchName}_datafile.txt";
                    using (StreamW sw = new StreamW(dataFile))
                    {
                        sw.WriteLine("SSN, FirstName, MI, LastName, Address1, Address2, City, State, ZIP, Country, Phone, AltPhone, Email, School, SchName");
                        foreach (var l in sd.Value)
                            sw.WriteLine($"XXX-XX-{l.Ssn.Substring(5, 4)}, {l.FirstName}, {l.MI}, {l.LastName}, {l.Address1}, {l.Address2}, {l.City}, {l.State}, {(l.Zip.Length > 5 ? l.Zip.ToLong().ToString("#####-####") : l.Zip)}, {l.Country}, {(l.Phone.IsPopulated() ? l.Phone.Insert(3, "-").Insert(7, "-") : "")}, {(l.AltPhone.IsPopulated() ? l.AltPhone.Insert(3, "-").Insert(7, "-") : "")}, {l.Email}, {sd.Key.School}, {sd.Key.SchName}");
                    }
                    PrintCoverLetter(sd.Key.CostCenterCode, sd);
                    DocumentProcessing.PrintDocs(EnterpriseFileSystem.GetPath("SCHLTRLST"), "SCHLTRCVR", coverLetter);
                    DocumentProcessing.PrintDocs(EnterpriseFileSystem.GetPath("SCHLTRLST"), "SCHLTRLST", dataFile);
                    Repeater.TryRepeatedly(() => FS.Delete(coverLetter));
                    Repeater.TryRepeatedly(() => FS.Delete(dataFile));
                }
            }

            //Set the record as printed
            foreach (SchoolLetterData data in letters)
                DA.SetSchoolLetterSent(data.BorrowerRecordId);
        }

        /// <summary>
        /// Prints a cover letter
        /// </summary>
        private void PrintCoverLetter(string code, KeyValuePair<SchoolData, List<SchoolLetterData>> sld)
        {
            string coverSheet = $"{EnterpriseFileSystem.TempFolder}{code}_coverLetter.txt";
            using (StreamW sw = new StreamW(coverSheet))
            {
                List<SchoolLetterData> letters = sld.Value.Where(c => c.CostCenterCode == code).ToList();
                sw.WriteLine("BU, Description, NumPages, Cost, Standard, Foreign, CoverComment");
                int numPages;
                if ((letters.Count() % 3) > 0) //There is a remainder that will be on a second page
                    numPages = (letters.Count() / 3) + 2; //1 extra page for the remainder and 1 for the cover letter
                else
                    numPages = (letters.Count() / 3) + 1; //1 extra page for the cover letter
                int standard = letters.Where(p => p.State != "FC").Count();
                int foreign = letters.Where(p => p.State == "FC").Count();
                string bu = code == "MA2324" ? "LPP Portfolio Servicing" : "LPP Lender Services";
                sw.WriteLine($"{bu},School Letter -Skip Trace Assistance Request,{numPages},{code},{standard},{foreign},Deliver mail to business unit for processing");
            }
            DocumentProcessing.PrintDocs(EnterpriseFileSystem.GetPath("CoverSheet"), "Scripted State Mail Cover Sheet", coverSheet);
            Repeater.TryRepeatedly(() => FS.Delete(coverSheet));
        }

        /// <summary>
        /// Adds a KSLST ARC to every borrower account that is getting a letter sent to a school
        /// </summary>
        private void AddKlsltComment(SchoolLetterData data)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = data.Ssn,
                Arc = "KSLST",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = $"letter(s) mailed to school(s): {(string.Join(" ", data.Schools))} to request verification of borrower's demographic information",
                ScriptId = Program.ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding ARC KSLST to Borrower: {data.Ssn}; Error: {result.Ex}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
            }
        }

        private SchoolData GetSchoolInfo(string school)
        {
            SchoolData sData = new SchoolData();
            if (FindDepartment(school))
            {
                sData.SchName = RI.GetText(5, 21, 40);
                sData.SchAdd = RI.GetText(8, 21, 30);
                sData.SchAdd2 = RI.GetText(9, 21, 30);
                sData.SchAdd3 = RI.GetText(10, 21, 30);
                sData.SchCity = RI.GetText(11, 21, 30);
                sData.SchST = RI.GetText(11, 59, 2);
                if (sData.SchST == "FC")
                {
                    sData.SchST = RI.GetText(12, 21, 2);
                    sData.SchCountry = RI.GetText(12, 55, 2);
                }
                sData.SchZip = RI.GetText(11, 66, 5);
                if (RI.GetText(11, 71, 1).IsPopulated())
                    sData.SchZip += $"-{RI.GetText(11, 71, 4)}";
                RI.Hit(ReflectionInterface.Key.F10);
                sData.OpeStatus = RI.GetText(4, 20, 1);
                RI.Hit(ReflectionInterface.Key.F12);
            }
            return sData;
        }

        /// <summary>
        /// Iterates LPSCI to find department 112 or use GEN if 112 is not found
        /// </summary>
        private bool FindDepartment(string school)
        {
            string general = "";
            string registrar = "";
            RI.FastPath($"LPSCI{school}");
            if (RI.CheckForText(21, 3, "SEL"))
            {
                int row = 7;
                while (RI.AltMessageCode != "46004")
                {
                    if (RI.CheckForText(row, 7, "GEN"))
                        general = RI.GetText(row, 2, 2);
                    if (RI.CheckForText(row, 7, "112"))
                    {
                        registrar = RI.GetText(row, 2, 2);
                        break;
                    }
                    row++;
                    if (row == 19)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;
                        general = "";
                        if (RI.AltMessage == "46004" && (registrar.IsNullOrEmpty() && general.IsPopulated()))
                            registrar = general; //If 112 is not found, use general
                    }
                }
                if (registrar.IsNullOrEmpty())
                {
                    string message = $"There was an error finding department 112 or GEN for school: {school}.";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    return false;
                }
                else
                {
                    RI.PutText(21, 13, registrar, ReflectionInterface.Key.Enter);
                    return true;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the cost center code for each borrower
        /// </summary>
        private void GetCostCenterCode(List<SchoolLetterData> letters)
        {
            int seq = 4;
            int counter = 1;
            List<string> lenders = DA.GetLenderCodes();
            foreach (SchoolLetterData data in letters)
            {
                RI.FastPath($"LG10I{data.Ssn}");
                if (RI.CheckForText(1, 53, "LOAN BWR STATUS RECAP SELECT"))
                {
                    RI.PutText(19, 15, counter.ToString(), ReflectionInterface.Key.Enter);
                    while (!RI.CheckForText(20, 3, "47001"))
                    {
                        int tempSeq = GetSequence(lenders, data);
                        if (seq > tempSeq)
                            seq = tempSeq; //Make sure to always get the lowest sequence
                        RI.Hit(ReflectionInterface.Key.F12);
                        counter++;
                        RI.PutText(19, 15, counter.ToString(), ReflectionInterface.Key.Enter);
                    }
                }
                else
                    seq = GetSequence(lenders, data); //LG10 screen found
                if (seq > 0)
                    data.CostCenterCode = (seq == 1) ? "MA2324" : "MA2327";
            }
        }

        /// <summary>
        /// Searches through LG10 to find any loans that are Uheaa and have a balance
        /// </summary>
        private int GetSequence(List<string> lenders, SchoolLetterData data)
        {
            int seq = 4;
            if (RI.CheckForText(1, 52, "LOAN BWR STATUS RECAP DISPLAY"))
            {
                do
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (!RI.CheckForText(11 + i, 2, "_"))
                            break;
                        if (!RI.GetText(11, 59, 2).IsIn("CR", "CP") && RI.GetText(11 + i, 48, 8).ToDouble() > 0 && RI.GetText(5, 18, 6).IsIn(lenders.ToArray()))
                            return 1;
                    }
                    RI.Hit(ReflectionInterface.Key.F8);

                } while (RI.AltMessageCode != "46004");
            }
            else
            {
                string message = $"There was an error getting cost center code from LG10I for borrower: {data.Ssn}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return 0;
            }
            return seq;
        }

        /// <summary>
        /// Closes the queue after it has been worked.
        /// </summary>
        public void CloseQueue(Dictionary<SchoolData, List<SchoolLetterData>> schools)
        {
            foreach (KeyValuePair<SchoolData, List<SchoolLetterData>> sd in schools)
            {
                foreach (SchoolLetterData sld in sd.Value)
                {
                    RI.FastPath($"TX3ZCTD2A{sld.Ssn}");
                    RI.PutText(11, 65, "KLSLT");
                    RI.PutText(21, 16, DateTime.Now.ToString("MMddyy"));
                    RI.PutText(21, 30, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter);
                    if (RI.MessageCode == "01019")
                        return;
                    if (RI.ScreenCode == "TDX2C")
                        RI.PutText(7, 2, "X", ReflectionInterface.Key.Enter);
                    while (RI.MessageCode != "90007")
                    {
                        if (RI.GetText(15, 2, 5).Replace("_", "").IsNullOrEmpty())
                            RI.PutText(15, 2, (sd.Key.OpeStatus == "C" ? "INVAD" : "PRNTD"), ReflectionInterface.Key.Enter);
                        RI.Hit(ReflectionInterface.Key.F8);
                    }
                }
            }
        }
    }
}