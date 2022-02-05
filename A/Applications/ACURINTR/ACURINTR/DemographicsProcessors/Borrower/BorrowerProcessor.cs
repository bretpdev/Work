using System;
using ACURINTR.DemographicsParsers;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ACURINTR.DemographicsProcessors
{
	class BorrowerProcessor : ProcessorBase
	{
		public BorrowerProcessor(ReflectionInterface ri, string userId, string scriptId, ProcessLogData logData)
			: base(ri, userId, scriptId, logData)
		{
		}
		/// <summary>
		/// Entry point for BorrowerProcessor Queue processing logic.
		/// </summary>
		/// <param name="queueData"></param>
		/// <param name="assemblyName"></param>
		/// <param name="pauseOnQueueClosingError"></param>
		public override void Process(QueueData queueData, string assemblyName, bool pauseOnQueueClosingError)
		{
			GeneralObj = new General(RI, UserId, ScriptId, LogData);
			if (queueData.System == QueueData.Systems.COMPASS)
				ProcessCompassQueue(queueData, assemblyName, pauseOnQueueClosingError);
			else
				ProcessOneLinkQueue(queueData, assemblyName, pauseOnQueueClosingError);
		}

		protected override void ProcessApplicablePath(QueueData queueData, QueueTask task, bool pauseOnQueueClosingError)
		{
			if (task.HasAddress)
			{
				BorrowerPathAddress addressProcessor = new BorrowerPathAddress(RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				addressProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			//A borrower task may have both home phone and alt phone, but the processor is set up
			//to deal with just one--whichever one is present. So before calling the phone processor,
			//make sure the home phone and alt phone aren't both present.
			string tempPhone = task.Demographics.AlternatePhone;
			task.Demographics.AlternatePhone = "";
			if (task.HasHomePhone)
			{
				BorrowerPathPhone phoneProcessor = new BorrowerPathPhone(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				phoneProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			task.Demographics.AlternatePhone = tempPhone;
			tempPhone = task.Demographics.PrimaryPhone;
			task.Demographics.PrimaryPhone = "";
			if (task.HasAltPhone)
			{
				BorrowerPathPhone phoneProcessor = new BorrowerPathPhone(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				phoneProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			task.Demographics.PrimaryPhone = tempPhone;
			if (task.HasEmail)
			{
				BorrowerPathEmail emailProcessor = new BorrowerPathEmail(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				emailProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
		}
	}
}
