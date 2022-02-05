using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace NDNUMINVAL
{
    public abstract class _BaseInvalidator
    {
        public ReflectionInterface RI { get; set; }
        public ProcessLogRun PLR { get; set; }
        public string UserName { get; set; }
        public DataAccess DA { get; set; }
        public void Initialize(ProcessLogRun plr, ReflectionInterface ri, string username, DataAccess da)
        {
            PLR = plr;
            RI = ri;
            DA = da;
            UserName = username;
        }
        public abstract bool InvalidatePhoneNumber(NobleData record);
    }
}
