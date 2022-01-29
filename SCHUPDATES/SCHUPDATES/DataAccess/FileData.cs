using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace SCHUPDATES
{
    public class FileData
    {
        public string SchoolId { get; set; }
        public string MergedSchool { get; set; }
        public DateTime? MergedSchoolDate { get; set; }
        public string LoanProgram { get; set; }
        public string Guarantor { get; set; }
        public string TX10Approval { get; set; }
        public string TX13Approval { get; set; }
        public string TX13Reason { get; set; }
        public DateTime ApprovalDate { get; set; }

        public FileData()
        { }

        public FileData(List<string> data, ProcessLogRun logRun)
        {
            try
            {
                SchoolId = data[0];
                LoanProgram = data[1];
                Guarantor = data[2];
                TX10Approval = data[3];
                TX13Approval = data[4];
                ApprovalDate = data[5].ToDate();
                TX13Reason = data[6];
                MergedSchool = data[7];
                MergedSchoolDate = data[8].ToDateNullable() == null ? (DateTime?)null : data[8].ToDateNullable().Value;
            }
            catch (Exception ex)
            {
                string message = $"There was an error creating a new FileData object with the data passed in: {string.Join(",", data)}";
                logRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                SchoolId = null;
                LoanProgram = null;
                Guarantor = null;
                TX10Approval = null;
                TX13Approval = null;
                TX13Reason = null;
                MergedSchool = null;
            }
        }

        public override string ToString()
        {
            string formattedMergedSchoolDate = MergedSchoolDate.HasValue ? MergedSchoolDate.Value.ToString("MM/dd/yyyy") : "";
            return $"School Id:{SchoolId} Loan Program:{LoanProgram} Guarantor:{Guarantor} TX10 Status Type:{TX10Approval} TX13 Status Type:{TX13Approval} TX13 Status Reason:{TX13Reason??""} Approval Date(Shared by TX10 and TX13):{ApprovalDate:MM/dd/yyyy} TX0Y Merged School: {MergedSchool??""} TX0Y Merged School Date: {formattedMergedSchoolDate}";
        }
    }
}