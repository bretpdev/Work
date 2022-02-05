using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;

namespace SatisJudg
{
    public partial class SatJdg : FormBase
    {
        private QueueTaskProcInfo _procInfo;

        public SatJdg(List<string> counties, QueueTaskProcInfo procInfo)
        {
            InitializeComponent();
            cmbCounty.Items.AddRange(counties.ToArray()); //populate counties combo box
            _procInfo = procInfo;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _procInfo.ComplDate = txtComplaintDate.Text;
            _procInfo.ComplAmt = double.Parse(txtCompliantAmt.Text);
            _procInfo.AbstractNo = txtAbstractNum.Text;
            if (cmbCounty.SelectedItem == null)
            {
                _procInfo.County = string.Empty;
            }
            else
            {
                _procInfo.County = cmbCounty.SelectedItem.ToString();
            } 
            this.Hide();
        }
    }
}
