using System;
using Uheaa.Common.ProcessLogger;

namespace DIACTCMTS
{
    class CommentProcessor
    {
        public int Process(ProcessLogRun logRun, bool outbound)
        {
            DataAccess DA = new DataAccess(logRun);
            if (outbound)
                DA.OutboundLoad();
            else
                DA.InboundLoad();
            int uheaaResult = new UheaaProcessing(logRun, DA).Process();

            Console.WriteLine($"UHEAA return value:{uheaaResult}");
            return uheaaResult;
        }
    }
}