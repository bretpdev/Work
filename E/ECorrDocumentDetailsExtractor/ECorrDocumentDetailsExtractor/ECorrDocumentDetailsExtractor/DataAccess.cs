using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ECorrDocumentDetailsExtractor
{
    public class DataAccess
    {
        public DataAccess(DataAccessHelper.Region region)
        {
            DB = region == DataAccessHelper.Region.CornerStone ? DataAccessHelper.Database.ECorrFed : DataAccessHelper.Database.EcorrUheaa;
        }
        DataAccessHelper.Database DB;

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "GetPathsForExtractor")]
        [UsesSproc(DataAccessHelper.Database.EcorrUheaa, "GetPathsForExtractor")]
        public List<DocumentDetails> GetPaths(int skipDays)
        {
            return DataAccessHelper.ExecuteList<DocumentDetails>("GetPathsForExtractor", DB, SqlParams.Single("SkipDays", skipDays));
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "NullPrinted")]
        [UsesSproc(DataAccessHelper.Database.EcorrUheaa, "NullPrinted")]
        public void NullPrinted(int documentDetailsId)
        {
            DataAccessHelper.Execute("NullPrinted", DB, SqlParams.Single("DocumentDetailsId", documentDetailsId));
        }
    }

    public class DocumentDetails
    {
        public int DocumentDetailsId { get; set; }
        public string Path { get; set; }
    }
}
