using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace IDRUSERPRO
{
    public partial class ApplicationInformation : UserControl
    {
        public bool NewApp { get; set; }
        public DataAccess DA { get; set; }
        public ApplicationInformation()
        {
            InitializeComponent();
        }

        public string AccountNumber
        {
            get
            {
                return AccountNumberBox.Text;
            }
            set
            {
                AccountNumberBox.Text = value;
            }
        }

        public DateTime? ApplicationReceivedDate
        {
            get
            {
                return ApplicationDateBox.Text.ToDateNullable();
            }
            set
            {
                if (value == null)
                    ApplicationDateBox.Text = "";
                else
                    ApplicationDateBox.Text = value.Value.ToString("MM/dd/yyyy");
            }
        }

        public int? ApplicationId
        {
            get
            {
                return ApplicationIdTxt.Text.ToIntNullable();
            }
            set
            {
                if (value.HasValue)
                    ApplicationIdTxt.Text = value.ToString();
                else
                    ApplicationIdTxt.Text = "";
            }
        }

        public string AwardId
        {
            get
            {
                return AwardIdTxt.Text;
            }
            set
            {
                AwardIdTxt.Text = value;
            }
        }

        public int CodIdMaxLength
        {
            get
            {
                return CodIdTxt.MaxLength;
            }
            set
            {
                CodIdTxt.MaxLength = value;
            }
        }

        public string CodId
        {
            get
            {
                return CodIdTxt.Text;
            }
            set
            {
                CodIdTxt.Text = value;
            }
        }

        public bool Inactive
        {
            get
            {
                return InactiveChk.Checked;
            }
            set
            {
                InactiveChk.Checked = value;
            }
        }

        public int? ApplicationSourceId { get; set; }

        private bool misroutedApp;
        private string borrowerSsn;
        bool controlLoaded = false;
        public void LoadControl(bool misroutedApp, string borrowerSsn, bool userIsSupervisor, bool newApp, DataAccess da)
        {
            this.misroutedApp = misroutedApp;
            this.borrowerSsn = borrowerSsn;
            this.NewApp = newApp;
            this.DA = da;

            InactiveChk.Visible = userIsSupervisor;
            controlLoaded = true;
            SetVisibility();
        }

        public void SetVisibility()
        {
            if (!controlLoaded)
                return;
            if (ApplicationSourceId == 1 || !NewApp)
                CodIdTxt.Enabled = false;
            if (misroutedApp)
                AwardIdLbl.Visible = AwardIdTxt.Visible = true;
        }

        ///// <summary>
        ///// Runs the validation for all the fields in the Application tab
        ///// </summary>
        public void PerformValidation(Action<string, Control, Label> setError, bool misroutedApp, string borrowerSsn)
        {
            ControlHelper ch = new ControlHelper();
            using (var group = ch.Group(setError, ApplicationDateBox, ApplicationDateLabel))
            {
                group.SetErrorIf("Application Received Date is required", ad => ad.Text.ToDateNullable() == null);
                group.SetErrorIf("Application Received Date cannot be in the future.", ad => ad.Text.ToDateNullable() > DateTime.Now);
                group.SetErrorIf("Application Receieved Date must be within the last 100 years.", ad => ad.Text.ToDateNullable() < DateTime.Now.AddYears(-100));
            }
            using (var group = ch.Group(setError, CodIdTxt, CodLbl))
            {
                group.SetErrorIf("Incomplete COD ID", cd => cd.Text.Length < 8 && cd.Text.Length > 0);
                group.SetErrorIf("COD ID must be different from Borrower SSN", cd => cd.Text == borrowerSsn);
            }

            if (misroutedApp && AwardIdTxt.Text.IsNullOrEmpty())
                setError("Misrouted App must have an Award ID", AwardIdTxt, AwardIdLbl);

            if (NewApp && CodIdTxt.Text.IsPopulated())
            {
                List<DuplicateEappVerification> dupEapps = DA.GetDuplicateEApps(CodIdTxt.Text);

                if (dupEapps.Count != 0)
                {
                    string message = "The entered COD ID already exists for the following borrower(s): \n\n" + string.Join("\r\n", dupEapps);
                    setError(message, CodIdTxt, CodLbl);
                }
            }
        }

        private void Controls_TextChanged(object sender, EventArgs e)
        {
            SetVisibility();
        }
    }
}
