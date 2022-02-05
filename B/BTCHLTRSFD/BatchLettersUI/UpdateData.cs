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

namespace BatchLettersUI
{
    public partial class UpdateData : Form
    {
        public DatabaseData ModifiedDbData { get; set; }
        private DatabaseData OriginalObject { get; set; }
        private bool Insert { get; set; }

        public UpdateData(DatabaseData data, bool insert = false)
        {
            InitializeComponent();
            OriginalObject = new DatabaseData(data);
            ModifiedDbData = data;
            Insert = insert;
            SetValuesForForm();
        }

        /// <summary>
        /// Sets the form values based upon the DatabaseData object that is sent to the constructor
        /// </summary>
        private void SetValuesForForm()
        {
            LetterId.Text = ModifiedDbData.LetterId;
            SasFilePattern.Text = ModifiedDbData.SasFilePattern;
            StateCode.Text = ModifiedDbData.StateFieldCodeName;
            AccountNumberField.Text = ModifiedDbData.AccountNumberFieldName;
            CostCenterField.Text = ModifiedDbData.CostCenterFieldCodeName;
            OkIfMissing.Checked = ModifiedDbData.OkIfMissing;
            ProcessAll.Checked = ModifiedDbData.ProcessAllFiles;
            Arc.Text = ModifiedDbData.Arc;
            Comment.Text = ModifiedDbData.Comment;
            Active.Checked = ModifiedDbData.Active;
            Active.Visible = !Insert;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!CheckForErrors())
                return;

            if (!SetNewData())
                DialogResult = DialogResult.Ignore;
            else
                DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Checks to see if the user populated all required values
        /// </summary>
        /// <returns></returns>
        private bool CheckForErrors()
        {
            List<string> errors = new List<string>();
            if (LetterId.Text.IsNullOrEmpty())
                errors.Add( "Letter Id must be populated");

            if (SasFilePattern.Text.IsNullOrEmpty())
                errors.Add("Sas File Pattern must be populated");

            if (StateCode.Text.IsNullOrEmpty())
                errors.Add( "State Field Name must be populated");

            if (AccountNumberField.Text.IsNullOrEmpty())
                errors.Add("Account Number Field Name must be populated");

            if (errors.Any())
            {
                errors.Insert(0, "The following fields must be corrected:");
                Dialog.Error.Ok(string.Join("\n    -", errors.ToArray()));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the new data from the form
        /// </summary>
        /// <returns></returns>
        private bool SetNewData()
        {
            ModifiedDbData.LetterId = LetterId.Text;
            ModifiedDbData.SasFilePattern = SasFilePattern.Text;
            ModifiedDbData.StateFieldCodeName = StateCode.Text;
            ModifiedDbData.AccountNumberFieldName = AccountNumberField.Text;
            ModifiedDbData.CostCenterFieldCodeName = CostCenterField.Text;
            ModifiedDbData.OkIfMissing = OkIfMissing.Checked;
            ModifiedDbData.ProcessAllFiles = ProcessAll.Checked;
            ModifiedDbData.Arc = Arc.Text;
            ModifiedDbData.Comment = Comment.Text;
            ModifiedDbData.Active = Active.Checked;

            if (Insert)
            {
                ModifiedDbData.CreatedBy = Environment.UserName;
                return true;
            }
            else if (!OriginalObject.IsEqual(ModifiedDbData))
            {
                ModifiedDbData.UpdatedBy = Environment.UserName;
                return true;
            }
            else
                return false;
        }
    }
}
