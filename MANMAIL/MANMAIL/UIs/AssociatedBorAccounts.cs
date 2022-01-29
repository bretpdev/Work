using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;

namespace MANMAIL
{
    public partial class AssociatedBorAccounts : Form
    {
        public string SelectedAccount { get; set; }

        public AssociatedBorAccounts(List<AssociatedAccounts> accounts)
        {
            InitializeComponent();
            bool isChekced = false;
            foreach (AssociatedAccounts account in accounts)
            {
                AssociatedDemos demo = new AssociatedDemos(account);
                AccountsPanel.Controls.Add(demo);
                if (account.Priority == 0 && !isChekced)
                {
                    demo.SelectCbo.Checked = true;
                    isChekced = true;
                }
                demo.OnUserControllButtonClicked += (s, e) => SelectCbo_Click(s, e);
            }
        }

        private void SelectCbo_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in AccountsPanel.Controls)
            {
                ((AssociatedDemos)ctl).SelectCbo.Checked = false;
                AccountsPanel.Refresh();
            }
            ((AssociatedDemos)sender).SelectCbo.Checked = true;
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in AccountsPanel.Controls)
            {
                if (((AssociatedDemos)ctl).SelectCbo.Checked == true)
                    SelectedAccount = ((AssociatedDemos)ctl).AccountIdentifier;
            }
            if (SelectedAccount.IsPopulated())
                this.DialogResult = DialogResult.OK;
        }
    }
}