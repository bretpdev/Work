using BCSRETMAIL.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.Dialog;

namespace BCSRETMAIL
{
    partial class ScannerDialog : Form
    {
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public List<Account> Accounts { get; set; }
        public Account NewAddress { get; set; }
        public Account InvalidAddress { get; set; }
        public Func<ProcessedDocuments, object> Predicate { get; set; }
        public bool IsDescending { get; set; }
        public int TotalProcessed { get; set; }
        public List<string> LetterIds { get; set; }
        public bool WasScanned { get; set; }
        public bool HasForwarding { get; set; }

        public ScannerDialog(ProcessLogRun logRun)
        {
            InitializeComponent();
            DA = new DataAccess(logRun);
            LogRun = logRun;
            Predicate = p => p.AddedAt;
            SetDateSelection();
            LoadProcessedRecords();
            Accounts = new List<Account>();
            LetterIds = DA.GetLetterIds();
            LetterIds.Insert(0, "");
            ReceivedDate.Value = ReceivedDate.MaxDate = DateTime.Now;
            LoadReturnReasons();
            FocusText.Start();
            this.Text = $"{this.Text} :: Version: {Assembly.GetExecutingAssembly().GetName().Version}";
        }

        /// <summary>
        /// Make sure the ScannerInputTxt is always in focus
        /// </summary>
        private void ScannerDialog_Shown(object sender, EventArgs e)
        {
            FocusText.Start();
        }

        /// <summary>
        /// Make sure the ScannerInputTxt is always in focus
        /// </summary>
        private void ScannerDialog_MouseClick(object sender, MouseEventArgs e)
        {
            FocusText.Start();
        }

