using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace DFACDFED
{
    public class Processor
    {
        private DateTime today;
        private ProcessLogData data;
        public Processor(ProcessLogData data)
        {
            this.data = data;
            this.EcorrLetterBorrowers = new List<BorrowerInfo>();
            this.PrintedLetterBorrowers = new List<BorrowerInfo>();
        }
        DataAccess da;
        public void Process(DateTime today)
        {
            this.today = today;
            this.da = new DataAccess(data.ProcessLogId);
            Running = true;
            var letters = da.GetAllUnprocessedLetters();
            foreach (var letter in letters)
                if (Running)
                    ProcessLetter(letter);
                else
                    break;
            ProcessLogger.AddNotification(data.ProcessLogId, PrintedLetterBorrowers.Count + " Printed letters generated.", NotificationType.EndOfJob, NotificationSeverityType.Informational);
            ProcessLogger.AddNotification(data.ProcessLogId, EcorrLetterBorrowers.Count + " ECORR letters generated.", NotificationType.EndOfJob, NotificationSeverityType.Informational);
            ProcessLogger.LogEnd(data.ProcessLogId, null);
            Running = false;
        }

        private void ProcessLetter(UnprocessedLetter letter)
        {
            
            var borrowerInfo = new BorrowerInfo(letter, da.GetLoanInfo(letter, today).Result);
            if (borrowerInfo.Loans.Any())
            {
                var generator = new PdfGenerator(borrowerInfo, da);
                var result = generator.CreatePdf();
                if (result != PdfGenerator.PdfResult.None)
                {
                    UpdatePrinted(borrowerInfo.Letter.LetterSequence, borrowerInfo.Letter.LetterId, borrowerInfo.Letter.AccountNumber);
                }
                if (result == PdfGenerator.PdfResult.Ecorr)
                {
                    EcorrLetterBorrowers.Add(borrowerInfo);
                    if (EcorrLetterProcessed != null)
                        EcorrLetterProcessed(borrowerInfo);
                }
                else if (result == PdfGenerator.PdfResult.Printed)
                {
                    PrintedLetterBorrowers.Add(borrowerInfo);
                    if (PrintedLetterProcessed != null)
                        PrintedLetterProcessed(borrowerInfo);
                }
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cdw, "Lt20MarkLetterPrinted")]
        private void UpdatePrinted(int letterSeq, string letterId, string accountNumber)
        {
            DataAccessHelper.Execute("Lt20MarkLetterPrinted", DataAccessHelper.Database.Cdw, SqlParams.Single("LetterSeq", letterSeq), SqlParams.Single("LetterId", letterId), SqlParams.Single("AccountNumber", accountNumber));
        }


        public List<BorrowerInfo> EcorrLetterBorrowers { get; private set; }
        public List<BorrowerInfo> PrintedLetterBorrowers { get; private set; }
        public delegate void BorrowerProcessed(BorrowerInfo info);
        public event BorrowerProcessed EcorrLetterProcessed;
        public event BorrowerProcessed PrintedLetterProcessed;
        public bool Running { get; private set; }
        public bool CancelPending { get; set; }
    }
}
