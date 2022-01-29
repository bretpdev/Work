using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Efs = Uheaa.Common.DataAccess.EnterpriseFileSystem;

namespace TCPAFEDPS
{
    public class TcpaPhoneScrubFed : FedBatchScript
    {
        private const string EojTotalsFromSas = "Total number of phone numbers in the SAS file";
        private const string EojProcessed = "Number of phone numbers successfully processed";
        private const string EojError = "Number of records sent to error queue or error report";
        private const string EojErrorAddingMobileInd = "Unable to save mobile indicator changes";
        private static readonly string[] EojFields = { EojTotalsFromSas, EojProcessed, EojError, EojErrorAddingMobileInd };
        private const string FileToProcessPattern = "flag_*";

        private enum CheckTd22Results
        {
            HasLoans,
            GotEndorser,
            None
        }

        public TcpaPhoneScrubFed(ReflectionInterface ri)
            : base(ri, "TcpaFedPS", "ERR_BU35", "EOJ_BU35", EojFields)
        {

        }

        public override void Main()
        {
            string[] arcsToCheckAccess = new string[] { "TCPAB", "TCPAE", "TCPAS", "TCPAR" };

            //Checks to see if the current UserId has access to the ARC's needed when running the script
            foreach (string arc in arcsToCheckAccess)
            {
                if (!CheckArcAccess(arc))
                {
                    NotifyAndEnd("You do not have access to the ARC: {0}", arc);
                }
            }

            if (!Recovery.RecoveryValue.IsNullOrEmpty())
            {
                DoRecovery();
            }

            //Gives the user the abitily to choose what mode to run in.
            using (ModeChooser mode = new ModeChooser())
            {
                if (mode.ShowDialog() == DialogResult.Cancel)
                {
                    EndDllScript();
                }

                if (mode.BatchProcessing)
                {
                    DoBatchProcessing();
                }
                else
                {
                    using (OpenFileDialog fileChooser = new OpenFileDialog())
                    {
                        fileChooser.InitialDirectory = Efs.FtpFolder;
                        fileChooser.Multiselect = false;

                        bool doLoop = true;

                        while (doLoop)
                        {
                            if (fileChooser.ShowDialog() == DialogResult.Cancel)
                            {
                                doLoop = false;
                                continue;
                            }

                            if (new FileInfo(fileChooser.FileName).Length == 0)
                            {
                                MessageBox.Show("The file is empty. Please choose a new file", "Emtpy File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                continue;
                            }

                            using (StreamReader sr = new StreamReader(fileChooser.FileName))
                            {
                                string header = sr.ReadLine();

                                if (header.IsNullOrEmpty())
                                    NotifyAndEnd("The file {0} was empty.  The script will now end.", fileChooser.FileName);

                                if (header.SplitAndRemoveQuotes(",").Count != 4)//Check the header for the correct file fields
                                {
                                    if (MessageBox.Show(string.Format("The SAS file {0} is not in the correct format.  Do you want to choose another file?", fileChooser.FileName), "Incorrect File Format", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        Recovery.Delete();
                                        continue;
                                    }
                                }

                                if (sr.ReadLine().IsNullOrEmpty())
                                {
                                    if (MessageBox.Show(string.Format("The SAS file {0} is empty.  Do you want to choose another file?", fileChooser.FileName), "File Empty", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        File.Delete(fileChooser.FileName);
                                        Recovery.Delete();
                                        continue;
                                    }
                                    else
                                    {
                                        File.Delete(fileChooser.FileName);
                                        Recovery.Delete();
                                        EndDllScript();
                                    }
                                }
                            }

                            //Copy the file to the T:\ drive
                            string file = @"T:" + fileChooser.FileName.Substring(fileChooser.FileName.LastIndexOf("\\"));
                            File.Move(fileChooser.FileName, file);

                            ProcessTheFile(file);
                        }
                    }
                }
            }

            if (Err.HasErrors)
                ProcessingComplete("Processing complete with errors. Please see the error log");
            else
                ProcessingComplete();
        }

        private void DoBatchProcessing()
        {
            //Search for the file
            List<string> filesToProcess = Directory.GetFiles(Efs.FtpFolder, FileToProcessPattern).ToList();

            if (filesToProcess.Count < 1)
            {
                NotifyAndEnd("The SAS file is missing.  Please investigate and try again.");
            }

            List<string> files = new List<string>(filesToProcess);
            List<string> filesToDelete = new List<string>();
            foreach (string file in files)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string header = sr.ReadLine();
                    string line = sr.ReadLine();
                    if (line.IsNullOrEmpty())
                    {
                        if (MessageBox.Show("The file '" + file + "' is empty. Do you want continue processing the other files?",
                            "Empty File", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            filesToProcess.Remove(file);
                            filesToDelete.Add(file);
                            continue;
                        }
                        else
                            EndDllScript();
                    }
                }
            }

            //Remove all the empty files
            foreach (string file in filesToDelete)
            {
                File.Delete(file);
            }

            //Process each file
            foreach (string file in filesToProcess)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string header = sr.ReadLine();

                    if (header.IsNullOrEmpty())
                        NotifyAndEnd("The file {0} was empty.  The script will now end.", file);

                    if (header.SplitAndRemoveQuotes(",").Count != 4)
                    {
                        if (MessageBox.Show(string.Format("The SAS file {0} located in {1} was not in the correct format.  Do you want to continue to the next file?"),
                            "Incorrect File Format", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Recovery.Delete();
                            continue;
                        }
                    }

                    if (sr.ReadLine().IsNullOrEmpty())
                    {
                        if (MessageBox.Show(string.Format("The SAS file {0} located in {1} was empty.  Do you want to continue processing?" +
                            " (Please note that this file will not be deleted)"), "File Empty", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Delete(file);
                            Recovery.Delete();
                            continue;
                        }
                        else
                        {
                            File.Delete(file);
                            Recovery.Delete();
                            EndDllScript();
                        }
                    }
                }

                //Copy the file to the T:\ drive
                string newFile = @"T:\" + file.Substring(file.LastIndexOf("\\") + 1);
                File.Move(file, newFile);

                ProcessTheFile(newFile);
                Recovery.Delete();//Clean up recovery for the next file
            }
        }

        private void ProcessTheFile(string file)
        {
            List<SasFileData> fileData = new List<SasFileData>();

            using (StreamReader sr = new StreamReader(file))
            {
                sr.ReadLine(); //Read in the header
                while (!sr.EndOfStream)
                {
                    List<string> fileLine = sr.ReadLine().SplitAndRemoveQuotes(",").ToList();
                    SasFileData sasData = new SasFileData() { AccountNumber = fileLine[0], PhoneNumber = fileLine[1], RunDate = DateTime.Parse(fileLine[2]), MobileIndicator = fileLine[3] };

                    //When the file is sent to possible now the account number is encrypted by multipling the account number by the Julian Date
                    int julianDate = int.Parse(string.Format("{0:yy}{1:D3}", sasData.RunDate, sasData.RunDate.DayOfYear));

                    if (!sasData.AccountNumber.Contains("P"))
                    {
                        long julianAccountNumber = long.Parse(sasData.AccountNumber);
                        sasData.AccountNumber = (julianAccountNumber / julianDate).ToString();
                        sasData.AccountNumber = sasData.AccountNumber.PadLeft(10, '0');
                    }

                    //update Eoj Counts if not in Recovery.  If in recovery this should have already been done.
                    if (Recovery.RecoveryValue.IsNullOrEmpty())
                    {
                        Eoj.Counts[EojTotalsFromSas].Increment();
                    }
                    //sasData.AccountNumber = DAccess.GetAccountNumber(sasData.Ssn);

                    fileData.Add(sasData);
                }
            }

            //Recovery counter used for recovery
            int recoveryCounter = Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0].IsNullOrEmpty() ? 0 : int.Parse(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0]);

            foreach (SasFileData item in fileData.Skip(recoveryCounter))
            {
                foreach (Arcs arcToCheck in DataAccess.GetArcsToCheck().Where(p => p.Endorser == false))
                {
                    RI.FastPath("TX3Z/ITD2A" + item.AccountNumber);

                    if (!RI.CheckForText(1, 72, "TDX2B"))
                        break;

                    RI.PutText(11, 65, arcToCheck.Arc, ReflectionInterface.Key.Enter, true);
                    if (RI.CheckForText(1, 72, "TDX2D", "TDX2D"))
                    {
                        item.HasArcToUpdate = true;
                        break;
                    }
                }

                RI.FastPath("TX3Z/CTX1J");

                RI.PutText(5, 16, "", true);
                RI.PutText(6, 16, "", true);
                RI.PutText(6, 20, "", true);
                RI.PutText(6, 23, "", true);

                if (item.AccountNumber.Substring(0, 1).Contains("P"))
                    RI.PutText(6, 16, item.AccountNumber, ReflectionInterface.Key.Enter);
                else
                {
                    RI.PutText(6, 61, item.AccountNumber, ReflectionInterface.Key.Enter);
                    if (RI.GetText(1, 9, 1) == "R")
                    {
                        RI.PutText(1, 9, "E", ReflectionInterface.Key.Enter);

                        //The SAS currently doesn't pull references unless they have both an SSN without a "P" and an account number.  Note: in these circumstances it will add an error to the ErrorReport indicating that an ARC could not be added.
                        if (!RI.CheckForText(1, 71, "TXX1R-02"))
                            RI.PutText(1, 9, "R", ReflectionInterface.Key.Enter);

                    }
                }

                if (!RI.CheckForText(1, 71, "TXX1R"))
                    NotifyAndEnd("Unable to locate account. Please contact Systems Support to make sure script is functioning properly.");

                RI.Hit(ReflectionInterface.Key.F6);
                RI.Hit(ReflectionInterface.Key.F6);
                RI.Hit(ReflectionInterface.Key.F6);

                string personType = RI.GetText(1, 9, 1);
                string bwrSsn = string.Empty;
                string personId = string.Empty;

                if (personType.Contains("B"))
                    bwrSsn = RI.GetText(3, 12, 11).Replace(" ", "");
                else
                {
                    bwrSsn = RI.GetText(7, 11, 11).Replace(" ", "");
                    personId = RI.GetText(3, 12, 11).Replace(" ", "");
                }

                string bwrAccountNumber = DataAccess.GetAccountNumber(bwrSsn);

                string phoneType = "H";
                string mbl = item.MobileIndicator.IsNullOrEmpty() ? "L" : "M";
                string results = string.Empty;

                if (personType.Contains("E"))
                {
                    foreach (Arcs arcToCheck in DataAccess.GetArcsToCheck().Where(p => p.Endorser == true))
                    {
                        RI.FastPath("TX3Z/ITD2A" + bwrSsn);

                        if (!RI.CheckForText(1, 72, "TDX2B"))
                            break;

                        RI.PutText(11, 65, arcToCheck.Arc, ReflectionInterface.Key.Enter, true);
                        if (RI.CheckForText(1, 72, "TDX2D", "TDX2C"))
                        {
                            item.HasArcToUpdate = true;
                            break;
                        }
                    }

                    RI.FastPath("TX3Z/CTX1J");

                    RI.PutText(5, 16, "", true);
                    RI.PutText(6, 16, "", true);
                    RI.PutText(6, 20, "", true);
                    RI.PutText(6, 23, "", true);

                    if (item.AccountNumber.Substring(0, 1).Contains("P"))
                        RI.PutText(6, 16, item.AccountNumber, ReflectionInterface.Key.Enter);
                    else
                        RI.PutText(6, 61, item.AccountNumber, ReflectionInterface.Key.Enter);

                    if (!RI.CheckForText(1, 71, "TXX1R"))
                        NotifyAndEnd("Unable to locate account. Please contact Systems Support to make sure script is functioning properly.");

                    RI.Hit(ReflectionInterface.Key.F6);
                    RI.Hit(ReflectionInterface.Key.F6);
                    RI.Hit(ReflectionInterface.Key.F6);
                }

                string endSsn = string.Empty;
                while (true)
                {
                    RI.PutText(16, 14, phoneType, ReflectionInterface.Key.Enter);
                    string currentPhoneNumber = RI.GetText(17, 14, 3) + RI.GetText(17, 23, 3) + RI.GetText(17, 31, 4);

                    if (currentPhoneNumber.Contains(item.PhoneNumber))
                    {
                        if (UpdatePhoneInfo(item, phoneType, mbl, file))
                        {
                            results += phoneType + ",";
                        }
                    }

                    if (phoneType.Contains("H"))
                    {
                        phoneType = "A";
                    }
                    else if (phoneType.Contains("A"))
                    {
                        phoneType = "W";
                    }
                    else if (phoneType.Contains("W"))
                    {
                        phoneType = "M";
                    }
                    else if (phoneType.Contains("M"))
                    {
                        break;
                    }
                }//End while

                if (personType.Contains("B") || personType.Contains("E"))
                {
                    CheckTd22Results result = CheckTd22(item.AccountNumber, personType.Contains("E"));
                    switch (result)
                    {
                        case CheckTd22Results.GotEndorser:
                            endSsn = RI.GetText(3, 12, 11).Replace(" ", "");
                            break;
                        case CheckTd22Results.None:
                            Err.AddRecord("Unable to add activity comment to borrowers account.", new { AccountNumber = item.AccountNumber, PhoneNumber = item.PhoneNumber, MBL = mbl, PhoneType = phoneType });
                            Eoj.Counts[EojError].Increment();
                            continue;
                    }
                }

                string comment = string.Empty;
                string arc = string.Empty;
                if (!results.IsNullOrEmpty())
                {
                    switch (personType)
                    {
                        case "B":
                            comment = string.Format("Changed mobile indicator field to {0} for phone {1} and PHN TYP(s) {2} because of Possible Now update", mbl, item.PhoneNumber, results.LastIndexOf(",") > 0 ? results.Remove(results.LastIndexOf(",")) : results);
                            arc = "TCPAB";
                            break;
                        case "S":
                            comment = string.Format("Changed mobile indicator field to {0} for student {1} phone {2}, and PHN TYP(s) {2} because of Possible Now update", mbl, item.AccountNumber, item.PhoneNumber, results.LastIndexOf(",") > 0 ? results.Remove(results.LastIndexOf(",")) : results);
                            arc = "TCPAS";
                            break;
                        case "E":
                            comment = string.Format("Changed mobile indicator field to {0} for endorser {1} phone {2} and PHN TYP(s) {2} because of Possible Now update", mbl, item.AccountNumber, item.PhoneNumber, results.LastIndexOf(",") > 0 ? results.Remove(results.LastIndexOf(",")) : results);
                            arc = "TCPAE";
                            break;
                        case "R":
                            comment = string.Format("Changed mobile indicator field to {0} for reference {1} phone {2} because of Possible Now update", mbl, item.AccountNumber, item.PhoneNumber);
                            arc = "TCPAR";
                            break;
                    }

                    if (!Atd22ByBalance(bwrSsn, arc, comment, item.AccountNumber.Contains("P") ? item.AccountNumber : "", ScriptId, false))
                    {
                        Err.AddRecord("Unable to add activity comment to borrowers account.", new { AccountNumber = item.AccountNumber, PhoneNumber = item.PhoneNumber, MBL = mbl, PhoneType = phoneType });
                        Eoj.Counts[EojError].Increment();
                        continue;
                    }
                }

                Eoj.Counts[EojProcessed].Increment();
                recoveryCounter++;
                Recovery.RecoveryValue = string.Format("{0},{1}", recoveryCounter, file);
            }//End foreach

            //Delete the file after the processing is complete.
            File.Delete(file);
            Recovery.Delete();
        }//End ProcessTheFile

