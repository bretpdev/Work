using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitor
{
    public partial class ReasonSelectionForm : Form
    {
        List<MonitorReason> source;
        public ReasonSelectionForm(List<MonitorReason> reasons)
        {
            InitializeComponent();

            source = reasons;
            ReasonsBox.Items.Add("Select All", true);
            foreach (var reason in reasons)
                ReasonsBox.Items.Add(string.Format("{0} ({1} borrower{2})", reason.Reason, reason.BorrowerCount, reason.BorrowerCount == 1 ? "" : "s"), true);
            ReasonsBox.ItemCheck += (o, ea) =>
            {
                if (ea.Index == 0)
                    for (int i = 1; i < ReasonsBox.Items.Count; i++)
                        ReasonsBox.SetItemCheckState(i, ea.NewValue);
            };
        }

        public List<MonitorReason> SelectedReasons
        {
            get
            {
                List<MonitorReason> selected = new List<MonitorReason>();
                for (int i = 1; i < ReasonsBox.Items.Count; i++)
                    if (ReasonsBox.GetItemChecked(i))
                        selected.Add(source[i - 1]);
                return selected;
            }
        }
    }
}
