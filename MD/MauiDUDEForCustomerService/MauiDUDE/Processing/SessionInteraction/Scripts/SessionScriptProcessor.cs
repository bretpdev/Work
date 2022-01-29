using Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace MauiDUDE
{
    public class SessionScriptProcessor : BaseScriptRequestProcessor
    {
        private Borrower _borrower;
        
        public SessionScriptProcessor(ScriptAndServiceMenuItem scriptOption, Borrower borrower) : base(scriptOption)
        {
            _borrower = borrower;
        }

        public override void RunScript(string argStrAppToFind, int runNumber)
        {
            List<string> cls = SessionInteractionComponents.RI.ReflectionSession.CommandLineSwitches.Replace("\"", "").Split('/').ToList();
            //bring reflection session to the top of all windows
            LActivatePrevInstance(cls.Last());
            SessionInteractionComponents.RI.ReflectionSession.SwitchToWindow(1);
            //call script
            try
            {
                SessionInteractionComponents.RI.ReflectionSession.RunMacro(_scriptOption.gsData["SubToBeCalled"].ToString(), $"{_borrower.SSN},{runNumber.ToString()},{DataAccessHelper.TestMode.ToString()}");
                //Check for existence of script completion file
                if (_scriptOption.gsData["CompletionFile"] != null && _scriptOption.gsData["CompletionFile"].ToString() != "Nothing")
                {
                    if (!File.Exists(_scriptOption.gsData["CompletionFile"].ToString()))
                    {
                        //if script completion file doesn't exist then
                        WhoaDUDE.ShowWhoaDUDE("DUDE detected that either the script was manually cancelled or the script ended abnormally.  In either case DUDE doesn't register that the script was run.", "Holy Maco");
                    }
                    else
                    {
                        //delete script completion file if it exists
                        FS.Delete(_scriptOption.gsData["CompletionFile"].ToString());
                    }
                } 
            }
            catch(Exception ex)
            {
                ProcessLogRun logRun = SessionInteractionComponents.UheaaLogRun;
                logRun.AddNotification("DUDE was unable to find your script. " + ex, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                WhoaDUDE.ShowWhoaDUDE("DUDE was unable to find your script.", "Holy Maco");
                return;
            }
            LActivatePrevInstance(argStrAppToFind);
        }
    }
}
