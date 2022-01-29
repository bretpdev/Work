using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using MDIntermediary;

namespace MauiDUDE
{
    public partial class BankruptcyACPQuestionsControl : UserControl, ILegalOtherControlValidator
    {
        public AcpBankruptcyInfo ResponseData
        {
            get
            {
                if (comboBoxBankruptcyOffiallyFiled.Text.Length == 0)
                {
                    return null;
                }
                else
                {
                    return new AcpBankruptcyInfo()
                    {
                        OfficiallyFiled = comboBoxBankruptcyOffiallyFiled.Text,
                        AttorneyInformation = textBoxAttorneyInformation.Text,
                        CourtInformation = textBoxCourtInformation.Text,
                        Chapter = textBoxChapter.Text,
                        FilingDate = maskedTextBoxFilingDate.Text,
                        DocketNumber = textBoxDocketNumber.Text,
                        EndIndicator = checkBoxEndorser.Checked,
                        EndIdentifier = textBoxEndorser.Text
                    };
                }
            }
        }

        public BankruptcyACPQuestionsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks if data is valid
        /// </summary>
        public bool UserInputIsValid()
        {
            if(comboBoxBankruptcyOffiallyFiled.Text.Length == 0)
            {
                return true; //if first question isn't answered assume that the control is just there and not populated
            }

            int? parsedResult = textBoxChapter.Text.ToIntNullable();
            bool success = parsedResult.HasValue;
            int result = parsedResult.HasValue ? parsedResult.Value : 0;

            if((!success || (result != 7 && result != 11 && result != 12 && result != 13)) && textBoxChapter.Text.Length > 0)
            {
                WhoaDUDE.ShowWhoaDUDE("You must provide a valid bankruptcy chapter(7, 11, 12, 13) or none at all. Do Not Use Leading 0's.", "Needed Bankruptcy Information Missing");
                return false;
            }

            if(textBoxChapter.Text.Trim().Length > 1)
            {
                if(textBoxChapter.Text[0] == '0')
                {
                    WhoaDUDE.ShowWhoaDUDE("You must provide a valid bankruptcy chapter(7, 11, 12, 13) or none at all. Do Not Use Leading 0's.", "Needed Bankruptcy Information Missing");
                    return false;
                }
            }

            DateTime? parsedDate = maskedTextBoxFilingDate.Text.ToDateNullable();
            success = parsedDate.HasValue;
            bool dateInPast = true;

            if(success)
            {
                if (parsedDate.Value > DateTime.Now)
                {
                    dateInPast = false;
                }
            }
            if((!success || !dateInPast) && maskedTextBoxFilingDate.Text.Replace("/","").Replace("_","").Replace(" ", "").Length > 1)
            {
                WhoaDUDE.ShowWhoaDUDE("You must provide a valid bankruptcy filing date or none at all", "Needed Bankruptcy Information Missing");
                return false;
            }

            long? checkIdentifier = textBoxEndorser.Text.ToLongNullable();
            success = checkIdentifier.HasValue;

            if(checkBoxEndorser.Checked && (textBoxEndorser.Text.Length < 9 || !success))
            {
                WhoaDUDE.ShowWhoaDUDE("You must provide a valid endorser ssn", "Needed Bankruptcy Information Missing");
                return false;
            }

            return true;
        }

        private void comboBoxBankruptcyOffiallyFiled_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxBankruptcyOffiallyFiled.Text == "Y")
            {
                groupBoxExtendedInfo.Enabled = true;
            }
            else
            {
                groupBoxExtendedInfo.Enabled = false;
            }
        }

        private void checkBoxEndorser_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxEndorser.Checked)
            {
                textBoxEndorser.Enabled = true;
            }
            if(!checkBoxEndorser.Checked)
            {
                textBoxEndorser.Text = "";
                textBoxEndorser.Enabled = false;
            }
        }
    }
}
