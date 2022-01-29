using System.Windows.Forms;
using Uheaa.Common.Scripts;

namespace ACHSETUP
{
    class ChangeBranchProcessor : BaseBranchProcessor
	{
		private string FullName { get; set; }

        public ChangeBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, string fullName, RecoveryProcessor recoveryProc)
			: base(ri, brwDemos, scriptID, recoveryProc)
		{
			FullName = fullName;
		}

		public override void Process()
		{
			ChangeMenuOptions options = new ChangeMenuOptions();
			using ChangeMenu menu = new ChangeMenu(options);
			if (menu.ShowDialog() != DialogResult.OK)
				return;
			if (options.Selection == ChangeMenuOptions.ChangeOption.AddRemove)
				new ChangeBranchAddRemoveProcesor(RI, BrwDemos, ScriptId, RecoveryProcessor).Process();
			else
				new ChangeBranchChangeExistingProcessor(RI, BrwDemos, ScriptId, FullName, RecoveryProcessor).Process();
		}
	}
}