using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class OneLINKDemosAndCallForwarding : Form
    {
        public OneLINKDemosAndCallForwarding()
        {
            InitializeComponent();
        }

        public OneLINKDemosAndCallForwarding(Borrower borrower)
        {
            InitializeComponent();

            borrowerBindingSource.DataSource = borrower; 
            foreach(var result in borrower.CallForwardingUserResultData)
            {
                if(!string.IsNullOrEmpty(result.OverrideMessage))
                {
                    listBoxCallForwardingResults.Items.Add(result.OverrideMessage);
                }
                else
                {
                    listBoxCallForwardingResults.Items.Add($"{result.Location} - {result.Number}");
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
