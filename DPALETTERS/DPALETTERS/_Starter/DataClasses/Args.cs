using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace _Starter
{
    class Args : KvpArgs
    {
        public enum ProcessingMode
        {
            Confirmation,
            Cancellation
        }

        [KvpRequiredArg("live", "test", "dev")]
        public DataAccessHelper.Mode DataMode { get; set; }
        [KvpRequiredArg("confirmation","cancellation")]
        public ProcessingMode ProcessMode { get; set; }
        public Args(string[] arguments) : base(arguments) { }
    }
}
