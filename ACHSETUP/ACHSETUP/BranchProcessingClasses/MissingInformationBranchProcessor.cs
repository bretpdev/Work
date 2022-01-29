using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace ACHSETUP
{
    class MissingInformationBranchProcessor : BaseBranchProcessor
	{
		private DataAccess DA { get; set; }

		public MissingInformationBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, RecoveryProcessor recoveryProc)
			: base(ri, brwDemos, scriptID, recoveryProc)
		{
			DA = new DataAccess(ri.LogRun);
		}

		public override void Process()
		{
			TD22CommentData td22Data = new TD22CommentData("Autopay request received, but missing information.", false);
			DenialLetter(BrwDemos, td22Data, "Do you want to send a denial letter?", DA);
			Dialog.Info.Ok("Processing Complete");
			return;
		}
	}
}