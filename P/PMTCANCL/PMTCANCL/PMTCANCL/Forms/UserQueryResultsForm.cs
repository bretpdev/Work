using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMTCANCL
{
    public partial class UserQueryResultsForm : Form
    {
        private List<PaymentInfo> paymentInfo { get; set; }
        public UserQueryResultsForm(List<PaymentInfo> paymentInfo)
        {
            this.paymentInfo = paymentInfo;
            InitializeComponent();
            PopulateListView();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (ValidateSelection())
            {
                var item = paymentInfo[DisplaySelectListView.CheckedItems[0].Index];
                string message = string.Join("\n", new string[] 
                {
                                "Would you like to cancel the following payment?",
                                 "Confirmation Number: " + item.Conf,
                                 "Ssn: " + item.Ssn,
                                 "Account Number: " + item.AccountNumber,
                                 "Borrower: " + item.Borrower,
                                 "Payment Type: " + item.PayType,
                                 "Payment Amount: " + item.PayAmt,
                                 "Payment Source: " + item.PaySource,
                                 "Payment Created: " + (item.PayCreated.HasValue ? item.PayCreated.Value.ToString() : "Null"),
                                 "Payment Effective: " + (item.PayEffective.HasValue ? item.PayEffective.Value.ToString() : "Null"),
                                 "Processed Date: " + (item.ProcessedDate.HasValue ? item.ProcessedDate.Value.ToString() : "Null"),
                                 "Deleted: " + (item.Deleted ? "Y" : "N")
                });
                if (MessageBox.Show(message, "Confirm Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    paymentInfo.Clear();
                    paymentInfo.Add(item);//new List<PaymentInfo>(new PaymentInfo[] { item });
                    DialogResult = DialogResult.OK;
                }
            }
            else
            {
                MessageBox.Show("You are unable to delete this payment.  It has either already been deleted or it has already been sent offsite for processing.  We can only delete non-processed non-deleted records.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ValidateSelection()
        {
            if (!CheckOneSelected())
            {
                return false;
            }
            if(ProcessedChecked())
            {
                return false;
            }
            if(DeletedChecked())
            {
                return false;
            }

            return true;
        }

        private bool CheckOneSelected()
        {
            if(DisplaySelectListView.CheckedItems.Count == 1)
            {
                return true;
            }
            return false;
        }

        private bool ProcessedChecked()
        {
            var item = paymentInfo[DisplaySelectListView.CheckedItems[0].Index];
            if(item.ProcessedDate.HasValue)
            {
                return true;
            }
            return false;
        }

        private bool DeletedChecked()
        {
            var item = paymentInfo[DisplaySelectListView.CheckedItems[0].Index];
            if (item.Deleted)
            {
                return true;
            }
            return false;
        }

        private void PopulateListView()
        {
            foreach(PaymentInfo pi in paymentInfo)
            {
                if(pi.PaySource == "ACHSETUP")
                {
                    pi.PaySource = "Autopay Setup";
                }
                else if(pi.PaySource == "OPSCHKPHN")
                {
                    pi.PaySource = "CBP Script";
                }
                else if(pi.PaySource == "AgentIVR")
                {
                    pi.PaySource = "IVR-Agent/Mini";
                }
                else if(pi.PaySource == "IVR")
                {
                    pi.PaySource = "IVR-Borr/Main";
                }
                else if(pi.PaySource == "OPSCBPFED")
                {
                    pi.PaySource = "CBP Script";
                }
                else if(pi.PaySource == "ACHSETUPFD")
                {
                    pi.PaySource = "Autopay Setup";
                }
                string[] vals = new string[]
                {
                    pi.Conf,
                    pi.Ssn,
                    pi.AccountNumber,
                    pi.Borrower,
                    pi.PayType,
                    pi.PayAmt,
                    pi.PaySource,
                    pi.PayCreated.HasValue ? pi.PayCreated.Value.ToString() : "Null",
                    pi.PayEffective.HasValue ? pi.PayEffective.Value.ToString() : "Null",
                    pi.ProcessedDate.HasValue ? pi.ProcessedDate.Value.ToString() : "Null",
                    pi.Deleted ? "Y": "N"
                };
                ListViewItem lvi = new ListViewItem(vals);
                DisplaySelectListView.Items.Add(lvi);  
            }
        }

        private void DisplaySelectListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if(DisplaySelectListView.CheckedItems.Count > 1)
            {
                for(int i = 0; i < DisplaySelectListView.CheckedItems.Count; i++)
                {
                    if (DisplaySelectListView.CheckedItems[i].Checked == true && DisplaySelectListView.CheckedItems[i] != e.Item)
                        DisplaySelectListView.CheckedItems[i].Checked = false;
                }
            }
        }
    }
}
