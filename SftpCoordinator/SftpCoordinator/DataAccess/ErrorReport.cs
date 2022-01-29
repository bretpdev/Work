using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SftpCoordinator
{
    public class ErrorReport
    {
        public static void GenerateErrorReport(RunHistory rh, ProcessLogRun plr)
        {
            List<ActivityLogDetail> details = ActivityLogDetail.GetActivityLogsByRunHistory(rh.RunHistoryId);
            var errors = details.Where(o => o.Success == "No");
            if (!errors.Any())
                return;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            Type type = typeof(ActivityLogDetail);
            var properties = type.GetProperties();

            List<string> headers = new List<string>();
            foreach (PropertyInfo prop in properties)
                headers.Add(prop.Name);
            string header = CsvHelper.SimpleEncode(headers);
            foreach (ActivityLogDetail detail in errors)
            {
                List<object> values = new List<object>();
                foreach (string name in headers)
                {
                    var prop = type.GetProperties().Where(p => p.Name == name).Single();
                    values.Add(prop.GetValue(detail, null)); 
                }
                string lineData = CsvHelper.SimpleEncode(values);
                plr.AddNotification($"The following file was not handled successfully: \r\n {header} \r\n {lineData} \r\n\r\n Please take necessary steps to ensure that the job details are set up correctly.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }
    }
}
