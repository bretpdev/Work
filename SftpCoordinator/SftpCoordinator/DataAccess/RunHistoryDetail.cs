using System;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    public class RunHistoryDetail
    {
        public int RunHistoryId { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? EndedOn { get; set; }
        public int SuccessfulFiles { get; set; }
        public int InvalidFiles { get; set; }
        public string RunBy { get; set; }

        public int TotalFiles { get { return SuccessfulFiles + InvalidFiles; } }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "GetRunHistoryDetailed")]
        public static List<RunHistoryDetail> GetReport(DateTime? start, DateTime? end, bool includeEmpty)
        {
            return Program.PLR.LDA.ExecuteList<RunHistoryDetail>(
                "GetRunHistoryDetailed",
                DataAccessHelper.Database.SftpCoordinator,
                SqlParams.Generate(new { StartDate = start, EndDate = end, IncludeEmpty = includeEmpty })).Result;
        }
    }
}
