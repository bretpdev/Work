using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace CFSARPTFED
{
    public partial class Mode : Form
    {
        private ReportData Report;
        public Mode(List<ReportData> reports, ReportData report)
        {
            InitializeComponent();
            cboReport.DataSource = reports.Select(p => p.ReportName).ToList();
            cboReport.SelectedIndex = -1;
            Report = report;
        }

        private void rdoSpecificRpt_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                cboReport.Enabled = rdoSpecificRpt.Checked;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rdoSpecificRpt.Checked)
            {
                if (cboReport.SelectedIndex < 0)
                {
                    MessageBox.Show("You must select a report.","Select A Report.");
                    return;
                }

                Report.ReportName = cboReport.SelectedItem.ToString();
            }

            DialogResult = DialogResult.OK;

        }

        private void rdoAllReports_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                if (rdoAllReports.Checked)
                {
                    cboReport.SelectedIndex = -1;
                }
            }
        }
    }
}
