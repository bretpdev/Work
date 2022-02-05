using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Uheaa.Common;

namespace ScriptCoordinator
{
    /// <summary>
    /// Arguments should be given in key-value pairs, separated by colons.
    /// ex: ScriptCoordinator.exe datamode:test scriptid:myscriptid
    /// Supported Arguments:
    /// datamode:dev|test|qa|live
    /// scriptid:[your script id]
    /// reflectionolename: [typically a guid, set by the code that calls this application]
    /// classname: [your full qualified class name]
    /// </summary>
    public class Args : KvpArgs
    {
        #region Arguments and Validation
        [KvpRequiredArg("dev", "test", "qa", "live")]
        public string DataMode { get; set; }
        public bool LiveMode { get { return DataMode == "live"; } }
        [KvpRequiredArg]
        public string ScriptId { get; set; }
        [KvpValidArg]
        public string ClassName { get; set; }
        [KvpValidArg]
        public string ReflectionOleName { get; set; }
        #endregion
        public Args(string[] arguments) : base(arguments){}
    }
}
