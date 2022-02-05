using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DUPLREFS.Forms
{
    public partial class ReferenceControl : UserControl
    {
        DataAccess DA { get; set; }
        bool LeaveActive_IsChecked { get; set; } = false;

        public ReferenceControl()
        {
            InitializeComponent();
        }

        private void PopulateDropdown()
        {
            cbo_RefState.DisplayMember = "State";
            cbo_RefState.ValueMember = "Code";
            List<string> stateCodes = DA.GetStates(); // TODO: Do we need to add "FC" as state when "Country" is populated. Ask BA.
            cbo_RefState.DataSource = stateCodes;
        }

        public void PopulateFields(Reference rfr, DataAccess da)
        {
            DA = da;
            tb_RefID.Text = rfr.RefId;
            tb_RefName.Text = rfr.RefName;
            tb_RefAddress1.Text = rfr.RefAddress1;
            tb_RefAddress2.Text = rfr.RefAddress2 ?? "";
            tb_RefCity.Text = rfr.RefCity;
            PopulateDropdown();
            cbo_RefState.SelectedItem = rfr.RefState;
            tb_RefZip.Text = rfr.RefZip;
            tb_RefPhone.Text = rfr.RefPhone;
            tb_RefCountry.Text = rfr.RefCountry;
        }

        public string GetID()
        {
            return tb_RefID.Text?.Trim();
        }

        public string GetName()
        {
            return tb_RefName.Text?.Trim();
        }

        public string GetAddress1()
        {
            return tb_RefAddress1.Text?.Trim();
        }

        public string GetAddress2()
        {
            return tb_RefAddress2.Text?.Trim();
        }

        public string GetCity()
        {
            return tb_RefCity.Text?.Trim();
        }

        public string GetState()
        {
            return cbo_RefState.Text?.Trim();
        }

        public string GetZip()
        {
            return tb_RefZip.Text?.Trim();
        }

        public string GetPhone()
        {
            return tb_RefPhone.Text?.Trim();
        }

        public string GetStatus()
        {
            return rb_RefLeaveActive.Checked ? "A" : "I";
        }

        public bool GetAddressValid()
        {
            return rb_RefLeaveActive.Checked; // If user did not select to keep ref, script will invalidate address
        }

        public bool GetPhoneValid()
        {
            return rb_RefLeaveActive.Checked; // If user did not select to keep ref, script will invalidate address
        }

        public string GetCountry()
        {
            return tb_RefCountry.Text?.Trim();
        }

        private void rb_RefLeaveActive_CheckedChanged(object sender, EventArgs e)
        {
            LeaveActive_IsChecked = rb_RefLeaveActive.Checked;
        }

        private void rb_RefLeaveActive_OnClick(object sender, EventArgs e)
        {
            if (rb_RefLeaveActive.Checked && !LeaveActive_IsChecked)
                rb_RefLeaveActive.Checked = false;
            else
            {
                rb_RefLeaveActive.Checked = true;
                LeaveActive_IsChecked = false;
            }
        }
    }
}
