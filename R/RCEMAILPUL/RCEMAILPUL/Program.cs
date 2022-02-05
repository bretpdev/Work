using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLoggerRC;

namespace RCEMAILPUL
{
    class Program
    {
        const int SUCCESS = 0;
        const int FAILURE = 1;
        const string ScriptId = "RCEMAILPUL";
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, ScriptId, false))
                return FAILURE;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return FAILURE;
            
            var plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            try
            {
                var da = new DataAccess(plr.LDA);
                var helper = new SendGridHelper();

                var unsubs = SynchonizeUnsubscribes(helper, da);

                Console.WriteLine("Pulling last 36 hours of SendGrid History...");
                var records = helper.GetSendGridHistory();
                Console.WriteLine($"Found {records.Count} records.");


                int addCount = 0;
                int changeCount = 0;
                foreach (var record in records)
                {
                    if (unsubs.Contains(record.to_email.ToLower()))
                    {
                        string message = $"Skipping record with unsubscribed email address.  Record: ${record.msg_id}.  Email: ${record.to_email}";
                        Console.WriteLine(message);
                        plr.AddNotification(message, NotificationType.EndOfJob, NotificationSeverityType.Informational);
                        continue;
                    }
                    var history = da.AddOrUpdate(record);
                    if (history == null)
                    {
                        Console.WriteLine("New Record: " + record.msg_id);
                        addCount++;
                    }
                    else
                    {
                        List<string> messages = new List<string>();
                        var addMessage = new Action<string,string,string>((name, oldValue, newValue) =>
                        {
                            if (oldValue != newValue)
                                messages.Add($"{name} ({oldValue} -> {newValue})");
                        });
                        addMessage("Status", history.Status, record.status);
                        addMessage("Opens Count", history.OpensCount.ToString(), record.opens_count.ToString());
                        addMessage("Clicks Count", history.ClicksCount.ToString(), record.clicks_count.ToString());
                        addMessage("Last Event Time", history.LastEventTime.ToString(), record.last_event_time.ToString());
                        if (messages.Any())
                        {
                            Console.WriteLine($"Record Changed: MsgId ({record.msg_id}) {string.Join(" ", messages)}");
                            changeCount++;
                        }
                    }

                }
                Console.WriteLine($"Processing Complete: {addCount} new records and {changeCount} changed records.");
#if DEBUG
                Console.ReadKey();
#endif
                return SUCCESS;

            }
            finally
            {
                plr.LogEnd();
                DataAccessHelper.CloseAllManagedConnections();
            }
        }

        private static List<string> SynchonizeUnsubscribes(SendGridHelper helper, DataAccess da)
        {
            Console.WriteLine("Synchronizing Unsubscribes...");
            var globalUnsubs = helper.GetGlobalUnsubscribes();
            var dbUnsubs = da.GetUnsubscribedEmails();
            int addCount = 0;
            int removeCount = 0;
            foreach (var existing in dbUnsubs.Where(o => o.Source == DataAccess.SOURCE))
            {
                if (!globalUnsubs.Any(o => o.ToLower() == existing.EmailAddress.ToLower()))
                {
                    da.RemoveUnsubscribedEmail(existing.EmailAddress);
                    removeCount++;
                }
            }
            foreach (var globalUnsub in globalUnsubs)
            {
                if (!dbUnsubs.Any(o => o.EmailAddress.ToLower() == globalUnsub.ToLower()))
                {
                    da.AddUnsubscribedEmail(globalUnsub);
                    addCount++;
                }
            }

            Console.WriteLine($"{addCount} unsubs added, {removeCount} unsubs removed.");

            return da.GetUnsubscribedEmails().Select(o => o.EmailAddress).ToList();
        }
    }
}
