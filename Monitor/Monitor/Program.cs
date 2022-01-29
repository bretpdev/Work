using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;
using RG = Uheaa.Common.DataAccess.DataAccessHelper.Region;

namespace Monitor
{
    class Program
    {
        public const string ScriptId = "Monitor";
        const string SKIPTASKCLOSE = "skiptaskclose";
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            DataAccessHelper.CurrentRegion = RG.CornerStone;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
            {
                Console.WriteLine("Invalid arguments");
                return 1;
            }
            List<string> arguments = new List<string>(args.Skip(1));
            bool skipTaskClose = false;
            string skipToken = arguments.FirstOrDefault(o => o.ToLower() == SKIPTASKCLOSE);
            if (skipToken != null)
            {
                skipTaskClose = true;
                arguments.Remove(skipToken);
            }
            string identifier = arguments.FirstOrDefault(o => o.ToLower().StartsWith("identifier:"));
            List<string> overrideIdentifiers = new List<string>();
            if (identifier != null)
            {
                arguments.Remove(identifier);
                string listIdentifiers = identifier.Split(':').Last();
                overrideIdentifiers.AddRange(listIdentifiers.Split(','));
            }
            var data = new List<Tuple<DB, DB, RG, string, bool>>();
            using (var launchForm = new LaunchForm())
            {
                launchForm.ShowDialog();
                Process("BatchCornerStone", launchForm.WasAutoLaunched, skipTaskClose, overrideIdentifiers);
            }
            return 0;
        }

        private static StandardArgs GetArgsAndLogin(string username, string password, DataAccess da, ProcessLogRun plr, MonitorSettings monitorSettings = null, List<MonitorReason> validReasons = null)
        {
            ReflectionInterface ri = new ReflectionInterface();
            if (!ri.Login(username, password, RG.CornerStone))
            {
                ri.CloseSession();
                return null;
            }
            if (DataAccessHelper.TestMode)
            {
                ri.FastPath("STUPVUK1");
                ri.Hit(ReflectionInterface.Key.F10);
            }

            StandardArgs sa = new StandardArgs()
            {
                RI = ri,
                DA = da,
                PLR = plr,
                MS = monitorSettings ?? da.GetMonitorSettings(),
                ValidReasons = validReasons ?? da.GetMonitorReasons(),
            };

            return sa;
        }

        private static void Process(string loginType, bool skipSettings, bool skipTaskClose, List<string> overrideIdentifiers)
        {
            var run = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), RG.CornerStone, DataAccessHelper.CurrentMode);
            var lda = new LogDataAccess(DataAccessHelper.CurrentMode, run.ProcessLogId, true, false);
            try
            {
                DataAccess da = new DataAccess(DB.Cdw, DB.Cls, lda);
                List<MonitorReason> validReasons = da.GetMonitorReasons();
                if (!skipSettings)
                {
                    var settings = da.GetMonitorSettings();
                    if (settings.LastRecoveryPage != null)
                    {
                        if (!Dialog.Info.YesNo(string.Format("The last time Monitor was run, it unexpectedly ended on Page {0}.  Would you like to continue processing from Page {0}?", settings.LastRecoveryPage)))
                            settings.LastRecoveryPage = null;
                    }
                    var editor = new ThresholdsEditorForm(settings);
                    if (editor.ShowDialog() == DialogResult.Cancel)
                        return;

                    da.SaveMonitorSettings(settings);

                    var reasonSelection = new ReasonSelectionForm(validReasons);
                    if (reasonSelection.ShowDialog() == DialogResult.Cancel)
                        return;
                    validReasons = reasonSelection.SelectedReasons;
                }
                if (!da.CheckMonitorSettings())
                    run.AddNotification("Unable to find valid monitor settings in CDW", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                else
                {
                    var monitorSettings = da.GetMonitorSettings();
                    List<BatchProcessingHelper> badIds = new List<BatchProcessingHelper>();
                    bool loggedIn = false;
                    while (!loggedIn)
                    {
                        BatchProcessingHelper batchId = BatchProcessingHelper.GetNextAvailableId(null, loginType);
                        var sa = GetArgsAndLogin(batchId.UserName, batchId.Password, da, run, monitorSettings, validReasons);
                        if (sa == null)
                        {
                            run.AddNotification("Could not log in with user " + batchId.UserName, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                            badIds.Add(batchId);
                        }
                        else
                        { 
                            var processor = new R0QueueProcessor(sa, batchId.UserName, skipTaskClose, overrideIdentifiers);
                            bool hasAccess = processor.CheckAccess();
                            if (!hasAccess)
                            {
                                badIds.Add(batchId);
                                run.AddNotification("User has insufficient access: " + batchId.UserName, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                                sa.RI.CloseSession();
                                continue;
                            }
                            foreach (var badId in badIds)
                                BatchProcessingHelper.CloseConnection(badId);
                            bool didAnyWork = processor.Work();

                            BatchProcessingHelper.CloseConnection(batchId);
                            sa.RI.CloseSession();
                            loggedIn = true;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                run.AddNotification("Encountered fatal exception " + ex.ToString(), NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
            finally
            {
                run.LogEnd();
            }
        }
    }
}
