using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace SIRPTFED.Views
{
    public partial class UserSelect : Form
    {
        public string User { get; set; }
        private List<string> data { get; set; }

        public UserSelect(List<string> Data )
        {
            InitializeComponent();
            Users.DataSource = Data;
            data = Data ;
        }

        private void Users_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Users_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Program.Suser = Users.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }
    }
}
