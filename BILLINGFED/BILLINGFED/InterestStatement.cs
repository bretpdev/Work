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
    class InterestStatement
    {
        private string LetterId
        {
            get
            {
                return "INTBILFED";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bor"></param>
        /// <returns></returns>
        public void ProcessInterestStatement(Borrower bor, ProcessLogRun logRun, bool isEndorser)
        {
            List<InterestStatementData> borIntStatement = new List<InterestStatementData>();
            foreach (string line in bor.LineData)
                borIntStatement.Add(InterestStatementData.Parse(line, isEndorser));

            List<string> addressInfo = borIntStatement.First().GetAddressInfo();
            DataTable loanDetail = InterestStatementData.GetLoanDetail(borIntStatement);

            EcorrData ecorrInfo = EcorrProcessing.CheckEcorr(bor.AccountNumber);
            string templatePath = Path.Combine(EnterpriseFileSystem.GetPath("Correspondence"), LetterId + ".pdf");

            //Always ecorr and image the bill
            CreateEcorrAndImage(bor, addressInfo, loanDetail, ecorrInfo, templatePath, borIntStatement, logRun);
            if (!bor.OnEcorr && !bor.PrintedAt.HasValue)
                BillingStatementsFed.IH.AddToInterestStatementFile(CreatePrintingFileLine(borIntStatement, bor.PrintProcessingId));
        }

        private void CreateEcorrAndImage(Borrower bor, List<string> addressInfo, DataTable loanDetail, EcorrData ecorrInfo, string templatePath, List<InterestStatementData> iData, ProcessLogRun logRun)
        {
            string imageFile = "";
            if (!bor.EcorrDocumentCreatedAt.HasValue)
            {
                //Creates the Ecorr file to go to Ecorr and Imaging
                if (bor.IsEndorser)
                {
                    bor.AccountNumber = bor.LineData[0].SplitAndRemoveQuotes(",")[2];
                    bor.Ssn = BillingStatementsFed.DA.GetSsnFromAcctNum(bor.AccountNumber);
                }
                imageFile = PdfHelper.GenerateEcorrPdf(templatePath, bor.AccountNumber, bor.Ssn, bor.OnEcorr ? DocumentProperties.CorrMethod.EmailNotify : DocumentProperties.CorrMethod.Printed, "UT00801",
                    ecorrInfo, addressInfo, loanDetail, GetFormFields(iData));
                bor.SetEcorrDocumentCreated();
                if (!bor.ImagedAt.HasValue)
                {
                    //Images Ecorr file
                    DocumentProcessing.ImageFile(imageFile, BillingStatementsFed.DocId, bor.Ssn);
                    bor.SetImagedAt();
                }
            }
            if (!bor.ImagedAt.HasValue && imageFile.IsNullOrEmpty())
            {
                //Creates imaging file to be imaged
                BillingStatementsFed.IH.CreateIntStatementDataFile(bor, iData);
                try
                {
                    BillingStatementsFed.IntStatementImagingLock.EnterWriteLock();
                    DocumentProcessing.ImageDocs(BillingStatementsFed.ScriptId, BillingStatementsFed.AccountNumber, BillingStatementsFed.DocId,
                        LetterId, BillingStatementsFed.IH.IntStatementDataFile, DocumentProcessing.LetterRecipient.Borrower);
                }
                catch (Exception ex)
                {
                    string message = $"There was an error creating the Interest Statement image for borrower: {bor.AccountNumber}";
                    logRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                }
                finally
                {
                    BillingStatementsFed.IH.IntStatementImagingLock.ExitWriteLock();
                    bor.SetImagedAt();
                }
            }
        }

        /// <summary>
        /// Creates a txt file to be merged with the word doc that will be used in printing.
        /// </summary>
        /// <param name="loans">All the loans for the borrower</param>
        /// <param name="printProcessingId">The PrintProcessingId for the borrower</param>
        /// <returns></returns>
        public static string CreatePrintingFileLine(List<InterestStatementData> loans, int printProcessingId)
        {
            List<string> line = new List<string>();
            for (int index = 0; index < loans.Count; index++)
            {
                InterestStatementData loan = loans[index];
                if (index == 0)//Only want to add the address info once
                    line.AddRange(new List<string>() { printProcessingId.ToString(), loan.ACSKeyLine, loan.FirstName, loan.LastName, loan.Address1, loan.Address2, loan.City, 
                        loan.State, loan.ZipCode, loan.Country, loan.DF_SPE_ACC_ID, loan.CoBorrowerAccountNumber, loan.LD_BIL_CRT, loan.LD_BIL_DU, loan.LD_FAT_EFF, 
                        loan.LA_FAT_CUR_PRI, loan.LA_FAT_NSI, loan.TAP, loan.LA_BIL_PAS_DU.ToString(),
                        loan.LA_BIL_DU_PRT, loan.LA_CUR_INT_DU.ToString()});

                line.AddRange(new List<string>() { loan.LoanProgram, loan.FirstDisbDate, loan.InterestRate.ToString(),
                    loan.OriginalPrincipal.ToString(), loan.CurrentBalance.ToString() });
            }

            string commas = "";
            for (int commaCount = loans.Count; commaCount < 30; commaCount++)
                commas += ",,,,,";

            return string.Join(",", line) + commas;
        }

        private Dictionary<string, string> GetFormFields(List<InterestStatementData> iData)
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>();
            formFields.Add("FIRSTNAME", iData[0].FirstName);
            formFields.Add("LASTNAME", iData[0].LastName);
            formFields.Add("ACCOUNTNUMBER", iData[0].DF_SPE_ACC_ID);
            formFields.Add("LD_BIL_CRT", iData[0].LD_BIL_CRT);
            formFields.Add("LD_BIL_DU", iData[0].LD_BIL_DU);
            formFields.Add("LD_FAT_EFF", iData[0].LD_FAT_EFF);
            formFields.Add("ACCT_LA_FAT_CUR_PRI", iData[0].LA_FAT_CUR_PRI);
            formFields.Add("ACCT_LA_FAT_NSI", iData[0].LA_FAT_NSI);
            formFields.Add("ACCT_TAP", iData[0].TAP);
            formFields.Add("LA_BIL_PAS_DU", iData[0].LA_BIL_PAS_DU);
            formFields.Add("LA_CUR_INT_DU", iData[0].LA_CUR_INT_DU);
            formFields.Add("LA_BIL_DU_PRT", iData[0].LA_BIL_DU_PRT);
            formFields.Add("LD_BIL_DU1", iData[0].LD_BIL_DU);
            formFields.Add("ACCOUNTNUMBER1", iData[0].DF_SPE_ACC_ID);
            formFields.Add("LA_CUR_INT_DU1", iData[0].LA_CUR_INT_DU);
            formFields.Add("LA_BIL_DU_PRT1", iData[0].LA_BIL_DU_PRT);
            formFields.Add("LD_BIL_DU2", iData[0].LD_BIL_DU);

            return formFields;
        }
    }
}