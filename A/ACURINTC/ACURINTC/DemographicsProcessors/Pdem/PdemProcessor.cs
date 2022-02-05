using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACURINTC
{
    class PdemProcessor : BatchProcessorBase
    {
        public PdemProcessor(General general, bool skipTaskClose) : base(general, skipTaskClose) { }
        protected override void ProcessApplicablePath(QueueInfo data, PendingDemos task)
        {
            if (task.HasAddress)
            {
                PdemPathAddress addressProcessor = new PdemPathAddress(G, data, task, SkipTaskClose);
                addressProcessor.Process();
            }
            if (task.HasHomePhone)
            {
                PdemPathPhone phoneProcessor = new PdemPathPhone(G, data, task, SkipTaskClose);
                phoneProcessor.Process();
            }
        }
    }
}