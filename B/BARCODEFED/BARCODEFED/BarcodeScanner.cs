using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace BARCODEFED
{
    public class BarcodeScanner : FedScript
    {
        public enum PersonType { Borrower, Endorser, Reference, NONE }
        public ProcessLogRun LogRun { get; set; }
        private DataAccess DA { get; set; }
        //Keep a cache of letter IDs and whether they're required to be forwarded.
        private readonly Dictionary<string, bool> RequiredLetter;

        public BarcodeScanner(ReflectionInterface ri)
            : base(ri, "BARCODEFED")
        {
            LogRun = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode, true);
            DA = new DataAccess(LogRun);
            RequiredLetter = new Dictionary<string, bool>();
            RI.LogRun = LogRun;
        }

        public override void Main()
        {
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
                return;

            //Have the user scan in or manually enter as many letters as he/she sees fit.
            bool forward = (MessageBox.Show("Does the Return Mail Include Forwarding Address?", "Return Mail FED", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);

            //See if the user has scanned/entered any letters. Keep hounding them until they scan/enter something or cancel out.
            IEnumerable<ForwardingInfo> records = new List<ForwardingInfo>();
            do
            {
                ReceivedDate rDate = new ReceivedDate();
                rDate.ShowDialog();
                //Dialog.Info.Ok("There were no records to process.");
                using (ScannerDialog scannerdialog = new ScannerDialog(DA, forward, RI, rDate.selectedDate.Value))
                {
                    if (scannerdialog.ShowDialog() != DialogResult.OK) { return; }
                }
                records = DA.GetRecordsToProcess();
                if (records.Count() == 0)
                {
                    Dialog.Info.Ok("There were no records to process.");
                }
            } while (records.Count() == 0);

            //Update the system for all letters the user has scanned/entered.
            if (!RI.IsLoggedIn)
            {
                string loginMessage = "You are no longer logged into a Reflection session. Please log in to continue processing.";
                MessageBox.Show(loginMessage, "Session Timed Out", MessageBoxButtons.OK, MessageBoxIcon.Information);
                while (!RI.IsLoggedIn)
                    Login();
            }

            foreach (ForwardingInfo record in records)
            {
                List<int> loanSeq = new List<int>();
                //If the forwarding info does not have a value in the table
                //determine how to process them by looking at their ITX1J records
                //take the first person type in the order B -> E -> R
                if (record.PersonType == null)
                    if (!GetPersonType(record)) //Sets the person type and borrowerSsn for a record if they are not populated
                        return;

                if (!DA.CheckBorrowerRegion(record.RecipientId))
                {
                    string message = $"The SSN or Account Number for {record.RecipientId} was not found in the system.";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    continue;
                }
                bool hasForwardingAddress = record.Address1.IsPopulated();
                try
                {
                    if (record.Ssn == null && record.RecipientId.Length == 9)
                        record.Ssn = record.RecipientId;
                    else if (record.Ssn == null)
                        record.Ssn = DA.GetSsnFromAcct(record.RecipientId);
                }
                catch (Exception)
                {
                    record.Ssn = string.Empty;//if borrower is not found in CDW it is probably an endorser
                }
                //Need to get loan sequence for endorsers
                if (record.PersonType == "E")
                {
                    loanSeq = GetLoanSeqForEnd(record.Ssn);
                    if (loanSeq.Any())
                        loanSeq = loanSeq.Distinct().ToList();
                }
                bool invalidated = InvalidateAddress(record.Ssn, record.CreateDate);
                AddComments(record, invalidated, loanSeq);
                if (hasForwardingAddress)
                {
                    AddArcToResend(record, loanSeq);
                    AddArcToUpdateAddress(record, loanSeq);
                }
                AddCABorToBatchEmail(record.RecipientId, record.PersonType == "B");
                DA.SetProcessed(record.ReturnMailId);
            }
            MessageBox.Show("Processing Complete", "Return Mail FED");
        }

        private bool GetPersonType(ForwardingInfo record)
        {
            WarehouseDemographics demo = DA.GetDemographicsFromWarehouse(record.RecipientId);
            if (demo == null)
            {
                string message = $"Scanned recipient ID: {record.RecipientId} was not found in the system. Please contact System Support";
                Dialog.Error.Ok(message, "Borrower Not Found");
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
            }
            else
            {
                record.Ssn = demo.SSN;
                RI.FastPath("TX3ZITX1J;" + record.Ssn);

                string personType = null;
                string borrowerSsn = null;
                foreach (string type in new string[] { "B", "E", "R" })
                {
                    RI.FastPath($"TX3Z/ITX1J{type}{record.Ssn}");
                    if (RI.CheckForText(1, 71, "TXX1R"))
                    {
                        if (type == "E" || type == "R")
                        {
                            borrowerSsn = RI.GetText(7, 11, 11).Replace(" ", "");
                        }
                        personType = type;
                        break;
                    }
                }
                record.PersonType = personType;
                record.BorrowerSsn = borrowerSsn;
                if (personType.IsNullOrEmpty())
                {
                    LogRun.AddNotification($"Unable to determine person type of recipient {record.RecipientId}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
            }
            return true;
        }

        private List<int> GetLoanSeqForEnd(string ssn)
        {
            List<int> loanSeq = new List<int>();
            RI.FastPath($"TX3Z/ITX1JE{ssn}");
            if (RI.CheckForText(1, 71, "TXX1R"))
            {
                RI.Hit(ReflectionInterface.Key.F2);
                RI.Hit(ReflectionInterface.Key.F4);
                for (int row = 10; RI.MessageCode != "90007"; row++)
                {
                    if (row > 21 || RI.CheckForText(row, 2, "  "))
                    {
                        row = 9;
                        RI.Hit(ReflectionInterface.Key.F8);
                        continue;
                    }

                    if (RI.CheckForText(row, 5, "E") && RI.CheckForText(row, 13, ssn) && RI.CheckForText(row, 78, "A"))
                        loanSeq.Add(RI.GetText(row, 9, 3).ToInt());
                }
            }
            return loanSeq;
        }

        /// <summary>
        /// If borrower is in California, they need to be sent an email notifiying them that we recieved a document.
        /// </summary>
        /// <param name="forwardingInfo"></param>
        private void AddCABorToBatchEmail(string recipientId, bool isBorrower)
        {
            //Skip emailing references
            if (!recipientId.ToUpper().Contains("P"))
            {
                List<EmailData> emailData = DA.GetCAEmailData(recipientId, isBorrower);
                if (emailData != null)
                {
                    List<EmailData> borEmailData = new List<EmailData>(emailData.Where(p => p.Priority == 0 || p.Priority == 1));
                    List<EmailData> endEmailData = new List<EmailData>(emailData.Where(p => p.Priority == 2 || p.Priority == 3));
                    if (borEmailData.Count > 0)
                        DA.InsertEmailBatch(borEmailData, true);
                    if (endEmailData.Count > 0)
                        DA.InsertEmailBatch(endEmailData, false);
                }
            }
        }

        private void Login()
        {
            LoginForm login = new LoginForm();
            login.ShowDialog();
            if (login.DialogResult == DialogResult.OK)
                RI.Login(login.UserName, login.Password);
            login.Dispose();
        }

        private void AddArcToResend(ForwardingInfo record, List<int> loanSeq)
        {
            //Update the cache if needed for this letter ID.
            if (!RequiredLetter.ContainsKey(record.LetterId))
            {
                if (!record.LetterId.TrimRight(" ").IsNullOrEmpty())
                {
                    bool required = DA.LetterIsRequired(record.LetterId);
                    RequiredLetter.Add(record.LetterId, required);
                }
            }

            //See if we need to re-send the letter.
            if (!RequiredLetter.ContainsKey(record.LetterId))
                return;
            //We need to check if the mapping is false if it exists
            if (RequiredLetter[record.LetterId] == false)
                return;

            if (record.PersonType == "B")
                AddArcToResendBorrower(record);
            else if (record.PersonType == "E")
                AddArcToResendEndorser(record, loanSeq);
            else if (record.PersonType == "R")
                AddArcToResendReference(record);
        }

        private void AddArcToResendBorrower(ForwardingInfo record)
        {
            ArcAddResults arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                Arc = "RMLS",
                AccountNumber = record.RecipientId,
                Comment = record.LetterId,
                ScriptId = ScriptId,
                RecipientId = record.Ssn,
                IsEndorser = false,
                IsReference = false
            }.AddArc();

            if (!arc.ArcAdded)
            {
                string message = $"There as an error adding RMLS ARC to borrower: {record.RecipientId}; EX: {arc.Ex}";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, arc.Ex);
            }
        }

        private void AddArcToResendEndorser(ForwardingInfo record, List<int> loanSeq)
        {
            ArcAddResults arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Arc = "RMLS",
                AccountNumber = record.BorrowerSsn,
                Comment = record.LetterId,
                ScriptId = ScriptId,
                RecipientId = record.BorrowerSsn,
                IsEndorser = true,
                IsReference = false,
                LoanSequences = loanSeq,
                RegardsTo = record.Ssn,
                RegardsCode = "E"
            }.AddArc();

            if (!arc.ArcAdded)
            {
                string message = $"There as an error adding RMLS ARC to borrower: {record.RecipientId}; EX: {arc.Ex}";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, arc.Ex);
            }
        }

        private void AddArcToResendReference(ForwardingInfo record)
        {
            ArcAddResults arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                Arc = "RMLS",
                AccountNumber = record.BorrowerSsn,
                Comment = record.LetterId,
                ScriptId = ScriptId,
                //RecipientId = (record.PersonType == "B" ? "" : record.Ssn),
                IsEndorser = false,
                IsReference = true,
                RegardsTo = record.Ssn,
                RegardsCode = "R"
            }.AddArc();

            if (!arc.ArcAdded)
            {
                string message = $"There as an error adding RMLS ARC to borrower: {record.RecipientId}; EX: {arc.Ex}";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, arc.Ex);
            }
        }

        private void AddArcToUpdateAddress(ForwardingInfo record, List<int> loanSeq)
        {
            if (record.PersonType == "B")
                AddBorrowerAddressUpdateArc(record);
            else if (record.PersonType == "E")
                AddEndorserAddressUpdateArc(record, loanSeq);
            else if (record.PersonType == "R")
                AddReferenceAddressUpdateArc(record);
        }

        private void AddBorrowerAddressUpdateArc(ForwardingInfo record)
        {
            string comment = $"Post Office,{record.Address1},{record.Address2},{record.City},{record.State},{record.Zip},,,,";
            //Add the "RTFWD" arc for borrowers
            string arc = "RTFWD";
            ArcAddResults arcResult = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Arc = arc,
                AccountNumber = record.RecipientId,
                Comment = comment,
                ScriptId = ScriptId,
                RecipientId = record.Ssn,
                IsEndorser = false,
                IsReference = false
            }.AddArc();

            if (!arcResult.ArcAdded)
            {
                string message = $"There was an error adding the {arc} ARC for borrower: {record.RecipientId}; EX: {arcResult.Ex}";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, arcResult.Ex);
            }
        }

        private void AddEndorserAddressUpdateArc(ForwardingInfo record, List<int> loanSeq)
        {
            string comment = $"Post Office,{record.Address1},{record.Address2},{record.City},{record.State},{record.Zip},,,,";
            //Add the "RTFWD" arc for endorsers
            string arc = "RTFWD";
            ArcAddResults arcResult = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Arc = arc,
                AccountNumber = record.BorrowerSsn,
                Comment = comment,
                ScriptId = ScriptId,
                RecipientId = record.BorrowerSsn,
                IsEndorser = true,
                IsReference = false,
                RegardsTo = record.Ssn,
                RegardsCode = "E",
                LoanSequences = loanSeq
            }.AddArc();

            if (!arcResult.ArcAdded)
            {
                string message = $"There was an error adding the {arc} ARC for borrower: {record.RecipientId}; EX: {arcResult.Ex}";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, arcResult.Ex);
            }
        }

        private void AddReferenceAddressUpdateArc(ForwardingInfo record)
        {
            string comment = $"Post Office,{record.Address1},{record.Address2},{record.City},{record.State},{record.Zip},,,,";
            string arc = "RFFWD";
            ArcAddResults arcResult = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Arc = arc,
                AccountNumber = record.BorrowerSsn,
                Comment = comment,
                ScriptId = ScriptId,
                RecipientId = record.Ssn,
                IsEndorser = false,
                IsReference = true,
                RegardsTo = record.Ssn,
                RegardsCode = "R"
            }.AddArc();

            if (!arcResult.ArcAdded)
            {
                string message = $"There was an error adding the {arc} ARC for borrower: {record.RecipientId}; EX: {arcResult.Ex}";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, arcResult.Ex);
            }
        }

        private void AddComments(ForwardingInfo record, bool invalidatedAddress, List<int> loanSeq)
        {
            if (record.PersonType == "B")
                AddBorrowerComment(record, invalidatedAddress);
            else if (record.PersonType == "E")
                AddEndorserComment(record, invalidatedAddress, loanSeq);
            else if (record.PersonType == "R")
                AddReferenceComment(record, invalidatedAddress);
        }

        private void AddBorrowerComment(ForwardingInfo record, bool invalidatedAddress)
        {
            //Use the S4LRM for borrower comments
            string arc = "S4LRM";
            string comment = "Received return mail from the post office";
            if (invalidatedAddress) { comment += ", address invalidated"; }
            comment += string.Format(" {0} {1:MM/dd/yyyy}", record.LetterId, record.CreateDate);

            ArcAddResults arcResult = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Arc = arc,
                AccountNumber = record.RecipientId,
                Comment = comment,
                ScriptId = ScriptId,
                //RecipientId = record.Ssn,
                IsEndorser = false,
                IsReference = false
            }.AddArc();

            if (!arcResult.ArcAdded)
            {
                string message = $"There was an error adding the {arc} ARC for borrower: {record.RecipientId}; EX: {arcResult.Ex}";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, arcResult.Ex);
            }
        }

        private void AddEndorserComment(ForwardingInfo record, bool invalidatedAddress, List<int> loanSeq)
        {
            //Use the SELRM arc for endorser comments
            string arc = "SELRM";
            string comment = "Received return mail from the post office";
            if (invalidatedAddress) { comment += ", address invalidated"; }
            comment += string.Format(" {0} {1:MM/dd/yyyy}", record.LetterId, record.CreateDate);

            ArcAddResults arcResult = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Arc = arc,
                AccountNumber = record.BorrowerSsn,
                Comment = comment,
                ScriptId = ScriptId,
                RecipientId = record.Ssn,
                IsEndorser = true,
                IsReference = false,
                RegardsTo = record.Ssn,
                RegardsCode = "E",
                LoanSequences = loanSeq
            }.AddArc();

            if (!arcResult.ArcAdded)
            {
                string message = $"There was an error adding the {arc} ARC for borrower: {record.RecipientId}; EX: {arcResult.Ex}";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, arcResult.Ex);
            }
        }

        private void AddReferenceComment(ForwardingInfo record, bool invalidatedAddress)
        {
            //Use the S4LRM arc for Reference comments
            string arc = "S4LRM";
            string comment = "Received return mail from the post office";
            if (invalidatedAddress) { comment += ", address invalidated"; }
            comment += string.Format(" {0} {1:MM/dd/yyyy}", record.LetterId, record.CreateDate);

            ArcAddResults arcResult = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                Arc = arc,
                AccountNumber = record.BorrowerSsn,
                Comment = comment,
                ScriptId = ScriptId,
                //RecipientId = record.Ssn,
                IsEndorser = false,
                IsReference = true,
                RegardsTo = record.RecipientId,
                RegardsCode = "R"
            }.AddArc();

            if (!arcResult.ArcAdded)
            {
                string message = $"There was an error adding the {arc} ARC for borrower: {record.RecipientId}; EX: {arcResult.Ex}";
                Dialog.Error.Ok(message);
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, arcResult.Ex);
            }
        }

        private bool InvalidateAddress(string ssn, DateTime letterCreateDate)
        {
            RI.FastPath("TX3ZITX1J;" + ssn);
            DateTime lastVerifiedDate = DateTime.Parse(RI.GetText(10, 32, 8));
            if (letterCreateDate < lastVerifiedDate)
                return false;
            else
            {
                //Invalidate the address.
                RI.PutText(1, 4, "C", ReflectionInterface.Key.Enter);
                RI.Hit(ReflectionInterface.Key.F6);
                RI.Hit(ReflectionInterface.Key.F6);
                if (RI.CheckForText(1, 9, "B"))//SOURCE CODE is only available for a borrower
                    RI.PutText(8, 18, "25"); //SOURCE: CODE
                RI.PutText(10, 32, DateTime.Now.ToString("MMddyy")); //ADDR LAST VER
                RI.PutText(11, 55, "N", ReflectionInterface.Key.Enter); //ADDR VALID
                return true;
            }
        }

        public static string PersonTypeToString(PersonType p)
        {
            if (p == PersonType.Borrower)
                return "B";
            else if (p == PersonType.Endorser)
                return "E";
            else if (p == PersonType.Reference)
                return "R";
            return null;
        }

        public static PersonType? StringToPersonType(string s)
        {
            if (s == null)
                return null;
            else if (s == "B")
                return PersonType.Borrower;
            else if (s == "E")
                return PersonType.Endorser;
            else if (s == "R")
                return PersonType.Reference;
            else
                return null;
        }
    }
}