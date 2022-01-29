﻿using System;
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
	class CompareProcessor : ProcessorBase
	{
		public CompareProcessor(ReflectionInterface ri, string userId, string scriptId, ProcessLogData logData)
			: base(ri, userId, scriptId, logData)
		{
		}

		/// <summary>
		/// Overrides ProcessorBase.Process
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
			//Tasks going through the compare process will only involve either address or phone (never both),
			//so process the appropriate path based on the task demographics.
			if (task.HasAddress)
			{

				ComparePathAddress addressProcessor = new ComparePathAddress(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				addressProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
			else if (task.HasHomePhone || task.HasAltPhone)
			{
				ComparePathPhone phoneProcessor = new ComparePathPhone(base.RI, queueData.DemographicsReviewQueue, queueData.ForeignReviewQueue, LogData, UserId, ScriptId);
				phoneProcessor.Process(task, queueData, pauseOnQueueClosingError);
			}
		}
	}
}
