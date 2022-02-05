using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace CONPMTPST
{
    public partial class PaymentSourceSetup : Form
    {
        List<PaymentSources> Sources { get; set; }
        List<FileTypes> Types { get; set; }
        public DataAccess DA { get; set; }

        public PaymentSourceSetup(DataAccess da)
        {
            InitializeComponent();

            DA = da;

            Sources = DA.GetPaymentSources();
            Sources.Insert(0, new PaymentSources());
            PaymentSource.DataSource = Sources;
            PaymentSource.DisplayMember = "PaymentSource";
            PaymentSource.ValueMember = "PaymentSourcesId";


            Types = DA.GetFilesTypes();
            Types.Insert(0, new FileTypes());
            FileType.DataSource = Types;
            FileType.DisplayMember = "FileType";
            FileType.ValueMember = "FileTypeId";
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (!Active.Checked)
            {
                if (!DA.DeleteSource(((PaymentSources)PaymentSource.SelectedItem).PaymentSourcesId))
                    Dialog.Error.Ok("There was an error deleting the payment source", "Error in deletion");
                this.DialogResult = DialogResult.OK;
            }
            else if (!Sources.Any(p => p.PaymentSource == PaymentSource.Text))
            {
                if (AddPaymentSource())
                    this.DialogResult = DialogResult.OK;
            }
            else if (CheckForUpdate())
            {
                if (UpdateSource())
                    this.DialogResult = DialogResult.OK;
            }
        }

        private void PaymentSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((PaymentSources)PaymentSource.SelectedItem).FileType != null)
            {
                PaymentSources source = ((PaymentSources)PaymentSource.SelectedItem);
                FileType.SelectedItem = ((FileTypes)Types.Where(p => p.FileTypeId == source.FileType.FileTypeId).Single());
                FileName.Text = source.FileName;
                InstitutionId.Text = source.InstitutionId;
            }
            else if (PaymentSource.SelectedIndex == 0)
            {
                PaymentSource.Text = "";
                InstitutionId.Text = "";
                FileName.Text = "";
                FileType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Checks if the data is valid, adds the new payment source to the database
        /// </summary>
        private bool AddPaymentSource()
        {
            if (ValidateData())
            {
                PaymentSources pSource = new PaymentSources();
                pSource.PaymentSource = PaymentSource.Text;
                pSource.InstitutionId = InstitutionId.Text;
                pSource.FileName = FileName.Text;
                pSource.FileType = new FileTypes();
                pSource.FileType.FileTypeId = ((FileTypes)Types.Where(p => p.FileType == FileType.Text).Single()).FileTypeId;
                if (!DA.InsertPaymentSource(pSource))
                {
                    Dialog.Error.Ok("There was an error adding a new payment source", "Error adding");
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates all the fields before adding a new payment source
        /// </summary>
        private bool ValidateData()
        {
            string message = "Please provide the following fields to add a new payment source\r\n";
            List<string> errors = new List<string>();
            if (PaymentSource.Text.IsNullOrEmpty())
                errors.Add("Payment Source");
            if (InstitutionId.Text.IsNullOrEmpty())
                errors.Add("Institution ID");
            if (FileName.Text.IsNullOrEmpty())
                errors.Add("File Name");
            if (FileType.SelectedIndex == 0)
                errors.Add("File Type");
            if (!Active.Checked)
                errors.Add("Active must be checked");

            if (errors.Count > 0)
            {
                foreach (string item in errors)
                {
                    message = message + "\r\n" + item;
                }
                Dialog.Warning.Ok(message, "Errors found");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks to see if there were any fields changed that need to be updated
        /// </summary>
        private bool CheckForUpdate()
        {
            PaymentSources source = (PaymentSources)PaymentSource.SelectedItem;
            if (InstitutionId.Text != source.InstitutionId || FileName.Text != source.FileName || FileType.Text != source.FileType.FileType)
                return true;
            return false;
        }

        /// <summary>
        /// Updates the changed data
        /// </summary>
        private bool UpdateSource()
        {
            PaymentSources source = new PaymentSources();
            source.PaymentSourcesId = ((PaymentSources)Sources.Where(p => p.PaymentSource == PaymentSource.Text).Single()).PaymentSourcesId;
            source.PaymentSource = PaymentSource.Text;
            source.InstitutionId = InstitutionId.Text;
            source.FileName = FileName.Text;
            source.FileType = new FileTypes();
            source.FileType = (FileTypes)FileType.SelectedItem;
            if (!DA.UpdatePaymentSource(source))
            {
                Dialog.Error.Ok("There was an error updating the payment source", "Error Updating");
                return false;
            }
            return true;
        }
    }
}