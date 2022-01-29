using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACURINTC
{
    class CompareProcessor : BatchProcessorBase
	{
        public CompareProcessor(General general, bool skipTaskClose) : base(general, skipTaskClose) { }
		protected override void ProcessApplicablePath(QueueInfo data, PendingDemos task)
		{
            //Tasks going through the compare process will only involve either address or phone (never both),
            //so process the appropriate path based on the task demographics.
            if (task.HasAddress)
			{
				ComparePathAddress addressProcessor = new ComparePathAddress(G, data, task, SkipTaskClose);
				addressProcessor.Process();
			}
			if (task.HasHomePhone)
			{
				ComparePathPhone phoneProcessor = new ComparePathPhone(G, data, task, SkipTaskClose);
				phoneProcessor.Process();
			}
		}
    }
}