using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace DUEDTECNG
{
    public class ThreadedBatchReflectionWorker<T> where T : new()
    {
        public ConcurrentQueue<T> PendingWork { get; private set; }
        public List<Thread> Threads { get; private set; } = new List<Thread>();
        public ProcessLogRun PLR { get; private set; }
        public readonly string ScriptId;
        public readonly string LoginType;
        private Action<T, ReflectionInterface> processWork;
        public bool SuccessfullyLoggedIn { get; private set; }
        public ThreadedBatchReflectionWorker(string scriptId, string loginType, ProcessLogRun plr, List<T> work, int numberOfThreads, Action<T, ReflectionInterface> processWork)
        {
            ScriptId = scriptId;
            LoginType = loginType;
            PLR = plr;
            PendingWork = new ConcurrentQueue<T>(work);
            this.processWork = processWork;
            for (int i = 0; i < numberOfThreads; i++)
            {
                Thread thread = new Thread(ProcessItems);
                Threads.Add(thread);
            }
        }

        public void DoWork()
        {
            foreach (var thread in Threads)
                thread.Start();
            foreach (var thread in Threads)
                thread.Join();
        }

        private void ProcessItems()
        {
            ReflectionInterface ri = null;
            BatchProcessingHelper login = null;
            try
            {
                ri = new ReflectionInterface();
                login = BatchProcessingLoginHelper.Login(PLR, ri, ScriptId, LoginType);
                if (login == null)
                {
                    PLR.AddNotification("Unable to find a valid login for batch login type " + LoginType, NotificationType.EndOfJob, NotificationSeverityType.Warning);
                }
                else
                {
                    SuccessfullyLoggedIn = true;
                    if (PLR.Region == DataAccessHelper.Region.CornerStone && PLR.Mode != DataAccessHelper.Mode.Live)
                    {
                        ri.FastPath("STUPVUK1");
                        ri.Hit(ReflectionInterface.Key.F10);
                    }
                    while (!PendingWork.IsEmpty)
                    {
                        var t = new T();
                        if (PendingWork.TryDequeue(out t))
                            processWork(t, ri);
                    }
                }
            }
            catch (Exception ex)
            {
                PLR.AddNotification("Unexpected error occurred during thread processing.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
            finally
            {
                if (login != null)
                    BatchProcessingHelper.CloseConnection(login);
                if (ri != null)
                    ri.CloseSession();
            }
        }
    }
}