        private bool UpdatePhoneInfo(SasFileData fileData, string phoneType, string mbl, string file)
        {
            if (!RI.CheckForText(17, 54, "Y"))
                return false;

            RI.PutText(17, 54, "Y");
            bool isMobile = RI.CheckForText(16, 20, "M");
            RI.PutText(16, 20, fileData.MobileIndicator.IsNullOrEmpty() ? "L" : "M");
            RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));

            if (fileData.HasArcToUpdate)
                RI.PutText(16, 30, "Y");
            else
            {
                if (RI.CheckForText(16, 20, "L"))
                    RI.PutText(16, 30, "Y");
                else if (isMobile && RI.CheckForText(16, 30, "Y"))
                    RI.PutText(16, 30, "Y");
                else
                    RI.PutText(16, 30, "N");
            }

            RI.PutText(19, 14, "59", ReflectionInterface.Key.Enter);

            if (!RI.CheckForText(23, 2, "01097"))
            {
                //string message = string.Format("{0}, {1}, {2}, {3} Unable to save mobile indicator changes", fileData.AccountNumber, fileData.PhoneNumber, mbl, phoneType);
                //Atd22ByBalance(fileData.AccountNumber, "ERTCP", message, string.Empty, ScriptId, false);
                Eoj.Counts[EojErrorAddingMobileInd].Increment();
                Err.AddRecord(EojErrorAddingMobileInd, new { AccountNumber = fileData.AccountNumber, PhoneNumber = fileData.PhoneNumber, MBL = mbl, PhoneType = phoneType });
                return false;
            }

