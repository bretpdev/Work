using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Q;

namespace MauiDUDE
{
    public class DotNetScriptProcessor : BaseScriptRequestProcessor
    {
        private Borrower _borrower;
        private DataAccessHelper.Region networkRegion = DataAccessHelper.Region.Uheaa;

        
        private string PC_DIR 
        {
            get
            {
                return Uheaa.Common.DataAccess.EnterpriseFileSystem.GetPath("PC Dir", DataAccessHelper.Region.Uheaa);
            }
        }
        private string NETWORK_DIR 
        { 
            get 
            {
                return Uheaa.Common.DataAccess.EnterpriseFileSystem.GetPath("CodeBase", networkRegion);
            } 
        }

        public DotNetScriptProcessor(ScriptAndServiceMenuItem scriptOption, Borrower borrower) : base(scriptOption)
        {
            _borrower = borrower;
        }

        public override void RunScript(string argStrAppToFind, int runNumber)
        {
            //adding this so that we can support scripts that still run Q ScriptBase
            bool usesQScriptBase = (bool)_scriptOption.gsData["UsesQScriptBase"];

            string mainDllToLoad = _scriptOption.gsData["DLLToLoad"].ToString();
            List<object> objs = new List<object>();
            ObjectHandle scriptInstance;
            FileUpdater(_scriptOption.gsData["DLLsToCopy"].ToString()); //update files
            //if in test mode then load test dll, leaving the end check so that old things won't accidentally run in the wrong region
            if(DataAccessHelper.TestMode && !PC_DIR.EndsWith("Test\\"))
            {
                mainDllToLoad = $"Test\\{mainDllToLoad}";
            }
            //create arguments for the Q ScriptBase
            if (usesQScriptBase)
            {
                //load parameters for script's constructor
                objs.Add(new Q.ReflectionInterface(SessionInteractionComponents.RI.ReflectionSession,DataAccessHelper.TestMode));
                objs.Add(ScriptProcessingConversionHelper.GetQBorrower(_borrower));
                objs.Add(runNumber);
            }
            //create arguments for the Uheaa.Common ScriptBase
            else
            {
                //load parameters for script's constructor
                objs.Add(SessionInteractionComponents.RI);
                objs.Add(ScriptProcessingConversionHelper.GetUheaaCommonBorrower(_borrower));
                //objs.Add(runNumber);
            }


            //start script
            try
            {
                scriptInstance = System.Activator.CreateInstanceFrom(PC_DIR + mainDllToLoad, _scriptOption.gsData["ObjectToCreate"].ToString(), true, BindingFlags.Default, null, objs.ToArray(), null, null);

                if (usesQScriptBase)
                {
                    ((Q.ScriptBase)scriptInstance.Unwrap()).Main();
                }
                else
                {
                    ((Uheaa.Common.Scripts.ScriptBase)scriptInstance.Unwrap()).Main();
                }
            }
            catch(Uheaa.Common.Scripts.EndDLLException ex) //any time the coder wants the script to end they call a method that throws this exception
            {
                return; //end script
            }
            catch(Q.EndDLLException ex)
            {
                return; //end script
            }
            catch(Exception ex)
            {
                if(ex.GetType().Name.ToLower() == "enddllexception")
                {
                    return;
                }
                throw ex;
            }
            LActivatePrevInstance(argStrAppToFind);
        }

        private void UpdateDlls(string dllFiles)
        {
            networkRegion = DataAccessHelper.Region.Uheaa; //start in uheaa region for the network

            List<string> dlls = dllFiles.Split(',').ToList();

            //this will save the uheaa region network location to a variable
            string workingDir = NETWORK_DIR;

            string pcWorkingDir = PC_DIR;

            if (DataAccessHelper.TestMode)
            {
                //check for existence of test directory and create if needed
                if (!Directory.Exists(pcWorkingDir))
                {
                    Directory.CreateDirectory(pcWorkingDir);
                    //copy over all files from live directory if the test directory was just created
                    List<string> dllsToCopy = Directory.GetFiles(PC_DIR + "*").ToList();
                    foreach (string dll in dllsToCopy)
                    {
                        FS.Copy(PC_DIR + dll, pcWorkingDir + dll);
                    }
                }
            }

            string local = "";
            string network = "";

            try
            {
                foreach(string dll in dlls)
                {
                    local = pcWorkingDir + dll;
                    network = workingDir + dll;
                    if(!Directory.Exists(Path.GetDirectoryName(local)))
                    {
                        FS.CreateDirectory(Path.GetDirectoryName(local));
                    }
                    if(!File.Exists(local))
                    {
                        //if dll doesn't exist locally then pull it down from the network
                        FS.Copy(network, local);
                    }
                    else
                    {
                        //if dll exists then check if it needs to be update
                        if(File.GetLastWriteTime(network) != File.GetLastWriteTime(local))
                        {
                            //if time date stamps don't equal then update the dll
                            FS.Delete(local);
                            FS.Copy(network, local);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while trying to update your script code.  The most likely reason for this is because there is new code to be loaded and the code loaded on your PC is old.  It is suggested that you shutdown your Reflection session and start it back up to refresh your code.  If you feel that you have received this error for other reasons then please contact Systems Support. Local: " + local + ". Remote: " + network, ex);
            }
        }

        //delete old dlls
        private void DeleteOldDlls(string dllName)
        {
            if(File.Exists(PC_DIR +  dllName + ".dll"))
            {
                FS.Delete(PC_DIR + dllName + ".dll");
                FS.Delete(PC_DIR + dllName + ".tlb");
            }
        }

        //updates .net DLLs
        private void FileUpdater(string dllFilesToCopy)
        {
            UpdateDlls("Q.dll"); //We will still need this until all the scripts are conveted to not use q
            UpdateDlls(dllFilesToCopy); //update all other dlls
            //delete old files
            DeleteOldDlls("Alderaan");
            DeleteOldDlls("Coruscant");
            DeleteOldDlls("Dagobah");
            DeleteOldDlls("Endor");
            DeleteOldDlls("Ferengi");
            DeleteOldDlls("Gallifrey");
            DeleteOldDlls("Gondor");
            DeleteOldDlls("Hoth");
            DeleteOldDlls("Klingon");
            DeleteOldDlls("Mordor");
            DeleteOldDlls("Moria");
            DeleteOldDlls("Naboo");
            DeleteOldDlls("Rivendale");
            DeleteOldDlls("Rohan");
            DeleteOldDlls("Romulan");
            DeleteOldDlls("Shire");
            DeleteOldDlls("Tatooine");
            DeleteOldDlls("Vulcan");
            DeleteOldDlls("Yavin");
        }

        
    }
}
