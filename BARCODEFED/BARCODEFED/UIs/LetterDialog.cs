using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace BARCODEFED
{
	partial class LetterDialog : Form
	{
        public DataAccess DA { get; set; }
        public BarcodeInfo barcodeInfo { get; set; }

        public LetterDialog(BarcodeInfo barcodeInfo, DataAccess da)
		{
			InitializeComponent();
            DA = da;
			barcodeInfoBindingSource.DataSource = barcodeInfo;

            List<string> ids = da.GetLetterIds().OrderBy(p => p.ToString()).ToList();
            ids.Insert(0, "");
            LetterIds.DataSource = ids;
		}

        private void OK_Click(object sender, System.EventArgs e)
        {
            barcodeInfo.RecipientId = GetReturnedAccount(barcodeInfo.RecipientId);
            if(barcodeInfo.RecipientId.IsNullOrEmpty())
            {
                MessageBox.Show("The form was cancelled or selected Account Identifier was empty.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private string GetReturnedAccount(string accountIdentifier)
        {
            AssociatedBorAccounts accounts = new AssociatedBorAccounts(DA.GetAssociatedAccounts(accountIdentifier));
            string account = "";
            while(account.IsNullOrEmpty())
            {
                if(accounts.ShowDialog() == DialogResult.Cancel)
                {
                    return "";
                }
                account = accounts.SelectedAccount;
            }
            accounts.Close();
            accounts.Dispose();
            return account;
        }
    }
}