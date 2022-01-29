using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Uheaa.Common;

namespace ActiveDirectoryGroups
{
    public partial class frmAesAccounts : Form
    {
        public DataAccess DA { get; set; }
        public int SqlUserId { get; set; }
        public AesAccountIds Account { get; set; }

        public frmAesAccounts(int sqlUserId, DataAccess da)
        {
            InitializeComponent();
            DA = da;
            SqlUserId = sqlUserId;

            LoadAccounts();
        }

        private void LoadAccounts()
        {
            List<AesAccountIds> accts = DA.GetAesAccounts(SqlUserId);
            CurrentAccounts.DataSource = accts;
            CurrentAccounts.DisplayMember = "AesAccount";
            CurrentAccounts.ValueMember = "AesAccountId";
        }

        private void AddBtn_Click(object sender, System.EventArgs e)
        {
            if (AccountNumberTxt.Text.IsPopulated() && AccountNumberTxt.Text.Length == 10)
                AddAccount(AccountNumberTxt.Text);
            else
                AccountNumberTxt.BackColor = Color.Pink;

            AccountNumberTxt.Focus();
        }

        private void AddAccount(string accountNumber)
        {
            DA.AddAesAccount(SqlUserId, accountNumber);
            AccountNumberTxt.Text = "";
            CurrentAccounts.DataSource = null;
            LoadAccounts();
            AccountNumberTxt.BackColor = SystemColors.Window;
            Account = null;
        }

        private void RemoveBtn_Click(object sender, System.EventArgs e)
        {
            Account = CurrentAccounts.SelectedItem as AesAccountIds;
            if (Account != null)
            {
                DA.DeleteAesAccount(Account.AesAccountId);
                LoadAccounts();
                UndoBtn.Enabled = true;
            }
        }

        private void UndoBtn_Click(object sender, System.EventArgs e)
        {
            if (Account != null)
            {
                AddAccount(Account.AesAccount);
                UndoBtn.Enabled = false;
            }
        }
    }
}