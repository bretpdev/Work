using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.WinForms;
using Uheaa.Common;

namespace PAYARRREHB
{
    public partial class ArrangementForm : Form
    {
        PayArrangeRehab Pay { get; set; }

        DataAccess DA { get; set; }

        string SsnForSetup { get; set; }

        public string SsnForCobor { get; set; }

        public ArrangementForm(PayArrangeRehab pay, DataAccess da)
        {
            InitializeComponent();
            Pay = pay;
            DA = da;
        }

        public ArrangementForm(PayArrangeRehab pay, string cobSsn, DataAccess da)
        {
            InitializeComponent();
            Pay = pay;
            DA = da;
            SsnForCobor = cobSsn;
            this.SSN.Text = cobSsn;
            this.Verify.Enabled = true;
        }


        public List<string> GetDueDates()
        {
            List<string> dates = new List<string>();

            for (int i = 0; i < 60; i++) //review each date in the next two months
            {
                var date = DateTime.Now.AddDays(i);
                if (new int[] { 1, 7, 15, 22 }.Contains(date.Day))
                    dates.Add(date.ToLongDateString());
            }
            dates.Insert(0, "");
            return dates;
        }

        /// <summary>
        /// Validates the SSN control to makes sure it has 9 or 10 digits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Verify_Click(object sender, EventArgs e)
        {
            if (SSN.Text.Length == 9)
                Pay.Payment.SSN = SSN.Text;
            else
                Pay.Payment.AccountNumber = SSN.Text;

            SsnForSetup = "";
            List<CoBorrower> cobors = DA.GetCoborrowers(SSN.Text);
            bool process = false;
            if (cobors.Count > 0)
                process = Dialog.Info.YesNo("This borrower has coborrowers. Process coborrowers at this time?", "Confirm");

            if (!process)
                ProgramState.ProcessCoborrowers = false;
            else
                ProgramState.ProcessCoborrowers = true;



            List<CobInfo> cobs = DA.CoborrowerHasLoans(SSN.Text);
            if (cobs.Count > 0)
            {
                if (!Dialog.Info.YesNo("This is a coborrower account who is also a borrower. Select YES if you want to process the coborrowers loans, NO to process the borrowers loans.\nNOTE: This only applies to Setup."))
                {
                    ProgramState.ProcessCoborrowerLoans = true;
                    SsnForSetup = SSN.Text;
                    SSN.Text = cobs[0].BorSsn;
                    if (SSN.Text.Length == 9)
                        Pay.Payment.SSN = cobs[0].BorSsn;
                    else
                        Pay.Payment.AccountNumber = cobs[0].BorSsn;
                }
            }


            if (Pay.HasLP22)
                TypeSelection.Enabled = true;
            else
            {
                SSN.Text = "";
                return;
            }
        }

        /// <summary>
        /// Checks the number of characters in the SSN field and enables/disables the verify button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SSN_TextChanged(object sender, EventArgs e)
        {
            if (SSN.Text.Length > 8)
                Verify.Enabled = true;
            else
                Verify.Enabled = false;
        }

        /// <summary>
        /// Notifies the user that they are about to generate a rehab agrreement and enables the Send Type group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Generate_CheckedChanged(object sender, EventArgs e)
        {
            if (Generate.Checked)
            {
                if (SsnForSetup.IsPopulated())
                    if (SsnForSetup.Length == 9)
                        Pay.Payment.SSN = SsnForSetup;
                    else
                        Pay.Payment.AccountNumber = SsnForSetup;

                DueDate.DataSource = null; //Clear any data in the combobox and reset the datasource
                DueDate.DataSource = GetDueDates();
                ClearForm();
                if (!Pay.HasLC05)
                {
                    Generate.Checked = false;
                    TypeSelection.Enabled = false;
                    return;
                }

                if (Dialog.Info.OkCancel("You have chosen to generate an arrangement and send a Rehab Agreement to the borrower.", "Generate Rehab"))
                {
                    Pay.Payment.Type = ArrangementType.Generate;
                    ArrangementData.Enabled = true;
                }
                else
                {
                    Generate.Checked = false;
                    ArrangementData.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Notifies the user that they are going to setup an arrangement and enables the send type group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setup_CheckedChanged(object sender, EventArgs e)
        {
            if (Setup.Checked)
            {
                DueDate.DataSource = null; //Clear any data in the combobox and reset the datasource
                DueDate.DataSource = GetDueDates();
                ClearForm(); //Clears the fields that are dependant on the Setup
                if (MessageBox.Show("Have you verified that a signed Rehab Agreement has been received?",
                    "Document Received", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    Setup.Checked = false;
                    ArrangementData.Enabled = false;
                }
                else
                {
                    Pay.Payment.Type = ArrangementType.Setup;
                    ArrangementData.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Clears the fields on the form
        /// </summary>
        private void ClearForm()
        {
            ArrangementData.Enabled = false;
            Amount.Text = "";
            DueDate.SelectedIndex = -1;
        }


        /// <summary>
        /// Validates the data input by the user and calles the Process Method in the SetupArrangements class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Process_Click(object sender, EventArgs e)
        {
            string message = "Please provide";
            if (Amount.Text.IsNullOrEmpty())
                message += " Payment Amount";
            if (DueDate.Text.IsNullOrEmpty())
                message += Amount.Text.IsNullOrEmpty() ? ", Due Date" : " Due Date";
            if (!message.EndsWith("Please provide"))
            {
                Dialog.Error.Ok(message, "Missing Fields");
                return;
            }
            if (Generate.Checked)
                Pay.Payment.DueDate = DateTime.Parse(DueDate.Text);
            else if (Setup.Checked)
            {
                Pay.Payment.DueDate = DateTime.Parse(DueDate.Text);
                Pay.Payment.DueDay = Pay.Payment.DueDate.Value.Day;
            }
            Pay.Payment.Amount = double.Parse(Amount.Text);
            Pay.Process();
            this.DialogResult = DialogResult.OK;
        }

        private void SSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 && Verify.Enabled)
                Verify_Click(sender, new EventArgs());
        }

        private void ArrangementForm_Load(object sender, EventArgs e)
        {

        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        private void DueDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
