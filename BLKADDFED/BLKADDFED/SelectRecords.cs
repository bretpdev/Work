using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;
using BLKADDFED.UserControls;

namespace BLKADDFED
{
    partial class SelectRecords : FormBase
    {
        public DialogResult Result { get; set; }
        public string Comments { get; set; }

        private List<BorrowerAddress> _brwAddresses;
        private List<BorrowerPhone> _brwPhones;

        //constructor
        public SelectRecords(List<BorrowerAddress> brwAddresses, List<BorrowerPhone> brwPhones)
        {
            InitializeComponent();

            _brwAddresses = brwAddresses;
            _brwPhones = brwPhones;

            PopulateControls(brwAddresses, brwPhones);
        }

        //populate controls in flow panels
        private void PopulateControls(List<BorrowerAddress> brwAddresses, List<BorrowerPhone> brwPhones)
        {
            flwAddresses.Controls.Clear();
            foreach (BorrowerAddress i in brwAddresses)
            {
                flwAddresses.Controls.Add(new AddressRecord(i));
            }

            flwPhone.Controls.Clear();
            foreach (BorrowerPhone i in brwPhones)
            {
                flwPhone.Controls.Add(new PhoneRecord(i));
            }
        }


        //OK button click
        private void btnOK_Click(object sender, EventArgs e)
        {
            //user has selected records
            if (btnOK.Text == "OK")
            {
                //get numbers of records selected
                int addressesSelected = (from p in _brwAddresses where p.Selected == true select p).Count();
                int phonesSelected = (from p in _brwPhones where p.Selected == true select p).Count();

                //warn the user if no records were selected
                if (addressesSelected == 0 && phonesSelected == 0)
                {
                    MessageBox.Show("No records were selected.", "No Records Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //warn the user if too many records were selected
                else if (addressesSelected > 1 || phonesSelected > 3)
                {
                    MessageBox.Show("You may only select 1 address and up to 3 phone numbers.", "Too Many Records Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //refresh screen to display selected records to be verified
                else
                {
                    List<BorrowerAddress> brwAddressesSelected = new List<BorrowerAddress>();
                    List<BorrowerPhone> brwPhonesSelected = new List<BorrowerPhone>();

                    brwAddressesSelected = (from p in _brwAddresses where p.Selected == true select p).ToList();
                    brwPhonesSelected = (from p in _brwPhones where p.Selected == true select p).ToList();

                    PopulateControls(brwAddressesSelected, brwPhonesSelected);

                    lblInstructions.Text = "Review the addresses and/or phone numbers below.  Click Yes if the correct records were selected, click No to clear the records selected and select the correct records, or click Cancel to end the script.";
                    btnOK.Text = "Yes";
                    btnNo.Visible = true;
                }
            }
            //user has verified records selected, refresh screen to display comments text box
            else if (btnOK.Text == "Yes")
            {
                lblInstructions.Text = "Enter your comments below.  Click Continue to add activity records with the selected records and your comments or click Cancel to end the script.";
                lblAddresss.Text = "Comments";
                txtComments.Visible = true;
                flwAddresses.Visible = false;
                flwPhone.Visible = false;
                btnOK.Text = "Continue";
                btnNo.Visible = false;
            }
            //user has entered comments
            else
            {
                Comments = txtComments.Text;
                Result = DialogResult.OK;
                this.Close();
            }
        }

        //No button click
        private void btnNo_Click(object sender, EventArgs e)
        {
            //reset selection indicator for all records
            foreach (BorrowerAddress i in _brwAddresses)
            {
                i.Selected = false;
            }
            foreach (BorrowerPhone i in _brwPhones)
            {
                i.Selected = false;
            }

            //repopulate controls with all records
            PopulateControls(_brwAddresses, _brwPhones);

            //refresh controls so user can select records
            lblInstructions.Text = "Select up to 1 address and/or up to 3 phone numbers.  Click OK to continue or Cancel to end the script.";
            btnOK.Text = "OK";
            btnNo.Visible = false;
        }

        //Cancel button click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

    }


}
