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
namespace DASFORBFED
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
            else if (region == DataAccessHelper.Region.CornerStone)
                db = DB.Cls;
        }

        [UsesSproc(DB.Cls, "[dasforbfed].[GetPendingWork]")]
        [UsesSproc(DB.Uls, "[dasforbfed].[GetPendingWork]")]
        public List<ProcessQueueData> GetPendingWork()
        {
            return lda.ExecuteList<ProcessQueueData>("[dasforbfed].[GetPendingWork]", db).Result;
        }

        [UsesSproc(DB.Cls, "[dasforbfed].[MarkForbearanceAdded]")]
        [UsesSproc(DB.Uls, "[dasforbfed].[MarkForbearanceAdded]")]
        public void MarkForbearanceAdded(int processQueueId)
        {
            lda.Execute("[dasforbfed].[MarkForbearanceAdded]", db, Sp("ProcessQueueId", processQueueId));
        }

        [UsesSproc(DB.Cls, "[dasforbfed].[MarkArcAdded]")]
        [UsesSproc(DB.Uls, "[dasforbfed].[MarkArcAdded]")]
        public void MarkArcAdded(int processQueueId, int arcAddProcessingId)
        {
            lda.Execute("[dasforbfed].[MarkArcAdded]", db, Sp("ProcessQueueId", processQueueId), Sp("ArcAddProcessingId", arcAddProcessingId));
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}
