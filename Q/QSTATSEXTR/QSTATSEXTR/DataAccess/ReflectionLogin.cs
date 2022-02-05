using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace QSTATSEXTR
{
    class ReflectionLogin : IDisposable
    {
        public readonly ReflectionInterface RI;
        public readonly BatchProcessingHelper BPH; 
        public ReflectionLogin(ProcessLogRun plr, string scriptId, string loginType)
        {
            RI = new ReflectionInterface();
            BPH = BatchProcessingLoginHelper.Login(plr, RI, scriptId, loginType);
        }
        public void Dispose()
        {
            RI.CloseSession();
            BatchProcessingHelper.CloseConnection(BPH);
        }
    }
}
