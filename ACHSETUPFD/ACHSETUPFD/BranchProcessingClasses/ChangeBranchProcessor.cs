using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
	class ChangeBranchProcessor : BaseBranchProcessor
	{
		private readonly string FullName;

        public ChangeBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, string fullName, RecoveryProcessor recoveryProc, ProcessLogData processLogData, DataAccess DA, ProcessLogRun logRun)
			: base(ri, brwDemos, scriptID, recoveryProc, processLogData, DA, logRun)
		{
		    FullName = fullName;
		}

		public override void Process()
		{
			ChangeMenuOptions options = new ChangeMenuOptions();
			using (ChangeMenu menu = new ChangeMenu(options))
			{
				if (menu.ShowDialog() != DialogResult.OK) { EndDllScript(); }
			}
			if (options.Selection == ChangeMenuOptions.ChangeOption.AddRemove)
			{
                new ChangeBranchAddRemoveProcesor(RI, BrwDemos, ScriptId, RecoveryProcessor, ProcessLogData, DA, LogRun).Process();
			}
			else
			{
                new ChangeBranchChangeExistingProcessor(RI, BrwDemos, ScriptId, FullName, RecoveryProcessor, ProcessLogData, DA, LogRun).Process();
			}
            return;
		}
	}
}
