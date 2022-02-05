using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ECORRINERR
{
    public class DataAccess
    {
        private ProcessLogRun LogDataUheaa;
        private ProcessLogRun LogDataCornerstone;

        public DataAccess(ProcessLogRun Uheaa, ProcessLogRun Cornerstone)
        {
            LogDataUheaa = Uheaa;
            LogDataCornerstone = Cornerstone;
        }
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "ECORRINERR.InsertErrorRecord")]
        [UsesSproc(DataAccessHelper.Database.EcorrUheaa, "ECORRINERR.InsertErrorRecord")]
        public void InsertErrorInformation(ErrorData record, SqlConnection conn)
        {
            DataAccessHelper.Execute("ECORRINERR.InsertErrorRecord", conn,
                new SqlParameter("path", record.path),
                new SqlParameter("SSN", record.SSN),
                new SqlParameter("DOC_DATE", record.DOC_DATE),
                new SqlParameter("DOC_ID", record.DOC_ID),
                new SqlParameter("ADDR_ACCT_NUM", record.ADDR_ACCT_NUM),
                new SqlParameter("LETTER_ID", record.LETTER_ID),
                new SqlParameter("REQUEST_USER", record.REQUEST_USER),
                new SqlParameter("VIEWABLE", record.VIEWABLE),
                new SqlParameter("CORR_METHOD", record.CORR_METHOD),
                new SqlParameter("REPORT_DESC", record.REPORT_DESC),
                new SqlParameter("LOAD_TIME", record.LOAD_TIME),
                new SqlParameter("REPORT_NAME", record.REPORT_NAME),
                new SqlParameter("ADDRESSEE_EMAIL", record.ADDRESSEE_EMAIL),
                new SqlParameter("VIEWED", record.VIEWED),
                new SqlParameter("CREATE_DATE", record.CREATE_DATE),
                new SqlParameter("MAINFRAME_REGION", record.MAINFRAME_REGION),
                new SqlParameter("DCN", record.DCN),
                new SqlParameter("SUBJECT_LINE", record.SUBJECT_LINE),
                new SqlParameter("DOC_SOURCE", record.DOC_SOURCE),
                new SqlParameter("DOC_COMMENT", record.DOC_COMMENT),
                new SqlParameter("WORKFLOW", record.WORKFLOW),
                new SqlParameter("DOC_DELETE", record.DOC_DELETE),
                new SqlParameter("Region", record.Region),
                new SqlParameter("ErrorText", record.ErrorText),
                new SqlParameter("ErrorFileName", record.ErrorFileName));
        }
    
        private void AddNotification(string message, string region)
        {
            if (region == "CDW")
                LogDataCornerstone.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            else
                LogDataUheaa.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
        }
    }
}