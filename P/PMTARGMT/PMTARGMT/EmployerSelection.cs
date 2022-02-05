using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace Payments
{
    public partial class EmployerSelection : FormBase
    {

        public ExtendedEmployerDemographicsWithListView TheChosenOne { get; set; }

        public EmployerSelection(List<ExtendedEmployerDemographicsWithListView> demoRecs)
        {
            InitializeComponent();
            lvwEmployerSelection.Items.AddRange(demoRecs.ToArray());
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (lvwEmployerSelection.SelectedItems.Count == 0)
            {
                MessageBox.Show("You must select an employer to continue.","No Selection Made",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return; //exit method
            }
            //make note of TheChosenOne
            TheChosenOne = ((ExtendedEmployerDemographicsWithListView) lvwEmployerSelection.SelectedItems[0]);
            this.Close();
        }   

    }
}
