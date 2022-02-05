using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace THRDPAURES
{
    public class CommentsAndLetters
    {
        private ReflectionInterface RI { get; set; }
        private ProcessLogData LogData { get; set; }

        public CommentsAndLetters(ReflectionInterface ri, ProcessLogData logData)
        {
            RI = ri;
            LogData = logData;
        }

        /// <summary>
        /// Adds an activity comment to the session.
        /// </summary>
        /// <param name="accountNumber">Borrowers account number.</param>
        /// <param name="refDemos">References Information.</param>
        /// <param name="expirationDate">POA expiration date.</param>
        /// <param name="isApproved">Approved Indicator.</param>
        /// <param name="isPowerOfAttorney">Is a Power Of Attorney Request.</param>
        /// <param name="isNewReference">New Reference Indicator.</param>
        public void AddComment(string accountNumber, ReferencesDemographics refDemos, string expirationDate, bool isApproved, bool isPowerOfAttorney, bool isNewReference)
        {
            string comment = string.Empty;
            string arc = string.Empty;
            string expriationDate = expirationDate.IsNullOrEmpty() ? "" : string.Format("Expiration date: {0}", expirationDate);

            if (isApproved)
            {
                if (isNewReference && !isPowerOfAttorney)
                {
                    comment = string.Format("Added Reference {0} {1} to account with Third party Authorization.", refDemos.FirstName, refDemos.LastName);
                    arc = "M1REF";
                }
                else if (!isNewReference && !isPowerOfAttorney)
                {
                    comment = string.Format("3rd Party Authorization approved for {0} {1}", refDemos.FirstName, refDemos.LastName);
                    arc = "X3RDC";
                }
                else if (isNewReference && isPowerOfAttorney)
                {
                    comment = string.Format("Reference added with Power Of Attorney {0} {1} {2}", refDemos.FirstName, refDemos.LastName, expriationDate);
                    arc = "MREFP";
                }
                else if (!isNewReference && isPowerOfAttorney)
                {
                    comment = string.Format("Reference modified with Power of Attorney  {0} {1} {2}", refDemos.FirstName, refDemos.LastName, expriationDate);
                    arc = "MPOAA";
                }
            }
            else
            {
                if (!isPowerOfAttorney)
                {
                    comment = "3rd Party Authorization Denied";
                    arc = "X3RDD";
                }
                else
                {
                    comment = "Denied Power of Attorney";
                    arc = "MPOAD";
                }
            }

            if (!RI.Atd22AllLoans(accountNumber, arc, comment, "", "", false))
            {
                string errorMessage = string.Format("Unable to add comment to the following account: {0} ARC: {1} Comment: {2}", accountNumber, arc, comment);
                ProcessLogger.AddNotification(LogData.ProcessLogId, errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
        }

        /// <summary>
        /// Sends Approval/Denial Letter to Centralized Printing.
        /// </summary>
        /// <param name="bData">BorrowerData object</param>
        /// <param name="isApproved">Approved Indicator</param>
        /// <param name="isPowerOfAttorney">Is a Power Of Attorney Request.</param>
        public void PrintLetter(BorrowerData bData, bool isApproved, bool isPowerOfAttorney, string userId, string scriptId, int processLogId)
        {
            string file = GenerateDataFile(bData);
            string letterId = string.Empty;

            if (isApproved && !isPowerOfAttorney)
                letterId = "3PACONFFED";
            else if (!isApproved && !isPowerOfAttorney)
                letterId = "3PADENFED";
            else if (isApproved && isPowerOfAttorney)
                letterId = "POACNFFED";
            else if (!isApproved && isPowerOfAttorney)
                letterId = "POADENFED";

            SystemBorrowerDemographics demos = RI.GetDemographicsFromTx1j(bData.AccountNumber);

            EcorrProcessing.EcorrCentralizedPrintingAndImage(bData.AccountNumber, bData.Ssn, letterId, file, userId, scriptId, "AccountNumber", "State", demos, processLogId, !bData.HasValidAddress);
            while (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Generates a datafile for Centralized Printing.
        /// </summary>
        /// <param name="bData">BorrowerDara object</param>
        /// <returns>File path of temp file.</returns>
        private string GenerateDataFile(BorrowerData bData)
        {
            string file = Path.Combine(EnterpriseFileSystem.TempFolder, string.Format("THRDPAURES{0:MMddyyhhmmss}.txt", DateTime.Now));
            string keyLine = DocumentProcessing.ACSKeyLine(bData.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);

            using (StreamWriter sw = new StreamWriter(file))
            {
                //Write Header
                sw.WriteLine(string.Format("KeyLine,FirstName,LastName,Address1,Address2,City,State,ZIP,Country,AccountNumber,StaticCurrentDate"));
                sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10:MM/dd/yyyy}", keyLine, bData.BorrowersFirstName, bData.BorrowersLastName, bData.Street1,
                    bData.Street2, bData.City, bData.ForeignCountry.IsNullOrEmpty() ? bData.State : bData.ForeignState, bData.Zip, bData.ForeignCountry, bData.AccountNumber, DateTime.Now));
            }

            return file;
        }
    }
}
