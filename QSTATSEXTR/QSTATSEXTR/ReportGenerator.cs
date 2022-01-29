using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using System.Diagnostics;
using System.IO;
using System.Data;

namespace QSTATSEXTR
{
    class ReportGenerator
    {
        readonly ParallelOptions PO = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
        readonly string ScriptId;
        readonly ProcessLogRun PLR;
        const string LoginType = "BatchUheaa";
        readonly DateTime RuntimeDate;
        readonly DataAccess DA;
        const string fullTimeFormatString = "MM/dd/yyyy hh:mm:ss.fff tt";
        public ReportGenerator(string scriptId, ProcessLogRun plr, DateTime? runTimeDate = null)
        {
            ScriptId = scriptId;
            PLR = plr;
            DA = new DataAccess(plr);
            RuntimeDate = runTimeDate ?? DateTime.Now;
            RuntimeDate = RuntimeDate.ToString(fullTimeFormatString).ToDate(); //fixes an uncommon date rounding error
        }

        public bool Generate(bool skipDataGathering)
        {
            Console.WriteLine("Runtime key: " + RuntimeDate.ToString(fullTimeFormatString));
            var watch = new Stopwatch();
            watch.Start();
            if (!skipDataGathering)
            {
                var lateDays = DA.GetLateQueueDetails();
                var oneLinkData = Task.Run((Func<List<QueueData>>)GatherOneLinkQueueInfo);
                var compassData = Task.Run((Func<List<QueueData>>)GatherCompassQueueInfo);
                using (new ConsoleStep("Scraping Session Data"))
                    Task.WaitAll(oneLinkData, compassData);
                var allData = new QueueDataList();
                allData.AddRange(oneLinkData.Result);
                allData.AddRange(compassData.Result);
                using (new ConsoleStep("Adding Data to Database"))
                    Parallel.ForEach(allData, queue =>
                    {
                        DA.AddQueueData(queue);
                        foreach (var user in queue.ScrapedUserData)
                            DA.AddUserData(user);
                    });
            }
            using (new ConsoleStep("Generating Reports"))
            {
                SendErrorEmail();
                GenerateReports();
            }
            watch.Stop();
            Console.WriteLine("Total elapsed time: " + watch.Elapsed);
            return true;
        }

        private void SendErrorEmail()
        {
            var errors = DA.GetQueueErrors(RuntimeDate);
            string errorMessage = errors.GetErrorMessage();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                string recipients = string.Join(", ", errors.ErrorEmailAddresses.Select(o => o + "@uheaa.org"));
                EmailHelper.SendMail(DataAccessHelper.TestMode, recipients, Environment.UserName + "@uheaa.org", "Queue Stats DB data problems", errorMessage, "", EmailHelper.EmailImportance.Normal, false);
            }
        }