            if (file.Contains("R3"))
            {
                if (!RI.CheckForText(9, 59, "_"))
                {
                    Eoj.Counts[EojError].Increment();
                    Err.AddRecord("", new { AccountNumber = fileData.AccountNumber, PhoneNumber = fileData.PhoneNumber, MBL = mbl, PhoneType = phoneType });
                    return false;
                }
            }

            return true;
        }

        private bool CheckArcAccess(string arc)
        {
            RI.FastPath("TX3Z/ITX68");
            RI.PutText(8, 41, UserId, true);
            RI.PutText(10, 41, arc, ReflectionInterface.Key.Enter, true);

            return RI.CheckForText(1, 72, "TXX6C");
        }

        private void DoRecovery()
        {
            ProcessTheFile(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1]);
        }

        private CheckTd22Results CheckTd22(string accountNumber, bool checkEnd)
        {
            string arcToUse = checkEnd ? "TCPAB" : "TCPAE";
            string personTypeToCheck = checkEnd ? "E" : "S";
            RI.FastPath(string.Format("TX3Z/ATD22{0};{1}", accountNumber, arcToUse));

            if (!RI.CheckForText(1, 72, "TDX24"))
            {
                RI.FastPath("TX3Z/ITX1J");
                RI.PutText(5, 16, personTypeToCheck, true);
                RI.PutText(6, 16, "", true);
                RI.PutText(6, 20, "", true);
                RI.PutText(6, 23, "", true);
                RI.PutText(6, 61, accountNumber, ReflectionInterface.Key.Enter);

                if (RI.CheckForText(1, 71, "TXX1R"))
                {
                    return CheckTd22Results.GotEndorser;
                }

                return CheckTd22Results.None;
            }

            return CheckTd22Results.HasLoans;

        }
    }
}
