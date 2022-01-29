using System;
using System.Linq;

using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    public class RunHistory
    {
        public int RunHistoryId { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime? EndedOn { get; set; }
        public string RunBy { get; set; }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "InsertRunHistory")]
        public static RunHistory CreateNewRun()
        {
            Console.WriteLine("Creating Run History record...");
            return Program.PLR.LDA.ExecuteSingle<RunHistory>("InsertRunHistory", DataAccessHelper.Database.SftpCoordinator, SqlParams.Insert(new { RunBy = Environment.UserName })).Result;
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "UpdateRunHistoryMarkAsEnded")]
        public static void FinishRun(RunHistory rh)
        {
            Program.PLR.LDA.Execute("UpdateRunHistoryMarkAsEnded", DataAccessHelper.Database.SftpCoordinator, SqlParams.Update(new { RunHistoryID = rh.RunHistoryId }));
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "GetLastRunHistory")]
        public static RunHistory GetLastRunHistory()
        {
            return Program.PLR.LDA.ExecuteList<RunHistory>("GetLastRunHistory", DataAccessHelper.Database.SftpCoordinator).Result.SingleOrDefault();
        }
    }
}