        private void GenerateReports()
        {
            string timeStamp = RuntimeDate.ToString().Replace("", " ", @"\", "/", ":");
            string reportDirectory = EnterpriseFileSystem.GetPath("QSTATSEXTR_Reports");
            var businessUnits = DA.GetBusinessUnitsForReports();
            foreach (var bu in businessUnits)
            {
                string buDir = Path.Combine(reportDirectory, bu);
                var reportData = DA.GetReportData(RuntimeDate, bu);
                if (reportData.Rows.Count == 0)
                    continue;
                if (!Directory.Exists(buDir))
                    Directory.CreateDirectory(buDir);
                List<string> generatedFiles = new List<string>();
                var export = new Func<string, string, string>((reportName, reportUrl) =>
                {
                    string fileName = $"{bu} {reportName}";
                    fileName = Path.Combine(buDir, fileName + " " + timeStamp + ".pdf");
                    var dl = new SsrsDownloader(reportUrl, DataAccessHelper.TestMode);
                    dl.AddParameter("RunTimeDate", RuntimeDate.ToString(fullTimeFormatString));
                    dl.AddParameter("BusinessUnit", bu);
                    dl.AddParameter("UseClosestDate", DataAccessHelper.TestMode ? "false" : "true");
                    var result = dl.Download();
                    if (!result.ReportFound)
                        PLR.AddNotification($"Unable to access report {reportUrl}.  Details: {result.ToString()}", NotificationType.ErrorReport, NotificationSeverityType.Critical, result.CaughtException);
                    else
                    {
                        result.SaveDataAsFile(fileName);
                        generatedFiles.Add(fileName);
                    }
                    return dl.GenerateLink();
                });
                export("Queue Detail Report", "/Operations/Queue Stats Report Detail");
                string summaryLink = export("Queue Summary Report", "/Operations/Queue Stats Report Summary");
                var recipients = DA.GetReportRecipients(bu);
                if (string.IsNullOrWhiteSpace(recipients))
                {
                    PLR.AddNotification($"No email recipients found for BU {bu}.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                }
                else
                {
                    string message = "Please navigate to following link for a summary report.  A copy of the summary report and detail report can be found out on the X drive.";
                    message += $"<br /><br /><a href='{summaryLink}'>{summaryLink}</a>";
                    message += "<br /><br />" + string.Join("<br /><br />", generatedFiles);
                    EmailHelper.SendMail(
                            DataAccessHelper.TestMode,
                            recipients,
                            Environment.UserName + "@uheaa.org",
                            "Queue Stats Report for " + bu,
                            message,
                            "", EmailHelper.EmailImportance.Normal, true
                    );
                }
            }
        }

        private List<QueueData> GatherOneLinkQueueInfo()
        {
            List<QueueData> scrapedQueueData = new List<QueueData>();
            Parallel.ForEach(Queues.OneLinkQueueDepartments, PO, department =>
            {
                using (var login = new ReflectionLogin(PLR, ScriptId, LoginType))
                {
                    var ri = login.RI;
                    ri.FastPath("LP8YI" + department);
                    if (ri.CheckForText(1, 61, "QUEUE TASK SELECTION"))
                    {
                        PageHelper.Iterate(ri, row =>
                        {
                            var selection = ri.GetText(row, 5, 3).ToIntNullable();
                            if (selection.HasValue)
                            {
                                var queueData = new QueueData()
                                {
                                    RuntimeDate = this.RuntimeDate,
                                    Queue = ri.GetText(row, 11, 8),
                                    Total = ri.GetText(row, 20, 10).ToInt(),
                                    Complete = ri.GetText(row, 31, 10).ToInt(),
                                    Critical = ri.GetText(row, 42, 10).ToInt(),
                                    Cancelled = ri.GetText(row, 53, 10).ToInt(),
                                    Outstanding = ri.GetText(row, 64, 10).ToInt(),
                                    Department = department
                                };
                                ri.PutText(21, 13, selection.ToString().PadLeft(2, '0'));
                                ri.Hit(ReflectionInterface.Key.Enter);
                                queueData.ScrapedUserData = ScrapeUserDataOneLink(ri);
                                ri.Hit(ReflectionInterface.Key.F12);//return to master list
                                scrapedQueueData.Add(queueData);
                            }
                        });
                    }
                }
            });
            return scrapedQueueData;
        }

        private List<QueueData> GatherCompassQueueInfo()
        {
            QueueDataList scrapedQueueData = new QueueDataList();
            Parallel.ForEach(Queues.CompassQueueDepartments, PO, department =>
            {
                using (var login = new ReflectionLogin(PLR, ScriptId, LoginType))
                {
                    var ri = login.RI;
                    ri.FastPath("TX3ZITX6D" + department);
                    if (ri.CheckForText(1, 74, "TXX6F"))
                    {
                        PageHelper.Iterate(ri, queueRow =>
                        {
                            var selection = ri.GetText(queueRow, 2, 3).ToIntNullable();
                            if (selection.HasValue)
                            {
                                ri.PutText(22, 18, selection.ToString().PadLeft(2, '0'));
                                ri.Hit(ReflectionInterface.Key.Enter);
                                if (ri.CheckForText(1, 74, "TXX6G"))
                                {
                                    string queue = ri.GetText(4, 8, 2);
                                    PageHelper.Iterate(ri, subqueueRow =>
                                    {
                                        var queueData = ScrapeCompassQueueData(ri, subqueueRow, department);
                                        var userSelection = ri.GetText(subqueueRow, 2, 2).ToIntNullable();
                                        if (userSelection.HasValue)
                                        {
                                            ri.PutText(22, 18, userSelection.Value.ToString().PadLeft(2, '0'));
                                            ri.Hit(ReflectionInterface.Key.Enter);
                                            if (ri.CheckForText(1, 74, "TXX6H"))
                                            {
                                                ScrapeUserDataCompass(ri, queueData);
                                                ri.Hit(ReflectionInterface.Key.F12);
                                            }
                                        }
                                        scrapedQueueData.Add(queueData);
                                    });
                                    ri.Hit(ReflectionInterface.Key.F12);
                                }
                                else
                                {
                                    scrapedQueueData.Add(ScrapeCompassQueueData(ri, queueRow, department));
                                }
                            }
                        });
                    }
                }

            });
            return scrapedQueueData;
        }

        private QueueData ScrapeCompassQueueData(ReflectionInterface ri, int row, string department)
        {
            var queueData = new QueueData()
            {
                RuntimeDate = this.RuntimeDate,
                Queue = ri.GetText(4, 8, 2),
                Total = ri.GetText(row, 17, 6).ToInt(),
                Late = ri.GetText(row, 24, 6).ToInt(),
                Critical = ri.GetText(row, 31, 6).ToInt(),
                Cancelled = ri.GetText(row, 38, 6).ToInt(),
                Complete = ri.GetText(row, 45, 6).ToInt(),
                Outstanding = ri.GetText(row, 59, 6).ToInt(),

                Department = department
            };
            return queueData;
        }

        private UserDataList ScrapeUserDataOneLink(ReflectionInterface ri)
        {
            UserDataList results = new UserDataList();
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 7;
            PageHelper.Iterate(ri, row =>
            {
                string ssn = ri.GetText(7, 2, 9);
                if (ssn.Trim().Length == 9)
                {
                    var userData = new UserData
                    {
                        RuntimeDate = this.RuntimeDate,
                        Queue = ri.GetText(row, 65, 8),
                        UserId = ri.GetText(row, 38, 7),
                        Status = (UserData.TaskStatus)ri.GetText(row, 33, 1)[0],
                        TotalTimeWorked = ri.GetText(row, 54, 6).ToTimeSpanNullable() - ri.GetText(row, 47, 6).ToTimeSpanNullable(), //End Time - Start Time
                        LastWorked = ri.GetText(row, 22, 8).ToDateNullableYearFirst(),
                        CountInStatus = 1
                    };
                    if (!userData.UserId.StartsWith("UT"))
                        return; //only add results for UT IDs
                    results.Add(userData);
                }
            }, settings);
            return results;
        }

        private void ScrapeUserDataCompass(ReflectionInterface ri, QueueData qd)
        {
            qd.Queue = ri.GetText(4, 11, 2) + ri.GetText(5, 11, 2); //queue + subqueue
            UserDataList results = new UserDataList();
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 10;
            settings.RowIncrementValue = 2;
            PageHelper.Iterate(ri, row =>
            {
                var sel = ri.GetText(row, 2, 2).ToIntNullable();
                if (sel.HasValue)
                {
                    int countInStatus;
                    int totalTasks = ri.GetText(row + 1, 17, 5).ToInt();
                    int completedTasks = ri.GetText(row + 1, 45, 5).ToInt();
                    int cancelledTasks = ri.GetText(row + 1, 38, 5).ToInt();
                    UserData.TaskStatus taskStatus = UserData.TaskStatus.Working;
                    if ((countInStatus = totalTasks - completedTasks) != 0)
                        taskStatus = UserData.TaskStatus.Assigned;
                    else if ((countInStatus = completedTasks) != 0)
                        taskStatus = UserData.TaskStatus.Complete;
                    else if ((countInStatus = cancelledTasks) != 0)
                        taskStatus = UserData.TaskStatus.Cancelled;
                    else
                        return; //next row
                    var userData = new UserData
                    {
                        RuntimeDate = this.RuntimeDate,
                        Queue = qd.Queue,
                        UserId = ri.GetText(row, 6, 7),
                        Status = taskStatus,
                        CountInStatus = countInStatus
                    };
                    results.Add(userData);
                }
            }, settings);
            qd.ScrapedUserData = results;
        }
    }
}
