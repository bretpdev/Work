using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACHSETUPFD
{
	class MissingInformationBranchProcessor : BaseBranchProcessor
	{
        public MissingInformationBranchProcessor(ReflectionInterface ri, SystemBorrowerDemographics brwDemos, string scriptID, RecoveryProcessor recoveryProc, ProcessLogData processLogdata, DataAccess DA, ProcessLogRun logRun)
			: base(ri, brwDemos, scriptID, recoveryProc, processLogdata, DA, logRun)
		{

		}

		public override void Process()
		{
			TD22CommentData td22Data = new TD22CommentData("Autopay request received, but missing information.", false);
			DenialLetter(BrwDemos, td22Data, "Do you want to send a denial letter?");
			MessageBox.Show("Processing Complete");
            return;
		}
	}
}
