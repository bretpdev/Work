using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa;

namespace ScriptSyncTester
{
    public class ScriptTest
    {
        public string ScriptId { get; set; }
        public TestResult TestResults { get; set; }
        public string ResultDisplay
        {
            get
            {
                return TestResults.ToString();
            }
        }
        public ScriptType Type { get; set; }
        public string TypeDisplay
        {
            get
            {
                return Type.ToString();
            }
        }
        public bool IsFed { get; set; }
        public string RegionDisplay
        {
            get
            {
                return IsFed ? "Fed" : "Uheaa";
            }
        }
        public bool IsLive { get; set; }
        public string ModeDisplay
        {
            get
            {
                return IsLive ? "Live" : "Test";
            }
        }
        public string Location { get { return Script.File.NetworkFullPath; } }
        public string Notes { get; set; }
        public Script Script { get; internal set; }
        public ScriptTest(Script script)
        {
            Script = script;
        }
    }

    public enum ScriptType
    {
        Temp,
        Common,
        Q
    }

    public enum TestResult
    {
        Pending,
        Success,
        BadConstructor,
        BadRegion,
        Failure,
        Skipped
    }
}
