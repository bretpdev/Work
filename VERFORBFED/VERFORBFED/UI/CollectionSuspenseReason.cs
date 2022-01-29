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

namespace VERFORBFED
{
    public partial class CollectionSuspenseReason : Form
    {
        public enum SelectedReason
        {
            Deferment,
            Forbearance,
            IDR,
            LoanConsolidation
        }

        public SelectedReason Reason { get; set; }

        public CollectionSuspenseReason()
        {
            InitializeComponent();
            Def.Checked = false;
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            if (Def.Checked)
                Reason = SelectedReason.Deferment;
            else if (Forb.Checked)
                Reason = SelectedReason.Forbearance;
            else if (IDR.Checked)
                Reason = SelectedReason.IDR;
            else if (LoanCon.Checked)
                Reason = SelectedReason.LoanConsolidation;
            else
            {
                Dialog.Error.Ok("You must select a reason for the Collection Suspension Forbearance.");
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void CollectionSuspenseReason_Load(object sender, EventArgs e)
        {
            Def.Checked = false;
        }
    }
}
