using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace EBILLFED
{
    public class RequestUpdater : FedBatchScriptBase
    {
        //Recovery value is file name, line number (starting from 1);

        //TODO: Set the file pattern once we find out what it will be.
        private const string EBILL_FILE_PATTERN = "Bill_Change_KU.*";
        private readonly ErrorReport _errorReport;
        private long _loansInFile = 0;
        private long _loansUpdated = 0;
        private long _queueTasksCreated = 0;
        private long _loansOnErrorReport = 0;
        private string _errorMessage;

        public RequestUpdater(ReflectionInterface ri)
            : base(ri, "EBILLFED")
        {
            _errorReport = new ErrorReport(ri.TestMode);
            _errorMessage = string.Empty;
        }

        public override void Main()
        {
            StartupMessage("This is the Update E-Bill Request script. Click OK to continue, or Cancel to quit.");

            //Get a list of all E-Bill request files.
            List<string> foundFiles = Directory.GetFiles(Efs.FtpFolder, EBILL_FILE_PATTERN).ToList();
            if (foundFiles.Count() == 0)
            {
                NotifyAndEnd("No E-Bill request files were found. The script will now end.");
            }

            bool allFilesWereEmpty = true;

            //Recover if needed.
            if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
            {
                allFilesWereEmpty = false;
                string recoveringFile = Recovery.RecoveryValue.Split(',')[0];
                ProcessFile(recoveringFile);
                File.Delete(recoveringFile);
                foundFiles.Remove(recoveringFile);
            }

            //Process the remaining files.
            foreach (string ebillFile in foundFiles)
            {
                if (new FileInfo(ebillFile).Length > 0)
                {
                    allFilesWereEmpty = false;
                    ProcessFile(ebillFile);
                }
                File.Delete(ebillFile);
            }//foreach

            if (allFilesWereEmpty)
            {
                NotifyAndEnd("All E-Bill request files were empty. Processing Complete!");
                //if (!CalledByMasterBatchScript())
                //{
                //    string emptyFileMessage = "All E-Bill request files were empty. Processing Complete!";
                //    MessageBox.Show(emptyFileMessage, ScriptID, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
            }
            else
            {
                //create end of job report
                CreateEojReport();
                ProcessingComplete();
            }
        }//Main()


        private void ProcessFile(string fileName)
        {
            using (StreamReader fileReader = new StreamReader(fileName))
            {
                int lineNumber = 0;
                //Skip to the last completed record if in recovery.
                if (!string.IsNullOrEmpty(Recovery.RecoveryValue))
                {
                    int recoveryLineNumber = int.Parse(Recovery.RecoveryValue.Split(',')[1]);
                    _loansUpdated = int.Parse(Recovery.RecoveryValue.Split(',')[2]);
                    _queueTasksCreated = int.Parse(Recovery.RecoveryValue.Split(',')[3]);
                    _loansOnErrorReport = int.Parse(Recovery.RecoveryValue.Split(',')[4]);
                    while (lineNumber != recoveryLineNumber)
                    {
                        fileReader.ReadLine();
                        lineNumber++;
                    }
                }//if

                //Process the remaining records.
                while (!fileReader.EndOfStream)
                {
                    RequestRecord record = RequestRecord.Parse(fileReader.ReadLine());
                    lineNumber++;
                    _errorMessage = string.Empty;
                    if (record.BillingPreference == "E" || record.BillingPreference == "P") { UpdateBillingPreference(record.SSN, record.LoanSequence, record.BillingPreference, record.HasNoGracePeriod); }
                    UpdateEmailAddress(record.SSN, record.Email);
                    AddActivityComment(record.SSN, record.BillingPreference, record.LoanSequence);
                    if (_errorMessage.Length != 0)
                    {
                        AddErrorQueueTask(record.BillingPreference, record.SSN, record.LoanSequence, fileName, lineNumber);
                    }
                    else
                    {
                        _loansUpdated++;
                    }
                    Recovery.RecoveryValue = string.Format("{0},{1},{2},{3},{4}", fileName, lineNumber, _loansUpdated, _queueTasksCreated, _loansOnErrorReport);
                }//while

                _loansInFile += lineNumber;
            }//using


            Recovery.Delete();
        }//ProcessFile()

        private void CreateEojReport()
        {
            //create end of job report
            string eojFileName = string.Format("{0}EBILLFED_EOJ_{1:MM-dd-yyyy.HHmm}.html", Efs.GetPath("EOJ_BU01"), DateTime.Now);
            using (StreamWriter eojWriter = new StreamWriter(eojFileName))
            {
                eojWriter.WriteLine("Updating E-bill Requests - FED</br>");
                eojWriter.WriteLine("End of Job Report</br>");
                eojWriter.WriteLine(String.Format("{0}</br>", eojFileName));
                eojWriter.WriteLine("<p>");
                eojWriter.WriteLine("<table>");
                eojWriter.WriteLine("<tr><td>Item</td><td>Count</td></tr>");
                eojWriter.WriteLine();
                eojWriter.WriteLine(string.Format("<tr><td>Loan Sequences in the File(s):</td><td>{0}</td></tr>", _loansInFile));
                eojWriter.WriteLine(string.Format("<tr><td>Loan Sequences Updated:</td><td>{0}</td></tr>", _loansUpdated));
                eojWriter.WriteLine(string.Format("<tr><td>Error Queue Tasks Created:</td><td>{0}</td></tr>", _queueTasksCreated));
                eojWriter.WriteLine(string.Format("<tr><td>Loan Sequences on Error Report:</td><td>{0}</td></tr>", _loansOnErrorReport));
            }
        }

        private void UpdateBillingPreference(string ssn, string loanSequence, string billingPreference, bool hasNoGracePeriod)
        {
            FastPath("TX3Z/CTS7C{0}", ssn);
            if (Check4Text(1, 72, "TSX3S"))
            {
                //Selection screen.
                while (!Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
                {
                    bool selectedLoan = false;
                    for (int row = 7; !Check4Text(row, 20, "    "); row++)
                    {
                        if (Check4Text(row, 20, loanSequence))
                        {
                            PutText(22, 19, GetText(row, 3, 2), Key.Enter);
                            selectedLoan = true;
                            break;
                        }
                    }//for
                    if (selectedLoan)
                    {
                        break;
                    }
                    else
                    {
                        Hit(Key.F8);
                    }
                }//while
            }//if

            //target screen.
            if (Check4Text(1, 72, "TSX7D"))
            {
                if (Check4Text(14, 48, "_")) { PutText(14, 48, "N"); } //EXT TRM DEBT IND
                if (Check4Text(18, 19, "_") && hasNoGracePeriod) { PutText(18, 19, "0"); }
                PutText(19, 42, (billingPreference == "E" ? "Y" : "N")); //E-BILL IND
                Hit(Key.Enter);
            }

            //verify  update
			if (!Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED", "01004 RECORD SUCCESSFULLY ADDED", "01003 NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED"))
            {
                _errorMessage = "Unable to update billing preference ";
            }
        }//UpdateBillingPreference()


        private void UpdateEmailAddress(string ssn, string email)
        {
            //access demographics
            FastPath("TX3Z/CTX1JB;{0}", ssn);

            //access e-mail screen
            if (Check4Text(1, 71, "TXX1R-01"))
            {
                Hit(Key.F2);
                Hit(Key.F10);
            }

            //update e-mail address
            if (Check4Text(1, 72, "TXX4V"))
            {
                PutText(9, 20, "32"); //ADDR SOURCE CODE
                PutText(11, 17, DateTime.Now.ToString("MMddyy")); //ADDR LAST VER
                PutText(12, 14, "Y"); //ADDR VALID
                string existingEmail = (GetText(14, 10, 60) + GetText(15, 10, 60) + GetText(16, 10, 60) + GetText(17, 10, 60) + GetText(18, 10, 14)).Trim('_');
                if (email != existingEmail)
                {
                    PutText(14, 10, "", true);
                    PutText(15, 10, "", true);
                    PutText(16, 10, "", true);
                    PutText(17, 10, "", true);
                    PutText(18, 10, "", true);
                    PutText(14, 10, email);
                }
                Hit(Key.Enter);
            }

            //verify update
			if (!Check4Text(23, 2, "01005 RECORD SUCCESSFULLY CHANGED", "01004 RECORD SUCCESSFULLY ADDED", "01003 NO FIELDS UPDATED - NO RECORD CHANGES PROCESSED"))
            {
                _errorMessage += "Unable to update email address ";
            }
        }//UpdateEmailAddress()


        private void AddActivityComment(string ssn, string billingPreference, string loanSequence)
        {
            string arc = "";
            string comment = "";
            switch (billingPreference)
            {
                case "M":
                    arc = "EBIL1";
                    comment = "Per borrowers online request updated e-mail address.";
                    break;
                case "P":
                    arc = "EBIL2";
                    comment = string.Format("Update E-bill to paper bill for loan seq {0} per bwrs online request.", loanSequence);
                    break;
                case "E":
                    arc = "EBIL3";
                    comment = string.Format("Update bill to E-bill for loan seq {0} per bwrs online request verified/updated email.", loanSequence);
                    break;
            }//switch

            //verify update
            if (!ATD22ByBalance(ssn, arc, comment, false))
            {
                _errorMessage = string.Format("There was an error in adding activity comments. Please review account to ensure the correct activity comment is added. {0}", _errorMessage);
            }
        }//AddActivityComment()


        //add error queue task
        private void AddErrorQueueTask(string billingPreference, string ssn, string loanSequence, string fileName, int lineNumber)
        {
            string arc = (billingPreference == "E" ? "EBIL4" : "EBIL5");
            string comment = string.Format("{0} for loan seq {1}", _errorMessage, loanSequence);
            if (!ATD22ByBalance(ssn, arc, comment, false))
            {

                string accountNumber = "";
                try
                {
                    BorrowerDemographics borrowerDemos = GetDemographicsFromTX1J(ssn);
                    accountNumber = borrowerDemos.AccountNumber;
                }
                catch
                {
                    accountNumber = string.Format("File:{0}; Line:{1}; SSN:XXX-XX-{2}", fileName, lineNumber, ssn.Substring(5, 4));
                }

                _errorReport.Add(accountNumber, billingPreference, loanSequence, comment, ref _loansOnErrorReport);
            }
            else
            {
                _queueTasksCreated++;
            }
        }//AddErrorQueueTask

    }//class
}//namespace
