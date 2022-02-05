using System;
using System.Reflection;
using System.Security.Policy;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using ACURINTR.DemographicsParsers;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
	class DudeProcessor : ProcessorBase
	{
		public DudeProcessor(ReflectionInterface ri, string userId, string scriptId, ProcessLogData logData)
			: base(ri, userId, scriptId, logData)
		{
		}
		/// <summary>
		/// Entry point for DudeProcessor Queue processing logic.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="assemblyName"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		public override void Process(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
		{
			GeneralObj = new General(RI, UserId, ScriptId, LogData);
			ProcessOneLinkQueue(queueData, assemblyName, pauseOnQueueClosingError);
		}

		protected override void ProcessApplicablePath(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
		{
			//DUDE tasks are set up to only include one demographic type (address, phone, or e-mail),
			//so process the appropriate path based on the task demographics.
			if (task.HasAddress)
			{
                DudePathAddress addressProcessor = new DudePathAddress(RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				addressProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			else if (task.HasHomePhone || task.HasAltPhone)
			{
				DudePathPhone phoneProcessor = new DudePathPhone(RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue,LogData, UserId, ScriptId);
				phoneProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			else if (task.HasEmail)
			{
				DudePathEmail emailProcessor = new DudePathEmail(RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				emailProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
		}
	}
}
