using System;
using System.Reflection;

namespace Uheaa.Common.ProcessLoggerRC
{
    public class ProcessLogData
    {
        public int ProcessLogId { get; set; }
        public DateTime StartTime { get; set; }
        public AppDomain Domain { get; set; }
        public Assembly ExecutingAssembly { get; set; }
    }
}
