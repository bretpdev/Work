using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIRPTFED.Views
{
    public partial class RoleAssign : Form
    {
        private string Account { get; set; }
        public RoleAssign(string account)
        {
            Account = account;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                Program.DA.AssignRole(Account, 2, "ROLE - Application Development - Programmer");
            else if (radioButton2.Checked)
                Program.DA.AssignRole(Account, 1, "ROLE - Application Development - Business Systems Analyst");
            else 
                Program.DA.AssignRole(Account, 3, "ROLE - Application Development - Manager");

            DialogResult = DialogResult.OK;
        }
    }
}
