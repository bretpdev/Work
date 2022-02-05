using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace CPINTRTLPD
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ValidateInput();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                InputFileBox.Text = dialog.FileName;
            }
        }

        private void GuarantorBox_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void OwnerBox_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void BondBox_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        List<string> errors = new List<string>();
        private void ValidateInput()
        {
            errors = new List<string>();

            GuarantorCheck.Checked = GuarantorBox.TextLength == 6;
            if (!GuarantorCheck.Checked)
                errors.Add("Guarantor Code must be 6 characters.");
            OwnerCheck.Checked = OwnerBox.TextLength >= 6;
            if (!OwnerCheck.Checked)
                errors.Add("Owner Code must be at least 6 characters.");
            BondCheck.Checked = BondBox.TextLength >= 6;
            if (!BondCheck.Checked)
                errors.Add("Bond Code must be at least 6 characters.");

            bool goodFile = false;
            if (string.IsNullOrEmpty(InputFileBox.Text))
                errors.Add("No input file selected.");
            else if (!File.Exists(InputFileBox.Text))
                errors.Add("No input file found at " + InputFileBox.Text);
            else
            {
                var results = CsvHelper.ValidateHeader<CsvRow>(File.ReadLines(InputFileBox.Text).First());
                if (results.MissingFields.Any())
                    errors.Add(results.GenerateErrorMessage());
                else
                    goodFile = true;
            }
            InputFileCheck.Checked = goodFile;

            ContinueButton.Enabled = !(ReviewButton.Enabled = errors.Any());
        }

        private void ReviewButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Join(Environment.NewLine, errors));
        }

        private void InputFileBox_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }
        public UserInput Input { get; private set; }
        private void ContinueButton_Click(object sender, EventArgs e)
        {

            Input = new UserInput()
            {
                Guarantor = GuarantorBox.Text,
                Owner = OwnerBox.Text,
                Bond = BondBox.Text,
                InputFileLocation = InputFileBox.Text
            };
            this.Close();
        }
    }
}