        private void ScannerInputTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter) && ScannerInputTxt.Text.IsPopulated())
            {
                BorrowerName.Text = "";
                BarcodeInfo info;
                try
                {
                    FocusText.Stop();
                    info = BarcodeInfo.Parse(ScannerInputTxt.Text);
                }
                catch (Exception ex)
                {
                    InvalidBarcode(ex);
                    FocusText.Start();
                    return;
                }
                WasScanned = true;

                if (info != null)
                {
                    if (info.RecipientId.IsAlpha()) //Decrypt ACS encryption code
                        info.RecipientId = DecryptAcs(info.RecipientId);
                    Accounts = GetAvailableAccounts(info.RecipientId);
                    if (Accounts.Count > 0)
                    {
                        info.ReceivedDate = ReceivedDate.Text.ToDate();
                        info.System = Accounts.Count == 2 ? BarcodeInfo.SystemType.Both : Accounts.FirstOrDefault().System;
                        if (SaveRecord(info, false))
                            ClearControls();
                    }
                    else
                        InvalidBarcode(new Exception($"The scanned borrower was not found in Onelink and there are no open loans in Compass. Scanned data: {ScannerInputTxt.Text}"));
                }
            }
        }

        /// <summary>
        /// Gets the borrower if they are in both compass and onelink. Checks for a balance in compass and returns either both regions or just one.
        /// </summary>
        private List<Account> GetAvailableAccounts(string recipientId)
        {
            List<Account> accounts = DA.GetAccountIdentifier(recipientId);
            if (accounts.Count == 1 && accounts.First().System == BarcodeInfo.SystemType.Onelink)
                return accounts;
            else if (accounts.Count == 1 && accounts.First().System == BarcodeInfo.SystemType.Compass)
            {
                string accountNumber = recipientId.ToUpper().StartsWith("P") ? recipientId : accounts.First().AccountNumber;
                if (DA.CheckForOpenLoans(accountNumber))
                    return accounts;
                else
                    return new List<Account>();
            }
            else if (accounts.Count > 1)
            {
                string accountNumber = recipientId.ToUpper().StartsWith("P") ? recipientId : accounts.Where(p => p.System == BarcodeInfo.SystemType.Compass).FirstOrDefault().AccountNumber;
                //Checks if the borrower has open loans in Compass. If not, return the onelink account otherwise return both
                if (!DA.CheckForOpenLoans(accountNumber))
                    return accounts.Where(p => p.System == BarcodeInfo.SystemType.Onelink).ToList();
                else
                    return accounts;
            }
            return accounts;
        }

        private void InvalidBarcode(Exception ex = null)
        {
            string message = ex.Message + " Please process letter manually";
            Warning.Ok(message, "Bad Barcode");
            Clear_Click(new object(), new EventArgs());
        }

        private void AccountIdentifier_TextChanged(object sender, EventArgs e)
        {
            ResetNameField();
            if (AccountIdentifier.Text.Length >= 9)
            {
                string enteredData = AccountIdentifier.Text.ToUpper();
                if (enteredData.IsAlpha())
                {
                    enteredData = DecryptAcs(enteredData);
                    AccountIdentifier.Text = enteredData;
                    return;
                }
                Accounts = DA.GetAccountIdentifier(enteredData);
                LoadAccounts();
                if (Accounts.Count > 0)
                {
                    LetterCode.DataSource = LetterIds;
                    LetterCode.DisplayMember = "ID";
                }
            }
            if (AccountIdentifier.Text.IsNullOrEmpty())
                FocusText.Start();
        }

        private void ReturnReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateFields();
            if (ReturnReason.Text.IsPopulated())
                CreateDate.Focus();
        }

        /// <summary>
        /// Validate the fields which will unlock the Add button when everything has been filled out
        /// </summary>
        private void CreateDate_TextChanged(object sender, EventArgs e)
        {
            ValidateFields();
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadProcessedRecords();
            FocusText.Start();
        }

        private void ReceivedDate_ValueChanged(object sender, EventArgs e)
        {
            ValidateFields();
            FocusText.Start();
        }

        private void DateSelection_ValueChanged(object sender, EventArgs e)
        {
            LoadProcessedRecords();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            ClearControls();
            FocusText.Start();
        }

        private void Lookup_Click(object sender, EventArgs e)
        {
            FocusText.Stop();
            using SearchForm search = new SearchForm();
            search.ShowDialog();
            if (search.Borrower != null && search.Borrower.SSN.IsPopulated())
                AccountIdentifier.Text = search.Borrower.SSN;
            else
                FocusText.Start();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            BarcodeInfo info = new BarcodeInfo()
            {
                RecipientId = Accounts.FirstOrDefault().AccountNumber.Trim().IsPopulated() ? Accounts.FirstOrDefault().AccountNumber.Trim() : AccountIdentifier.Text,
                LetterId = LetterCode.Text,
                CreateDate = CreateDate.Text.ToDate(),
                ReceivedDate = ReceivedDate.Text.ToDate(),
                System = Accounts.Count == 2 ? BarcodeInfo.SystemType.Both : Accounts.FirstOrDefault().System
            };
            if (SaveRecord(info, true))
            {
                ClearControls();
                AccountIdentifier.Enabled = true;
                LetterCode.Enabled = true;
                CreateDate.Enabled = true;
                WasScanned = false;
                FocusText.Start();
            }
        }

        private void DailyProcessed_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                    Predicate = p => p.AccountIdentifier;
                    break;
                case 1:
                    Predicate = p => p.LetterId;
                    break;
                case 2:
                    Predicate = p => p.System;
                    break;
                case 3:
                    Predicate = p => p.CreateDate;
                    break;
                case 4:
                    Predicate = p => p.ReceivedDate;
                    break;
                case 5:
                    Predicate = p => p.HasForwarding;
                    break;
                case 6:
                    Predicate = p => p.AddedBy;
                    break;
                case 7:
                    Predicate = p => p.AddedAt;
                    break;
                case 8:
                    Predicate = p => p.ProcessedAt;
                    break;
                case 9:
                    Predicate = p => p.ArcAddProcessingId;
                    break;
                case 10:
                    Predicate = p => p.ArcProcessedAt;
                    break;
            }
            LoadProcessedRecords();
            IsDescending = !IsDescending;
        }

        private void Forward_Click(object sender, EventArgs e)
        {
            FocusText.Stop();
            if ((Accounts != null && Accounts.Count > 0) && Accounts.First().AccountNumber.IsPopulated())
            {
                NewAddress ??= new Account();
                using ForwardingAddress address = new ForwardingAddress(Accounts, NewAddress, DA);
                Hide();
                address.ShowDialog();
                NewAddress = address.NewAddress;
                if (address.InvalidAddress != null)
                {
                    HasForwarding = true;
                    InvalidAddress = address.InvalidAddress;
                }
                Show();
            }
            else
            {
                Info.Ok("You must first enter an account and choose a letter.");
                FocusText.Start();
            }
            if (NewAddress != null && NewAddress.Address1.IsPopulated())
                Check.Image = Resources.check;
            else
                Check.Image = null;
        }

        /// <summary>
        /// Loads the return reason drop down
        /// </summary>
        private void LoadReturnReasons()
        {
            List<string> reasons = DA.GetReturnReasons();
            reasons.Insert(0, "");
            ReturnReason.DataSource = reasons;
        }

        /// <summary>
        /// Loads the accounts found from the manual process
        /// </summary>
        private void LoadAccounts()
        {
            BorrowerName.Text = "";
            // Only search if account identifier and letter code are provided
            if (AccountIdentifier.Text.Length >= 9)
            {
                if (Accounts != null && Accounts.Count > 0)
                {
                    Account acct = Accounts.Where(p => p.System == BarcodeInfo.SystemType.Onelink).FirstOrDefault() ?? Accounts.First();
                    BorrowerName.Text = $"{acct.FirstName} {acct.LastName}\r{acct.Address1}\r{acct.City} {acct.State} {acct.ZipCode}";
                }
                else
                    BorrowerName.Text = "No accounts found for account number and letter id.";
            }
            ValidateFields();
        }

        /// <summary>
        /// Decrypts the Account Number from the scanned barcode
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string DecryptAcs(string value)
        {
            if (value.ToUpper().StartsWith("RF@") || value.ToUpper().StartsWith("P"))
                return value;
            value = value.ToUpper().Replace("M", "0");
            value = value.ToUpper().Replace("Y", "9");
            value = value.ToUpper().Replace("L", "8");
            value = value.ToUpper().Replace("A", "7");
            value = value.ToUpper().Replace("U", "6");
            value = value.ToUpper().Replace("G", "5");
            value = value.ToUpper().Replace("H", "4");
            value = value.ToUpper().Replace("T", "3");
            value = value.ToUpper().Replace("E", "2");
            value = value.ToUpper().Replace("R", "1");

            return value;
        }

        /// <summary>
        /// Saves the scanned/manual input and checks to see if the letter has already been scanned
        /// </summary>
        private bool SaveRecord(BarcodeInfo info, bool manualMail)
        {
            if (NewAddress != null)
                AddNewAddressFields(info);
            if (manualMail)
                info.Comment = GetManualMailComment(info);
            //Gets a ManagedDataResult to check for unique constraint error if letter has already been scanned.
            List<ManagedDataResult<object>> result = DA.SaveScannedInfo(info);
            if (result.Any(p => p.DatabaseCallSuccessful))
                ++TotalProcessed;
            else if (result.Any(p => p.CaughtException.Message.Contains("UNIQUE")))
            {
                Error.Ok("This document has already been scanned.", "Already Scanned");
                if (WasScanned)
                    return true; //Remove all fields if it was scanned
                else
                    return false;
            }
            else
            {
                string message = $"There was an error saving the letter {info.LetterId} for borrower: {info.RecipientId}; Error: {result.FirstOrDefault().CaughtException.Message}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Error.Ok(message);
                return false;
            }
            LoadProcessedRecords();
            return true;
        }

        private void AddNewAddressFields(BarcodeInfo info)
        {
            info.Address1 = NewAddress.Address1;
            info.Address2 = NewAddress.Address2;
            info.City = NewAddress.City;
            info.State = NewAddress.State;
            info.ZipCode = NewAddress.ZipCode;
        }

        private string GetManualMailComment(BarcodeInfo info)
        {
            string badAddress;
            if (InvalidAddress != null)
                badAddress = $"{InvalidAddress.Address1},{InvalidAddress.Address2},{InvalidAddress.City} {InvalidAddress.State} {InvalidAddress.ZipCode}";
            else
            {
                Account acct = Accounts.FirstOrDefault();
                badAddress = $"{acct.Address1},{acct.Address2},{acct.City} {acct.State} {acct.ZipCode}";
            }
            string forwardingAddress = $"{info.Address1},{info.Address2},{info.City} {info.State} {info.ZipCode}";
            return $"RTRNMAIL, For: {info.RecipientId}, {info.LetterId}, {CreateDate.Text:MMddyy},  Received return mail from {badAddress}. New Address {forwardingAddress}  Return Mail Reason: {ReturnReason.Text}";
        }

        public void ResetNameField()
        {
            BorrowerName.Text = "";
            Add.Enabled = false;
        }

        /// <summary>
        /// Close all the controls that are not needed after an insert and remove any data
        /// </summary>
        private void ClearControls()
        {
            Accounts = new List<Account>();
            Add.Enabled = false;
            AccountIdentifier.Text = "";
            LetterCode.Text = "";
            CreateDate.Text = "";
            BorrowerName.Text = "";
            ScannerInputTxt.Text = "";
            NewAddress = null;
            Check.Image = null;
            WasScanned = false;
            AccountIdentifier.Enabled = true;
            LetterCode.Enabled = true;
            CreateDate.Enabled = true;
            ReturnReason.Text = "";
            LetterCode.DataSource = null;
            FocusText.Start();
            NewAddress = null;
            InvalidAddress = null;
        }

        /// <summary>
        /// Loads the Daily Processed grid view and updates the processed count
        /// </summary>
        private void LoadProcessedRecords()
        {
            SetDateSelection();
            List<ProcessedDocuments> documents = DA.GetProcessedDocuments(DateSelection.Value);
            DailyProcessed.DataSource = SetOrder(documents);
            DailyProcessed.Columns[0].HeaderText = "SSN/Acct";
            DailyProcessed.Columns[1].HeaderText = "Letter ID";
            DailyProcessed.Columns[2].HeaderText = "System";
            DailyProcessed.Columns[3].HeaderText = "Created";
            DailyProcessed.Columns[4].HeaderText = "Received";
            DailyProcessed.Columns[5].HeaderText = "Forwarding";
            DailyProcessed.Columns[6].HeaderText = "Added By";
            DailyProcessed.Columns[7].HeaderText = "Added At";
            DailyProcessed.Columns[8].HeaderText = "Processed";
            DailyProcessed.Columns[9].HeaderText = "Arc ID";
            DailyProcessed.Columns[10].HeaderText = "Arc Processed";
            if (documents.Count > 0)
                ProcessedCount.Text = $"Daily Total: {documents.Count}     Session Total: {TotalProcessed}";
            else
                ProcessedCount.Text = "";
            //Need to invoke the Auto Resize for the columns on a new thread
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() =>
                {
                    DailyProcessed.AutoResizeColumns();
                }));
        }

        /// <summary>
        /// Changes between descending and ascending
        /// </summary>
        private List<ProcessedDocuments> SetOrder(List<ProcessedDocuments> documents)
        {
            if (IsDescending)
                return documents.OrderBy(Predicate).ToList();
            return documents.OrderByDescending(Predicate).ToList();
        }

        private void SetDateSelection()
        {
            DateSelection.MinDate = DA.GetEarliestDate();
            DateSelection.MaxDate = DateTime.Now.Date;
        }

        /// <summary>
        /// Validates all the fields are populated before adding the data to the database.
        /// Region will be populated once the borrower has been selected and the letter id is selected.
        /// Reason is only needed when the letter is OneLink
        /// </summary>
        private void ValidateFields()
        {
            if (CreateDate.Text.Length == 10 && CreateDate.Text.ToDateNullable().HasValue && CreateDate.Text.ToDateNullable().Value >= DateTime.Now.Date)
            {
                Info.Ok("The create date can not be a future date.");
                return;
            }
            if (CreateDate.Text.Length == 10 && CreateDate.Text.ToDateNullable().HasValue && CreateDate.Text.ToDateNullable().Value.Date > ReceivedDate.Value.Date)
            {
                Info.Ok("The received date must be after the create date.");
                return;
            }
            if (CreateDate.Text.Length == 10 && !CreateDate.Text.ToDateNullable().HasValue)
            {
                Info.Ok("The create date is not not a the proper format.");
                return;
            }
            if (AccountIdentifier.Text.IsPopulated() && CreateDate.Text.Replace("/", "").Trim().Length == 8 && ReturnReason.Text.IsPopulated())
                Add.Enabled = true;
            else
                Add.Enabled = false;
            if (!WasScanned)
                Forward.Enabled = true;
            else
                Forward.Enabled = false;
        }

        private void FocusText_Tick(object sender, EventArgs e)
        {
            if (AccountIdentifier.Text.IsPopulated() || LetterCode.Text.IsPopulated() || ReturnReason.Text.IsPopulated() || CreateDate.Text.Replace("/", "").Trim().IsPopulated())
                FocusText.Stop();
            else
                ScannerInputTxt.Focus();
        }

        private void AccountIdentifier_Enter(object sender, EventArgs e)
        {
            FiveSecondDelay();
        }

        public void FiveSecondDelay()
        {
            Timer checkTime = new Timer();
            checkTime.Interval = 5000;
            checkTime.Start();
            checkTime.Tick += HitFiveSeconds;
            FocusText.Stop();
        }

        public void HitFiveSeconds(object sender, EventArgs e)
        {
            if (LetterCode.Text.IsNullOrEmpty() && AccountIdentifier.Text.IsNullOrEmpty() && CreateDate.Text.Replace("/", "").Trim().IsNullOrEmpty() && ReturnReason.Text.IsNullOrEmpty())
            {
                Timer t = sender as Timer;
                t.Stop();
                FocusText.Start();
            }
        }

        private void LetterCode_Enter(object sender, EventArgs e)
        {
            FocusText.Stop();
            FiveSecondDelay();
        }

        private void ReturnReason_Enter(object sender, EventArgs e)
        {
            FocusText.Stop();
            FiveSecondDelay();
        }

        private void CreateDate_Enter(object sender, EventArgs e)
        {
            FocusText.Stop();
            FiveSecondDelay();
        }

        private void ReceivedDate_Enter(object sender, EventArgs e)
        {
            FocusText.Stop();
            FiveSecondDelay();
        }

        private void DateSelection_Enter(object sender, EventArgs e)
        {
            FocusText.Stop();
            FiveSecondDelay();
        }

        private void ReceivedDate_Leave(object sender, EventArgs e)
        {
            FocusText.Start();
        }

        private void DateSelection_Leave(object sender, EventArgs e)
        {
            FocusText.Start();
        }

        private void AccountIdentifier_Leave(object sender, EventArgs e)
        {
            FocusText.Start();
        }

        private void LetterCode_Leave(object sender, EventArgs e)
        {
            FocusText.Start();
        }

        private void ReturnReason_Leave(object sender, EventArgs e)
        {
            FocusText.Start();
        }

        private void CreateDate_Leave(object sender, EventArgs e)
        {
            FocusText.Start();
        }
    }
}