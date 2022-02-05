using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public abstract class BaseScriptRequestProcessor
    {
        protected ScriptAndServiceMenuItem _scriptOption;

        [DllImport("user32")]
        public static extern long OpenIcon(long hwnd);
        [DllImport("user32")]
        public static extern long SetForegroundWindow(long hwnd);

        public BaseScriptRequestProcessor(ScriptAndServiceMenuItem scriptOption)
        {
            _scriptOption = scriptOption;
        }

        public abstract void RunScript(string argStrAppToFind, int runNumber);

        public static void LActivatePrevInstance(string argStrAppToFind)
        {
            long prevHndl = 0;
            long result = 0;

            //variable to hold individual process.
            //Process objProcess;
            //collection of all the processes running on local machine
            List<Process>  objProcesses = Process.GetProcesses().ToList();

            foreach(var process in objProcesses)
            {
                //Check and exit if we have SMS running already
                if(process.MainWindowTitle.ToUpper() == argStrAppToFind.ToUpper())
                {
                    prevHndl = process.MainWindowHandle.ToInt32();
                    break;
                }
            }
            //if no previous instance found exit the application
            if(prevHndl == 0)
            {
                return;
            }
            //if previous instance found 
            result = OpenIcon(prevHndl); //restore the program;
            result = SetForegroundWindow(prevHndl); //activate the application
        }
    }
}
