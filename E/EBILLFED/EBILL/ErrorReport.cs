using System;
using System.IO;
using Q;

namespace EBILLFED
{
    class ErrorReport
    {
        private readonly string _fileName;
        public string FileName { get { return _fileName; } }

        public bool Exists { get { return File.Exists(_fileName); } }
        private bool _testMode;

        public ErrorReport(bool testMode)
        {
            EnterpriseFileSystem Efs = new EnterpriseFileSystem(testMode, ScriptSessionBase.Region.CornerStone);
            _fileName = string.Format("{0}Errors_{1:MM-dd-yyyy.HHmm}.txt", Efs.GetPath("ERR_BU35"), DateTime.Now);
            _testMode = testMode;
        }

        public void Add(string accountNumber, string billingPreference, string loanSequence, string message, ref long loansOnErrorReport)
        {
            try
            {
                bool newFile = !Exists;
                using (StreamWriter errorWriter = new StreamWriter(_fileName, true))
                {
                    if (newFile) { errorWriter.WriteLine("AccountNumber,BillingPreference,LoanSequence,Message"); }
                    errorWriter.WriteCommaDelimitedLine(accountNumber, billingPreference, loanSequence, message);
                }
                loansOnErrorReport++;
            }
            catch (Exception)
            {
                string emailRecipients = DataAccessBase.GetEmailForKeyString(_testMode, "SSHELP", 0, DataAccessBase.EmailLookupOption.None);
                Common.SendMail(_testMode, emailRecipients, "", "Unable to save error log for E-bill Script", "The E-bill script was unable to save the error log to Q:\\Systems Support\\Batch Scripts\\Error Logs please review and resolve.", "", "", "", Common.EmailImportanceLevel.High, true);
                Common.EndDLLScript();
            }
        }//Add()
    }//class
}//namespace
