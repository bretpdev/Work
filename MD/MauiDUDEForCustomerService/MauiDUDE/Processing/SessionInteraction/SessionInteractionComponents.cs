using Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace MauiDUDE
{
    //Can't add a reference to the MD project this creates a static helper containing the reflection session that is active
    public class SessionInteractionComponents
    {
        public static readonly string MAUI_DUDE_EMAIL_ADDRESS = "mauidude@utahsbr.edu";
        public static readonly string MAUI_DUDE_SCRIPT_ID = "MAUIDUDE";
        public static readonly string ScriptId = "DUDE";

        public static ProcessLogRun UheaaLogRun { get; set; }
        public static ReflectionInterface RI { get; set; }

        public static Reflection.Session ReflectionSession { get; private set; }
        public static int ProcessLogId { get; set; }

        public static void InstantiateVariables(ReflectionInterface ri, ProcessLogRun uheaaLogRun)
        {
            ReflectionSession = ri.ReflectionSession;
            RI = ri;
            UheaaLogRun = uheaaLogRun;
        }

        public static Action KillReflection { get; set; }


    }
}
