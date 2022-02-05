using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace PHNCONUPUH
{
    public class PhoneConsentUpdate : BatchScript
    {

        private static readonly string[] EojFields = { string.Empty };
        private int ErrorCount { get; set; }

        public PhoneConsentUpdate(ReflectionInterface ri)
            : base(ri, "PHNCONUPUH", "ERR_BU35", "EOJ_BU35", EojFields, DataAccessHelper.Region.Uheaa)
        {
            ErrorCount = 0;
        }

        public override void Main()
        {
            List<QueueInformation> queues = DataAccess.GetQueueInfo();

            foreach (QueueInformation queue in queues.Skip(CheckRecovery(queues)))
            {
                while (true)
                {
                    RI.FastPath("TX3Z/ITX6X");
                    RI.PutText(10, 37, " ", true);
                    RI.PutText(6, 37, queue.Queue);
                    RI.PutText(8, 37, queue.SubQueue, ReflectionInterface.Key.Enter);
                    if (RI.CheckForText(23, 2, "01020"))
                        break;
                    string ssn = RI.GetText(8, 6, 9);
                    string endAccountNumber = RI.GetText(9, 61, 10);
                    RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter);
                    WorkTheQueue(ssn, queue, endAccountNumber);
                    CloseTheTask(queue.Queue, queue.SubQueue);
                }
            }

            ProcessingComplete(string.Format("Phone Consent Update has finished. There were {0} errors while processing this script", ErrorCount));
        }

        private void WorkTheQueue(string ssn, QueueInformation queue, string endAccountNumber)
        {
            RI.FastPath("TX3Z/CTX1J;");
            RI.PutText(5, 16, " ");
            if (endAccountNumber.IsNullOrEmpty() || !endAccountNumber.IsNumeric())
                RI.PutText(6, 16, ssn, ReflectionInterface.Key.Enter);
            else
            {
                if (endAccountNumber.Length == 10)
                    RI.PutText(6, 61, endAccountNumber, ReflectionInterface.Key.Enter);
                else
                    RI.PutText(6, 16, endAccountNumber, ReflectionInterface.Key.Enter);
            }

            RI.Hit(ReflectionInterface.Key.F6);
            RI.Hit(ReflectionInterface.Key.F6);
            RI.Hit(ReflectionInterface.Key.F6);

            string phoneType = "H";
            while (!phoneType.IsNullOrEmpty())
            {
                if (RI.CheckForText(17, 54, "Y"))
                {
                    RI.PutText(17, 54, "Y");
                    if (!UpdateMBL(ssn, queue, endAccountNumber))
                        return;
                }

                if (phoneType.Contains("H"))
                {
                    phoneType = "A";
                    RI.PutText(16, 14, phoneType, ReflectionInterface.Key.Enter);
                }
                else if (phoneType.Contains("A"))
                {
                    phoneType = "W";
                    RI.PutText(16, 14, phoneType, ReflectionInterface.Key.Enter);
                }
                else if (phoneType.Contains("W"))
                {
                    phoneType = "M";
                    RI.PutText(16, 14, phoneType, ReflectionInterface.Key.Enter);
                }
                else
                    phoneType = string.Empty;
            }

            string queueComment = queue.Comment;

            if (!endAccountNumber.IsNullOrEmpty())
                queueComment = queue.Comment + " END " + endAccountNumber;

            if (!Atd22AllLoans(ssn, queue.Arc, queueComment, string.Empty, ScriptId, false))
            {
                Err.AddRecord("Unable to leave comment in borrower’s notes", new
                {
                    SSN = ssn,
                    Endorser = endAccountNumber,
                    Queue = queue.Queue,
                    SubQueue = queue.SubQueue,
                    Arc = queue.Arc,
                    Comment = queue.Comment
                });
                ErrorCount++;
            }   
        }

        private bool UpdateMBL(string ssn, QueueInformation queue, string endAccountNumber)
        {

            RI.PutText(16, 20, RI.CheckForText(16, 20, "M") ? "M" : "L");

            if (RI.CheckForText(16, 30, "N", " ", "_"))
            {
                RI.PutText(16, 30, "Y");
                RI.PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                RI.PutText(19, 14, queue.SourceCode, ReflectionInterface.Key.Enter);

                if (!RI.CheckForText(23, 2, "01097"))
                {
                    Err.AddRecord("Unable to save mobile indicator changes", new
                    {
                        SSN = ssn,
                        Endorser = endAccountNumber,
                        Queue = queue.Queue,
                        SubQueue = queue.SubQueue,
                        Arc = queue.Arc,
                        Comment = queue.Comment
                    });
                    ErrorCount++;
                    return false;
                }
            }

            return true;
        }

        private void CloseTheTask(string queue, string subQueue)
        {
            RI.FastPath("TX3Z/ITX6X");
            RI.PutText(10, 37, " ", true);
            RI.PutText(6, 37, queue);
            RI.PutText(8, 37, subQueue, ReflectionInterface.Key.Enter);

            if (RI.CheckForText(8, 75, "W"))
            {

                RI.PutText(21, 18, "01", ReflectionInterface.Key.F2);
                RI.PutText(8, 19, "C");
                RI.PutText(9, 19, "COMPL", ReflectionInterface.Key.Enter);
            }
        }

        private int CheckRecovery(List<QueueInformation> queues)
        {
            int queueNumber = 0;
            foreach (QueueInformation queue in queues)
            {
                RI.FastPath("TX3Z/ITX6X");
                RI.PutText(10, 37, " ", true);
                RI.PutText(6, 37, queue.Queue);
                RI.PutText(8, 37, queue.SubQueue, ReflectionInterface.Key.Enter);
                if (RI.CheckForText(8, 75, "W"))
                    return queueNumber;

                queueNumber++;
            }

            return 0;
        }
    }
}
