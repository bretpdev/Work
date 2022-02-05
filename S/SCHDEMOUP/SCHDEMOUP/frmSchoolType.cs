using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCHDEMOUP
{
    partial class frmSchoolType : Form
    {
        private DepartmentCodes _codes;

        public frmSchoolType(DepartmentCodes codes)
        {
            InitializeComponent();
            _codes = codes;
        }

        /// <summary>
        /// Sets up the DepartmentCodes object and returns DialogResult.OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rdoGeneral.Checked)
            {
                _codes.Type = DepartmentCodes.UpdateType.General;
            }
            else if (rdoFinancial.Checked)
            {
                _codes.Type = DepartmentCodes.UpdateType.Financial;
            }
            else if (rdoRegistrar.Checked)
            {
                _codes.Type = DepartmentCodes.UpdateType.Registrar;
            }
            else if (rdoBursar.Checked)
            {
                _codes.Type = DepartmentCodes.UpdateType.Bursar;
            }
            else if (rdoAll.Checked)
            {
                _codes.Type = DepartmentCodes.UpdateType.All;
            }
            if (txtSchoolCode.Text == string.Empty || txtSchoolCode.Text.Length < 8)
            {
                MessageBox.Show("You must enter a valid school code to proceed.", "Enter School Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else { _codes.SchoolCode = txtSchoolCode.Text; }
            if (!rdoGeneral.Checked && !rdoFinancial.Checked && !rdoRegistrar.Checked && !rdoBursar.Checked && !rdoAll.Checked)
            {
                MessageBox.Show("You must select at least one update type to proceed", "Select Update Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _codes.SubmitDate = dtpDate.Text;
            rdoAll.Checked = false;
            rdoBursar.Checked = false;
            rdoFinancial.Checked = false;
            rdoGeneral.Checked = false;
            rdoRegistrar.Checked = false;
            txtSchoolCode.Text = string.Empty;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdates_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }
    }
}
