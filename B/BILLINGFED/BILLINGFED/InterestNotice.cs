using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace BILLINGFED
{
    class InterestNotice
    {
        private string LetterId
        {
            get
            {
                return "INNOSCHFED";
            }
        }

        public void ProcessInterestNotice(Borrower bor, ProcessLogRun logRun)
        {
            List<InterestNoticeData> borInterestNotices = new List<InterestNoticeData>();
            int? rpt = bor.ParseReportNumber();
            foreach (string line in bor.LineData)
                borInterestNotices.Add(InterestNoticeData.Parse(line, rpt.Value == 19));

            List<string> addressInfo = borInterestNotices.First().GetAddressInfo();
            DataTable loanDetail = InterestNoticeData.GetLoanDetail(borInterestNotices);

            EcorrData ecorrInfo = EcorrProcessing.CheckEcorr(bor.AccountNumber);
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), LetterId + ".pdf");

            //Always create ecorr doc and image
            CreateEcorrAndImage(bor, addressInfo, loanDetail, ecorrInfo, templatePath, borInterestNotices, logRun);
            if (!bor.OnEcorr && !bor.PrintedAt.HasValue)
                BillingStatementsFed.IH.AddToInterestNoticeFile(CreatePrintingFileLine(borInterestNotices, bor.PrintProcessingId));
        }

        private void CreateEcorrAndImage(Borrower bor, List<string> addressInfo, DataTable loanDetail, EcorrData ecorrInfo, string templatePath, List<InterestNoticeData> iData, ProcessLogRun logRun)
        {
            string imageFile = "";
            if (!bor.EcorrDocumentCreatedAt.HasValue)
            {
                //Creates the Ecorr file to go to Ecorr and Imaging
                if (bor.IsEndorser)
                    bor.Ssn = BillingStatementsFed.DA.GetSsnFromAcctNum(bor.AccountNumber);
                imageFile = PdfHelper.GenerateEcorrPdf(templatePath, bor.AccountNumber, bor.Ssn, bor.OnEcorr ? DocumentProperties.CorrMethod.EmailNotify : DocumentProperties.CorrMethod.Printed, "UT00801", ecorrInfo, addressInfo, loanDetail);
                bor.SetEcorrDocumentCreated();
                //Images Ecorr file
                if (!bor.ImagedAt.HasValue)
                {
                    DocumentProcessing.ImageFile(imageFile, BillingStatementsFed.DocId, bor.Ssn);
                    bor.SetImagedAt();
                }
            }
            if (!bor.ImagedAt.HasValue && imageFile.IsNullOrEmpty())
            {
                //Creates imaging file to be imaged
                BillingStatementsFed.IH.CreateIntNoticeDataFile(bor, iData);
                try
                {
                    BillingStatementsFed.IntNoticeImagingLock.EnterWriteLock();
                    DocumentProcessing.ImageDocs(BillingStatementsFed.ScriptId, BillingStatementsFed.AccountNumber, BillingStatementsFed.DocId,
                        LetterId, BillingStatementsFed.IH.IntNoticeDataFile, DocumentProcessing.LetterRecipient.Borrower);
                }
                catch (Exception ex)
                {
                    string message = $"There was an error creating the Interest Notice image for borrower: {bor.AccountNumber}";
                    logRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                }
                finally
                {
                    BillingStatementsFed.IH.IntNoticeImagingLock.ExitWriteLock();
                    bor.SetImagedAt();
                }
            }
        }

        public static string CreatePrintingFileLine(List<InterestNoticeData> loans, int printProcessingId)
        {
            List<string> line = new List<string>();
            for (int index = 0; index < loans.Count; index++)
            {
                InterestNoticeData loan = loans[index];
                if (index == 0)//Only want to add the address info once
                    line.AddRange(new List<string>() { printProcessingId.ToString(), loan.ACSKeyLine, loan.FirstName, loan.LastName, loan.Address1,
                        loan.Address2, loan.City, loan.State, loan.ZipCode, loan.Country, loan.AccountNumber });

                line.AddRange(new List<string>() { loan.FirstDisbDate, loan.LoanProgram, loan.PriorInterest.ToString(),
                    loan.TotalInterestDue.ToString(), loan.BalanceAtTimeOfBill.ToString() });
            }

            string commas = "";
            for (int commaCount = loans.Count; commaCount < 30; commaCount++)
                commas += ",,,,,";//adding the remaining commas


            return string.Join(",", line) + commas;
        }
    }
}