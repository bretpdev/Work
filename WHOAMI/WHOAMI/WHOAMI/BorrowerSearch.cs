using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace WHOAMI
{
    public class BorrowerSearch : ScriptBase
    {
        public ProcessLogRun LogRun { get; set; }

        public BorrowerSearch(ReflectionInterface ri)
            : base(ri, "WHOAMI")
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.TestMode ? DataAccessHelper.Mode.Dev : DataAccessHelper.Mode.Live;
            LogRun = ri.LogRun ?? new ProcessLogRun("WHOAMI", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
        }

        public override void Main()
        {
            using BorrowerSearchForm form = new BorrowerSearchForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LP22(form.SelectedBorrower);
            LogRun.LogEnd();
        }

        private void LP22(QuickBorrower borrower)
        {
            if (borrower.RegionEnum == RegionSelectionEnum.OneLINK)
                RI.FastPath("LP22I" + borrower.SSN);
            else
                RI.FastPath("TX3Z/ITX1JB" + borrower.SSN);
        }
    }
}