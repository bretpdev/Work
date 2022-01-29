using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace PHNCNUPFED
{
    public class PhoneConsentUpdateFed : FedBatchScript
    {
        private const string EojTotalsFromSas = "Total number of phone numbers in the SAS file";
        private const string EojProcessed = "Number of phone numbers successfully processed";
        private const string EojError = "Number of records sent to error queue or error report";
        private static readonly string[] EojFields = { EojTotalsFromSas, EojProcessed, EojError };
        private int NumberOfErrors;

        public PhoneConsentUpdateFed(ReflectionInterface ri)
            : base(ri, "PHNCNUPFED", "ERR_BU35", "EOJ_BU35", EojFields)
        {
            NumberOfErrors = 0;
        }

        public override void Main()
        {
            StartupMessage("This script will access various queues in order to update borrowers’ phone consent to Y on all of their valid phone numbers. Would you like to run this script? ");
            List<QueueInformation> queues = DataAccess.GetQueueInfo();

            foreach (QueueInformation queue in queues.Skip(CheckRecovery(queues)))
            {
                ITX6XSelection selection = null;
                while ((selection = MakeITX6XSelection(queue)) != null)
                {
                    ProcessQueue(queue, selection);
                    AddComment(queue, selection);
                    CloseTask(queue);
                }
            }

            ProcessingComplete();
        }

        /// <summary>
        /// Returns true if a selection was made.  False if not.
        /// </summary>
        private ITX6XSelection MakeITX6XSelection(QueueInformation queue)
        {
            //access queue task selection screen
            FastPath("TX3Z/ITX6X");
            PutText(6, 37, queue.Queue);
            PutText(8, 37, queue.SubQueue);
            PutText(10, 37, " ", true);
            RI.Hit(ReflectionInterface.Key.Enter);

            if (RI.MessageCode == "01020") //nothing to do here
                return null;

            if (!RI.CheckForText(1, 74, "TXX71"))
            {
                MessageBox.Show(string.Format("{0} Please review and try again", RI.Message));
                return null;
            }

            while (RI.MessageCode != "90007") //no more data
            {
                for (int i = 8; i <= 17; i += 3)
                {
                    string accountNumber = GetText(i + 1, 61, 10);
                    string ssn = GetText(i, 6, 9);
                    string status = GetText(i, 75, 1);
                    string selection = GetText(i, 3, 2);
                    if (status == "U")
                    {
                        PutText(21, 18, selection, ReflectionInterface.Key.Enter);
                        return new ITX6XSelection(ssn, accountNumber);
                    }
                }
                RI.Hit(ReflectionInterface.Key.F8);
            }
            return null;
        }

        private void ProcessQueue(QueueInformation queue, ITX6XSelection selection, bool shouldFastPath = true)
        {
            if (shouldFastPath)//On the recursive call you do not want to re-access TX1J
                AccessTx1j(selection);

            string phoneType = GetText(16, 14, 1);
            bool phoneValid = GetText(17, 54, 1) == "Y";
            if (phoneValid)
                if (!UpdateMBL(queue, selection.SSN))
                    return;

            string nextPhoneType = null;
            if (phoneType == "H") nextPhoneType = "A";
            if (phoneType == "A") nextPhoneType = "W";
            if (phoneType == "W") nextPhoneType = "M";
            if (nextPhoneType != null)
            {
                PutText(16, 14, nextPhoneType, ReflectionInterface.Key.Enter);
                ProcessQueue(queue, selection, false);//Recursive call this will allow us to cycle though all phone types
            }
        }

        private void AccessTx1j(ITX6XSelection selection)
        {
            string fastPath = "TX3Z/CTX1J";
            if (selection.HasAccountNumber)
                fastPath += "E;" + selection.AccountNumber;
            else
                fastPath += "B;" + selection.SSN;
            FastPath(fastPath);

            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);
            Hit(ReflectionInterface.Key.F6);
        }

        private void AddComment(QueueInformation queue, ITX6XSelection selection)
        {
            if (!Atd22AllLoans(selection.SSN, queue.Arc, queue.Comment, string.Empty, ScriptId, false))
            {
                string comment = selection.HasAccountNumber ? "Unable to leave comment in borrower's notes for endorser LN20.LF EDS" : "Unable to leave comment in borrowers notes.";
                if (!Atd22AllLoans(selection.SSN, "PHCUP", comment, string.Empty, ScriptId, false))
                {
                    Err.AddRecord("Unable to leave comment in borrower’s notes", new { SSN = selection.SSN, Queue = queue.Queue, SubQueue = queue.SubQueue, Arc = queue.Arc, Comment = queue.Comment });
                }
                NumberOfErrors++;
            }
        }

        private bool UpdateMBL(QueueInformation queue, string ssn)
        {
            //M = M, L = L, U = L.  No change unless we find a 'U'
            if (CheckForText(16, 20, "U"))
                PutText(16, 20, "L");

            if (CheckForText(16, 30, "N", " ", "_"))
            {
                PutText(16, 20, GetText(16, 20, 1));
                PutText(16, 30, "Y");
                PutText(17, 54, "Y");
                PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                PutText(19, 14, queue.SourceCode, ReflectionInterface.Key.Enter);

                if (!CheckForText(23, 2, "01097"))
                {
                    if (!Atd22AllLoans(ssn, "PHCUP", "Unable to save mobile indicator changes", string.Empty, ScriptId, false))
                    {
                        Err.AddRecord("Unable to leave comment in borrower’s notes", new { SSN = ssn, Queue = queue.Queue, SubQueue = queue.SubQueue, Arc = queue.Arc, Comment = queue.Comment });
                    }
                    NumberOfErrors++;
                    return false;
                }
            }

            return true;
        }

        private void CloseTask(QueueInformation queue)
        {
            FastPath("TX3Z/ITX6X");
            PutText(10, 37, " ", true);
            PutText(6, 37, queue.Queue);
            PutText(8, 37, queue.SubQueue, ReflectionInterface.Key.Enter);

            if (CheckForText(8, 75, "W"))
            {
                PutText(21, 18, "01", ReflectionInterface.Key.F2);
                PutText(8, 19, "C");
                PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
            }
        }

        private int CheckRecovery(List<QueueInformation> queues)
        {
            int queueNumber = 0;
            foreach (QueueInformation queue in queues)
            {
                FastPath("TX3Z/ITX6X");
                PutText(10, 37, " ", true);
                PutText(6, 37, queue.Queue);
                PutText(8, 37, queue.SubQueue, ReflectionInterface.Key.Enter);
                if (CheckForText(8, 75, "W"))
                {
                    return queueNumber;
                }
                queueNumber++;
            }

            return 0;
        }

        private new void ProcessingComplete()
        {
            Err.Publish();
            Eoj.Publish();

            File.Create(string.Format("{0}MBS{1}_{2}_{3}.TXT", EnterpriseFileSystem.LogsFolder, ScriptId, UserId, DateTime.Now.ToString("MMddyyyy_hhmmss")));
            Recovery.Delete();

            if (!CalledByJams)
            {
                MessageBox.Show(string.Format("Phone Consent Update – FED has finished. There were {0} errors while processing this script.", NumberOfErrors), ScriptId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            EndDllScript();
        }
    }
}
