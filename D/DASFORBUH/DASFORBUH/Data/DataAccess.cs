using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;
namespace DASFORBUH
{
    public class DataAccess
    {
        private LogDataAccess lda;
        private DB db;
        public DataAccess(LogDataAccess lda, DataAccessHelper.Region region)
        {
            this.lda = lda;
            if (region == DataAccessHelper.Region.Uheaa)
                db = DB.Uls;
        }

        [UsesSproc(DB.Uls, "[dasforbuh].[AddWork]")]
        public bool AddWork()
        {
            return lda.Execute("[dasforbuh].[AddWork]", db);
        }

        [UsesSproc(DB.Uls, "[dasforbuh].[GetPendingWork]")]
        public List<ProcessQueueData> GetPendingWork()
        {
            return lda.ExecuteList<ProcessQueueData>("[dasforbuh].[GetPendingWork]", db).Result;
        }

        [UsesSproc(DB.Uls, "[dasforbuh].[MarkForbearanceAdded]")]
        public void MarkForbearanceAdded(int processQueueId)
        {
            lda.Execute("[dasforbuh].[MarkForbearanceAdded]", db, Sp("ProcessQueueId", processQueueId));
        }

        [UsesSproc(DB.Uls, "[dasforbuh].[MarkArcAdded]")]
        public void MarkArcAdded(int processQueueId, int arcAddProcessingId)
        {
            lda.Execute("[dasforbuh].[MarkArcAdded]", db, Sp("ProcessQueueId", processQueueId), Sp("ArcAddProcessingId", arcAddProcessingId));
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}
