using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace Monitor
{
    class StandardArgs
    {
        public ReflectionInterface RI { get; set; }
        public DataAccess DA { get; set; }
        public ProcessLogRun PLR { get; set; }
        public MonitorSettings MS { get; set; }
        public List<MonitorReason> ValidReasons { get; set; }
    }
}
