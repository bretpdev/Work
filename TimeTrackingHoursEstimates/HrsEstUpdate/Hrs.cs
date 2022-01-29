using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.WinForms;
using Uheaa.Common;
using System.IO;

namespace HrsEstUpdate
{
    public partial class Hrs : Form
    {
        private string emailFile { get; set; }
        public List<Estimates> est { get; set; }
        public Hrs()
        {
            InitializeComponent();

            RequestType.Items.Add("SR");
            RequestType.Items.Add("SASR");
            RefreshGrid();
            Emp.Text = Environment.UserName;
            SetGridColumns();
        }

        private void SetGridColumns()
        {
            if (EstData.ColumnCount > 3)
            {
                EstData.Columns[4].Visible = false;
                EstData.Columns[5].Visible = false;
            }
        }


        private void MakeFieldsVisible(bool visible)
        {
            addhrs.Visible = AdditionalHrs.Visible = file.Visible = rea.Visible = reaText.Visible = attach.Visible = visible;
        }

        private void ClearFields()
        {
            RequestType.Text = "";
            RequestNumber.Text = "";
            HrsEst.Text = "";
            reaText.Text = "";
            TestHrs.Text = "";
            file.Text = "file";
            emailFile = "";
            AdditionalHrs.Text = "";
            MakeFieldsVisible(false);
            EnableFields(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private bool ValidateForm()
        {
            bool valid = true;
            if (RequestType.Text.IsNullOrEmpty())
            {
                RequestType.BackColor = Color.LightPink;
                valid = false;
            }
            else
                RequestType.BackColor = SystemColors.Window;

            if (RequestNumber.Text.IsNullOrEmpty())
            {
                RequestNumber.BackColor = Color.LightPink;
                valid = false;
            }
            else
                RequestNumber.BackColor = SystemColors.Window;

            if (HrsEst.Text.IsNullOrEmpty())
            {
                HrsEst.BackColor = Color.LightPink;
                valid = false;
            }
            else
                HrsEst.BackColor = SystemColors.Window;

            if (reaText.Visible && reaText.Text.IsNullOrEmpty())
            {
                reaText.BackColor = Color.LightPink;
                valid = false;
            }
            else
                reaText.BackColor = SystemColors.Window;

            if (AdditionalHrs.Visible && AdditionalHrs.Text.IsNullOrEmpty())
            {
                AdditionalHrs.BackColor = Color.LightPink;
                valid = false;
            }
            else
                AdditionalHrs.BackColor = SystemColors.Window;



            return valid;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            string emailPath = string.Format(@"Q:\Support Services\Jarom\Time Tracking Emails\{1}_{0}", Path.GetFileName(emailFile), DateTime.Now.ToString("MM_dd_yyyy_hhmmss"));
            var estimate = new Estimates() { RequestType = RequestType.Text, EstimatedHours = HrsEst.Text.ToDecimal(), RequestNumber = RequestNumber.Text, AdditionalHrs = AdditionalHrs.Text.ToDecimalNullable(), ReasonForAdjustment = reaText.Text, AttachmentFileName = file.Text, TestHours = TestHrs.Text.ToDecimalNullable() };



            MakeFieldsVisible(false);
            if (emailFile.IsPopulated())
            {
                File.Copy(emailFile, emailPath, true);
                emailFile = "";
                file.Text = "";
            }

            EnableFields(true);
            ClearFields();
            DataAccess.SaveData(estimate);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            est = DataAccess.GetEst();
            EstData.DataSource = est;
            SetGridColumns();
        }

        private void attach_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileD = new OpenFileDialog();
            fileD.ShowDialog();
            emailFile = fileD.FileName;
            file.Text = fileD.SafeFileName;
        }

        private void EstData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ClearFields();
            var item = est[e.RowIndex];
            RequestType.SelectedText = item.RequestType;
            RequestNumber.Text = item.RequestNumber;
            HrsEst.Text = item.EstimatedHours.ToString();
            AdditionalHrs.Text = item.AdditionalHrs.ToString();
            reaText.Text = item.ReasonForAdjustment;
            TestHrs.Text = item.TestHours.ToString();
            string emailFile = Path.GetFileName(item.AttachmentFileName).SafeSubString(Path.GetFileName(item.AttachmentFileName).LastIndexOf("_") + 1, 100);
            file.Text = emailFile;
            MakeFieldsVisible(true);
            EnableFields(false);
        }

        private void EnableFields(bool value)
        {
            TestHrs.Enabled = RequestType.Enabled = RequestNumber.Enabled = HrsEst.Enabled = value;
        }

        private void Hrs_Load(object sender, EventArgs e)
        {

        }

    }

}
