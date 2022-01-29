using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace SasCoordinator
{
    public class Args : KvpArgs
    {
        #region Arguments
        [KvpRequiredArg]
        public string SasFileLocation { get; set; }
        [KvpRequiredArg("duster")]
        public string Region { get; set; }
        [KvpValidArg]
        public string SysParm { get; set; }
        [KvpValidArg]
        public string ProcessLoggerScriptId { get; set; }
        [KvpValidArg]
        public bool? IsLocalJob { get; set; }
        #endregion
        public SasRegion SasRegion
        {
            get
            {
                    return SasRegion.Duster;
            }
        }
        
        public Args(string[] args) : base(args) { }
    }
}
