using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using Q;

namespace DocIdCornerStone
{
    class Program
    {
        public const string SCRIPT_ID = "DOCIDFED";
        private const string APPLICATION_NAME = "CornerStone Doc ID";
        private readonly DataAccess _dataAccess;
        private readonly bool _testMode;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool testMode = args.Contains("test");
            Program program = new Program(testMode);
            program.Run();
        }

        public Program(bool testMode)
        {
            _dataAccess = new DataAccess(testMode);
            _testMode = testMode;
        }

        public void Run()
        {
            //Log into a Reflection session and start processing.
            ReflectionInterface ri = LogIntoNewSession();
            if (ri == null)
            {
                return;
            }
            try
            {
                ProcessDocuments(ri);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //All done? Close the session and wait for the report to finish (if needed).
            ri.CloseSession();
        }//Run()

        private ReflectionInterface LogIntoNewSession()
        {
            ReflectionInterface ri = new ReflectionInterface(_testMode, ScriptSessionBase.Region.CornerStone);
            //TODO: Use the Credentials class and Login form from Q.
            SessionCredentials credentials = new SessionCredentials();
            using (Login login = new Login(credentials))
            {
                if (login.ShowDialog() != DialogResult.OK)
                {
                    ri.CloseSession();
                    return null;
                }
                while (!ri.Login(credentials.UserId, credentials.Password, ScriptSessionBase.Region.CornerStone))
                {
                    MessageBox.Show("The session could not log you in. Please try again.", APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (login.ShowDialog() != DialogResult.OK)
                    {
                        ri.CloseSession();
                        return null;
                    }
                    ri.LogOut();
                }//while
            }//using
            return ri;
        }//LogIntoNewSession()

        private void ProcessDocuments(ReflectionInterface ri)
        {
            //Get the items for the combo boxes.
            IEnumerable<DocumentDetail> documentDetails = _dataAccess.GetDocumentDetails();
            IEnumerable<DocumentSource> sources = DocumentSource.GetList();

            //Process as many documents as the user wishes.
            SessionHandler session = new SessionHandler(ri);
            while (true)
            {
                UserInput userInput = new UserInput();
                Main ui = new Main(documentDetails, sources, userInput);
                if (ui.ShowDialog() != DialogResult.OK) { return; }
                //Check that an account ID was entered, and that the borrower/reference is on the system.
                if (string.IsNullOrEmpty(userInput.AccountId))
                {
                    MessageBox.Show("An SSN, account number, or reference ID is required.", APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }
                SystemBorrowerDemographics demos = null;
                try
                {
                    demos = session.GetDemographicsFromTX1J(userInput.AccountId);
                }
                catch (DemographicRetrievalException)
                {
                    MessageBox.Show("Invalid ID entered.", APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                //Verify with the user that the correct person was found.
                string message = string.Format("Is {0} {1} the correct {2}?", demos.FName, demos.LName, (demos.SSN.StartsWith("P") ? "reference" : "borrower"));
                if (MessageBox.Show(message, APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) { continue; }

                //Create an ARC to note that the document was received, and add a processing record to the database.
                string ssn = demos.SSN.Replace(" ", "");
                string comment = string.Format("{0} received", userInput.DocumentDetail.Description);
                if (session.LogCorrespondence(ssn, userInput.DocumentDetail.Arc, comment))
                {
                    _dataAccess.SetProcessedRecord(userInput.DocumentDetail.DocId, userInput.DocumentSource.Code);
                }
                else
                {
                    string warning = "Please set this document aside and notify Loan Servicing that this document errored in Corr Logging.";
                    MessageBox.Show(warning, APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }//while
        }//ProcessDocuments()
    }//class
}//namespace
