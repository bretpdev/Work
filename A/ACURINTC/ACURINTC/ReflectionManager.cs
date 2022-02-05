using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace ACURINTC
{

    public class SessionAndLogin
    {
        public ReflectionInterface Session { get; set; }
        public BatchProcessingHelper Login { get; set; }
        public SessionAndLogin(ReflectionInterface session, BatchProcessingHelper login)
        {
            Session = session;
            Login = login;
        }
    }
    public class ReflectionManager : IDisposable
    {
        public static readonly string ScriptId = "ACURINTC"; 
        private string scriptId;
        ProcessLogRun plr;
        public ReflectionManager(string scriptId, ProcessLogRun plr)
        {
            this.plr = plr;
            this.scriptId = scriptId;
        }

        private List<SessionAndLogin> allReflections = new List<SessionAndLogin>();
        public SessionAndLogin GetAvailableReflectionSession()
        {
            lock (allReflections)
            {
                SessionAndLogin available = null;
                while (available == null)
                {
                    var ri = new ReflectionInterface();
                    BatchProcessingHelper login = null;
                    try
                    {
                        login = BatchProcessingLoginHelper.Login(plr, ri, ScriptId, "BatchUheaa");
                        plr.AddNotification("Successfully started session with user id " + login.UserName, NotificationType.EndOfJob, NotificationSeverityType.Informational);
                        if (!TaskHelper.UserHasOneLinkAccess(ri))
                        {
                            plr.AddNotification(string.Format("User {0} does not have OneLink access, please grant access to LP22.", login.UserName), NotificationType.ErrorReport, NotificationSeverityType.Warning);
                            ri.CloseSession();
                            continue;
                        }
                        available = new SessionAndLogin(ri, login);
                        allReflections.Add(available);
                    }
                    catch(Exception ex)
                    {
                        plr.AddNotification($"Unable to start BatchUheaa session, received Excpetion: {ex.Message}", NotificationType.EndOfJob, NotificationSeverityType.Informational);
                        ri.CloseSession();
                    }
                }
                return available;
            }
        }

        public void ReleaseReflectionSession(SessionAndLogin sal)
        {
            allReflections.Remove(sal);
            sal.Session.CloseSession();
        }

        public void Dispose()
        {
            foreach (var session in allReflections)
            {
                session.Session.CloseSession();
                BatchProcessingHelper.CloseConnection(session.Login);
            }

            allReflections.Clear();
        }
    }
}
