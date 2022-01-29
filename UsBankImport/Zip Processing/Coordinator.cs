using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace UsBankImport
{
    public class Coordinator
    {
        public List<string> PendingZips { get; private set; }
        public List<string> CompletedZips { get; private set; }
        public List<string> FailedZips { get; private set; }
        private Locations locations;
        private Log log;
        private Action refreshAction;
        public Coordinator(Locations locations, Log log, Action refreshAction = null)
        {
            this.locations = locations;
            this.log = log;
            this.refreshAction = refreshAction;
            PendingZips = new List<string>();
            CompletedZips = new List<string>();
            FailedZips = new List<string>();
        }
        public void LoadPendingZips(string overrideLocation = null)
        {
            PendingZips = new List<string>(Directory.GetFiles(overrideLocation ?? locations.ZipLocation, "*.zip")
                .OrderBy(s => new FileInfo(s).CreationTime).ToList());
            if (refreshAction != null)
                refreshAction();
        }

        public bool IsRunning { get; private set; }
        public void BeginProcessing(bool threaded)
        {
            Action action = () =>
            {
                try
                {
                    IsRunning = true;
                    log.Start();
                    while (PendingZips.Any() && IsRunning)
                    {
                        ProcessNextZip();
                        if (refreshAction != null)
                            refreshAction();
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show(ex.ToString());
#endif
                    log.LogItem("PL#{0} - An error was encountered in the processing thread.  Processing cannot continue.", log.ProcessLogData.ProcessLogId.ToString());
                    ProcessLogger.AddNotification(log.ProcessLogData.ProcessLogId, ex.ToString(), NotificationType.ErrorReport, NotificationSeverityType.Critical, Assembly.GetExecutingAssembly(), ex);
                }
                log.Finished();
            };
            if (threaded)
                new Thread(() => action()).Start();
            else
                action();
        }

        private void ProcessNextZip()
        {
            var zipLocation = PendingZips.Pop();
            var proc = new ZipProcessor(zipLocation, locations, log);
            if (proc.Process())
                CompletedZips.Add(zipLocation);
            else
                FailedZips.Add(zipLocation);
        }
    }
}
