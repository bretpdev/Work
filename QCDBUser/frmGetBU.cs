using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;
using System.Data.Sql;
using System.Data.SqlClient;


namespace QCDBUser
{
    public partial class frmGetBU : FormBase
    
    {
        private List<string> _dbRecords;
        private string _busUnitCount;
        private TestModeResults _directories;
        public string _busUnit { get; set; }

        public frmGetBU(bool testModeProp)
        {
            InitializeComponent();
            _dbRecords = DataAccess.GetBusinessUnits(testModeProp);
            cmbBU.Items.Clear();
            foreach (string BU in _dbRecords)
            {
                if (BU != "")
                {
                    _busUnitCount = DataAccess.GetQCBUCount(testModeProp, BU);
                    cmbBU.Items.Add(BU + " (" + _busUnitCount + ")");
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cmbBU.SelectedIndex == -1 )
            {
                MessageBox.Show("Please select a business unit from the list.");
            }
            else if (cmbBU.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Please select a business unit from the list.");
            }
            else //field populated and ready to search
            {
                _busUnit = cmbBU.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
            }
        }

        //private void btnCancel_Click(object sender, EventArgs e)
        //{
        //    Common.EndDLLScript();
        //}
    }
}
