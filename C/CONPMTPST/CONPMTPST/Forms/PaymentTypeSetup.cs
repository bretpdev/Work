using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace CONPMTPST
{
    public partial class PaymentTypeSetup : Form
    {
        public List<PaymentTypes> Types { get; set; }
        public List<string> Compass { get; set; }
        public List<string> Tiva { get; set; }
        public DataAccess DA { get; set; }

        public PaymentTypeSetup(DataAccess da)
        {
            InitializeComponent();
            DA = da;

            Compass = new List<string>();
            Tiva = new List<string>();

            Types = DA.GetPaymentTypes();
            LoadComboBox();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (!Active.Checked)
            {
                if (!DA.DeletePaymentType(((PaymentTypes)Types.Where(p => p.CompassLoanType == CompassLoanType.Text).Single()).PaymentTypeId))
                    Dialog.Error.Ok("There was an error deleting the payment type", "Error deleting");
                this.DialogResult = DialogResult.OK;
            }
            else if (!Types.Any(p => p.CompassLoanType == CompassLoanType.Text))
            {
                if (AddPaymentType())
                    this.DialogResult = DialogResult.OK;
            }
            else if (CheckForUpdate())
            {
                if (UpdateType())
                    this.DialogResult = DialogResult.OK;
            }
        }

        private void CompassLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CompassLoanType.SelectedIndex > 0)
                TivaLoanType.Text = ((PaymentTypes)Types.Where(p => p.CompassLoanType == CompassLoanType.Text).Single()).TivaFileLoanType;

        }

        private void LoadComboBox()
        {
            //Add an empty string at the top to have a blank space in the combobox
            Compass.Add(string.Empty);
            Tiva.Add(string.Empty);
            foreach (PaymentTypes type in Types)
            {
                Compass.Add(type.CompassLoanType);
                Tiva.Add(type.TivaFileLoanType);
            }

            CompassLoanType.DataSource = Compass;
            TivaLoanType.DataSource = Tiva;
        }

        /// <summary>
        /// Create a new payment type and add it to the database
        /// </summary>
        private bool AddPaymentType()
        {
            if (ValidateData())
            {
                PaymentTypes type = new PaymentTypes();
                type.CompassLoanType = CompassLoanType.Text;
                type.TivaFileLoanType = TivaLoanType.Text;
                if (!DA.InsertPaymentType(type))
                {
                    Dialog.Error.Ok("There was an error adding the payment type");
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check to make sure all the fields are populated to add a payment type
        /// </summary>
        private bool ValidateData()
        {
            string message = "Please provide the following fields to add a new payment type\r\n";
            List<string> errors = new List<string>();
            if (CompassLoanType.Text.IsNullOrEmpty())
                errors.Add("Payment Type");
            if (TivaLoanType.Text.IsNullOrEmpty())
                errors.Add("Tiva Loan File Type");
            if (!Active.Checked)
                errors.Add("Active must be checked");

            if (errors.Count() > 0)
            {
                foreach (string item in errors)
                {
                    message = message + "\r\n" + item;
                }
                Dialog.Error.Ok(message, "Errors found");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the fields to see if anything changed
        /// </summary>
        private bool CheckForUpdate()
        {
            PaymentTypes type = (PaymentTypes)Types.Where(p => p.CompassLoanType == CompassLoanType.Text).Single();
            if (CompassLoanType.Text != type.CompassLoanType || TivaLoanType.Text != type.TivaFileLoanType)
                return true;
            return false;
        }

        /// <summary>
        /// Updates the selected payment type
        /// </summary>
        private bool UpdateType()
        {
            PaymentTypes type = new PaymentTypes();
            type.PaymentTypeId = ((PaymentTypes)Types.Where(p => p.CompassLoanType == CompassLoanType.Text).Single()).PaymentTypeId;
            type.CompassLoanType = CompassLoanType.Text;
            type.TivaFileLoanType = TivaLoanType.Text;
            if (!DA.UpdatePaymentType(type))
            {
                Dialog.Error.Ok("There was an error updating the payment type", "Error Updating");
                return false;
            }
            return true;
        }
    }
}