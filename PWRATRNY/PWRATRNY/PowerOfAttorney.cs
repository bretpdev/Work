using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Dialog;

namespace PWRATRNY
{
    public class PowerOfAttorney : ScriptBase
    {
        public DataAccess DA { get; set; }

        public PowerOfAttorney(ReflectionInterface ri)
          : base(ri, "PWRATRNY", DataAccessHelper.Region.Uheaa)
        {
            Application.EnableVisualStyles();
            _ = RI.UserId;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            RI.LogRun = RI.LogRun ?? new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            DA = new DataAccess(RI.LogRun.LDA);
        }

        public override void Main()
        {
            while (Question.YesNo("Would you like to process a power of attorney request?", "Process Record?"))
            {
                UserPOAEntry entry = new UserPOAEntry();
                using EntryForm entryFrm = new EntryForm(entry);
                if (entryFrm.ShowDialog() == DialogResult.Cancel) 
                    return; 
                new Processor(RI, ScriptId, DA, RI.LogRun, null).Process(entry);
            }
            RI.LogRun.LogEnd();
        }
    }
}