using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace BARCODEFED
{
    partial class ScannerDialog : Form
    {
        private DataAccess DA;
        private readonly bool HasForwardingAddress;

        //Cache the list of states that we need to pass to the ForwardingDialog's constructor for every piece of mail.
        private readonly List<string> _states;
        WarehouseDemographics Demos { get; set; }
        ReflectionInterface RI { get; set; }
        public DateTime ReceivedDate { get; set; }

        public ScannerDialog(DataAccess da, bool mailIncludesForwardingAddress, ReflectionInterface ri, DateTime receivedDate)
        {
            InitializeComponent();

            RI = ri;
            DA = da;
            HasForwardingAddress = mailIncludesForwardingAddress;
            ReceivedDate = receivedDate;
            _states = DA.GetStateCodes();
            _states.Insert(0, "");
            toolTip.SetToolTip(btnNoBarcode, "Allows you to manually enter the return mail information if there is not a barcode or if the script cannot read the barcode.");
            if (mailIncludesForwardingAddress)
            {
                toolTip.SetToolTip(btnProcess, "Invalidates/updates the addresses for the barcodes you've scanned, plus any that were previously scanned.");
                toolTip.SetToolTip(btnQuit, "Stops the script without invalidating/updating any addresses. Barcodes that have been scanned will have their addresses invalidated/updated the next time the script runs.");
            }
            else
            {
                toolTip.SetToolTip(btnProcess, "Invalidates the addresses for the barcodes you've scanned, plus any that were previously scanned.");
                toolTip.SetToolTip(btnQuit, "Stops the script without invalidating any addresses. Barcodes that have been scanned will have their addresses invalidated the next time the script runs.");
            }
        }

        private void HandleBarcodeInput(string barcodeInput)
        {
            //Parse the barcode input.
            BarcodeInfo barcodeInfo;
            try
            {
                barcodeInfo = BarcodeInfo.Parse(barcodeInput);
            }
            catch (Exception ex)
            {
                string message = ex.Message + " Click OK to handle the letter as if it has no barcode.";
                if (MessageBox.Show(message, "Bad Barcode", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) { HandleNoBarcode(); }
                return;
            }
            if (!DA.CheckBorrowerRegion(barcodeInfo.RecipientId))
            {
                Dialog.Error.Ok($"The scanned {barcodeInfo.LetterId} document for borrower {barcodeInfo.RecipientId} was not found in the FED region.");
                return;
            }

            //Create a ForwardingInfo object from the barcode input, and get the forwarding address from the user if needed.
            ForwardingInfo forwardingInfo = new ForwardingInfo(barcodeInfo);
            if (HasForwardingAddress)
            {
                //Look up the borrower's address in the warehouse.
                Demos = DA.GetDemographicsFromWarehouse(forwardingInfo.RecipientId);
                if (Demos != null && (Demos.AddressVerifiedDate.ToDate().Date < barcodeInfo.CreateDate.Date))
                {
                    forwardingInfo.Address1 = Demos.Address1;
                    forwardingInfo.Address2 = Demos.Address2;
                    forwardingInfo.City = Demos.City;
                    forwardingInfo.State = Demos.State;
                    forwardingInfo.Zip = Demos.ZIP;
                    forwardingInfo.Country = Demos.Country;

                    using (ForwardingDialog forwardingDialog = new ForwardingDialog(_states, forwardingInfo, Demos.AddressVerifiedDate))
                    {
                        if (forwardingDialog.ShowDialog() != DialogResult.OK) { return; }
                    }
                }
                else if (Demos == null)
                {
                    string message = $"Scanned recipient ID: {barcodeInfo.RecipientId} was not found in the system. Please contact System Support";
                    Dialog.Error.Ok(message, "Borrower Not Found");
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
            }


            forwardingInfo.ReceivedDate = ReceivedDate;
            //Save the info to the database to be processed later, and show the barcode info in the status box.
            SaveLetterInfo(forwardingInfo);
            UpdateStatusText(barcodeInfo);
        }

        private void HandleNoBarcode()
        {
            BarcodeInfo barcodeInfo = new BarcodeInfo();
            string barcodeInfoString = "";
            string borrowerSsn = null;
            using (ReturnMail rm = new ReturnMail(RI, barcodeInfoString, DA))
            {
                if (rm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                barcodeInfo.CreateDate = rm.LetterCreateDate;
                barcodeInfo.LetterId = rm.Letter;
                barcodeInfo.RecipientId = rm.SelectedAccountIdentifier;
                barcodeInfo.CreateDate = rm.LetterCreateDate;
                barcodeInfo.ReceivedDate = rm.LetterReturnDate;

                if (rm.AccountIdentifier.Contains("P"))
                    borrowerSsn = GetSsnForReference(rm.AccountIdentifier, RI); //replace me with the borrower so that I work
                else if (rm.AccountIdentifier.Length == 10)  //Get and convert the account identifier input on the return mail screen to an ssn
                    borrowerSsn = DA.GetSsnFromAcct(rm.AccountIdentifier);
                else if (rm.AccountIdentifier.Length == 9)
                    borrowerSsn = rm.AccountIdentifier;
            }
            while (!RI.IsLoggedIn)
            {
                MessageBox.Show("Please log back into the session, and press insert to continue.");
                RI.PauseForInsert();
            }
            //Set up the forwarding info with the manual input results
            ForwardingInfo forwardingInfo = new ForwardingInfo(barcodeInfo);
            forwardingInfo.BorrowerSsn = borrowerSsn;

            BorrowerDemos bData = new BorrowerDemos();
            var selectedDemos = DA.GetDemographicsFromWarehouse(barcodeInfo.RecipientId);
            if (selectedDemos == null)
            {
                MessageBox.Show($"Unable to locate recipient {barcodeInfo.RecipientId}");
                return;
            }
            //Get the person type of the selected borrower
            List<string> personTypes = GetPersonType(selectedDemos.SSN);
            string personType;
            if (personTypes.Count > 1)
            {
                using (SelectPersonType pt = new SelectPersonType(personTypes))
                {
                    if (pt.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show($"Unable to determine person type for recipient {barcodeInfo.RecipientId}");
                        return;
                    }
                    personType = pt.SelectedPersonType;
                }
            }
            else if (personTypes.Count == 1)
                personType = personTypes[0];
            else
            {
                MessageBox.Show($"Unable to determine person type for recipient {barcodeInfo.RecipientId}");
                return;
            }

            try
            {
                //Set the forwarding info and barcode info person types to the result the ITX1J scrape
                barcodeInfo.PersonType = StringToPersonType(personType);
                forwardingInfo.PersonType = BarcodeScanner.PersonTypeToString(barcodeInfo.PersonType);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Unable to determine person type for recipient {barcodeInfo.RecipientId}");
                return;
            }

            //Set up the barcode data with the selected account information
            NoBarcodeData nBd = new NoBarcodeData() { AccountNum = selectedDemos.AccountNumber/*barcodeInfo.RecipientId*/, SSN = selectedDemos.SSN, Name = selectedDemos.FullName };
            if (HasForwardingAddress)
            {
                //Look up the borrower's address in the warehouse.
                Demos = DA.GetDemographicsFromWarehouse(barcodeInfo.RecipientId);
                if (Demos == null)
                {
                    string message = $"Scanned recipient ID: {barcodeInfo.RecipientId} was not found in the system. Please contact System Support";
                    Dialog.Error.Ok(message, "Borrower Not Found");
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
                else
                {
                    bData.Address1 = Demos.Address1;
                    bData.Address2 = Demos.Address2;
                    bData.City = Demos.City;
                    bData.State = Demos.State;
                    bData.Zip = Demos.ZIP;
                    bData.AddrValid = Demos.AddressValidityIndicator;
                    bData.AddrEffectiveDate = Demos.AddressVerifiedDate;

                    using (DemographicUpdateScreen demosDialog = new DemographicUpdateScreen(bData, nBd, forwardingInfo, DA))
                    {
                        if (demosDialog.ShowDialog() != DialogResult.OK)
                        {
                            txtScannerInput.Focus();
                            return;
                        }
                    }
                }
            }
            if (SaveLetterInfo(forwardingInfo))
                UpdateStatusText(barcodeInfo);

            txtScannerInput.Focus();
        }

        private bool SaveLetterInfo(ForwardingInfo forwardingInfo)
        {
            try
            {
                if (DA.SaveScannedInfo(forwardingInfo))
                    return true;
            }
            catch (Exception ex)
            {
                string message = "There was an error saving the letter info to the database. Please copy the following text and send it to Systems Support:" + Environment.NewLine + ex.ToString();
                MessageBox.Show(message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
            return false;
        }

        private void UpdateStatusText(BarcodeInfo info)
        {
            if (txtStatus.TextLength > 0) { txtStatus.AppendText(Environment.NewLine); }
            txtStatus.AppendText($"Recipient ID: {info.RecipientId}");
            txtStatus.AppendText(Environment.NewLine);
            txtStatus.AppendText($"Letter ID: {info.LetterId}");
            txtStatus.AppendText(Environment.NewLine);
            txtStatus.AppendText(string.Format("Date sent: {0:MM/dd/yyyy}", info.CreateDate));
            txtStatus.AppendText(Environment.NewLine);
        }

        private void btnNoBarcode_Click(object sender, EventArgs e)
        {
            HandleNoBarcode();
            txtScannerInput.Focus();
        }

        private void ScannerDialog_Shown(object sender, EventArgs e)
        {
            txtScannerInput.Focus();
        }

        private void txtScannerInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtScannerInput.TextLength > 0)
            {
                HandleBarcodeInput(txtScannerInput.Text);
                txtScannerInput.Clear();
                txtScannerInput.Focus();
            }
        }

        private List<string> GetPersonType(string ssn)
        {
            List<string> personTypes = new List<string>();
            foreach (string type in new string[] { "B", "E", "R" })
            {
                RI.FastPath($"TX3Z/ITX1J{type}{ssn}");
                if (RI.CheckForText(1, 71, "TXX1R"))
                {
                    personTypes.Add(type);
                }
            }
            return personTypes;
        }

        public BarcodeScanner.PersonType StringToPersonType(string personType)
        {
            if (personType == "B")
            {
                return BarcodeScanner.PersonType.Borrower;
            }
            else if (personType == "E")
            {
                return BarcodeScanner.PersonType.Endorser;
            }
            else if (personType == "R")
            {
                return BarcodeScanner.PersonType.Reference;
            }
            else
            {
                throw new FormatException("Unable to convert string to person type");
            }
        }

        private string GetSsnForReference(string referenceId, ReflectionInterface ri)
        {
            if (referenceId.Contains("P"))
            {
                RI.FastPath($"TX3Z/ITX1JR{referenceId}");
                if (RI.CheckForText(1, 71, "TXX1R"))
                {
                    //SSN of the borrower the reference is for
                    return RI.GetText(7, 11, 11).Replace(" ", "");
                }
            }
            return null;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {

        }
    }
}