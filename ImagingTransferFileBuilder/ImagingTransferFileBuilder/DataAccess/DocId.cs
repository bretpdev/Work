using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace ImagingTransferFileBuilder
{
    public class DocId
    {
        [PrimaryKey]
        public int DocIdId { get; set; }
        public string DocIdValue { get; set; }
        public int DocTypeId { get; set; }

        [UsesSproc(DataAccessHelper.Database.ImagingTransfers, "DocIdsSelectByDocType")]
        public static List<DocId> GetByDocType(DocType dt)
        {
            return DataAccessHelper.ExecuteList<DocId>("DocIdsSelectByDocType", DataAccessHelper.Database.ImagingTransfers, SqlParams.Generate(new { DocTypeID = dt.DocTypeId }));
        }
        [UsesSproc(DataAccessHelper.Database.ImagingTransfers, "DocIdInsert")]
        public static void Insert(DocId di)
        {
            DataAccessHelper.Execute("DocIdInsert", DataAccessHelper.Database.ImagingTransfers, SqlParams.Insert(di));
        }
        [UsesSproc(DataAccessHelper.Database.ImagingTransfers, "DocIdDelete")]
        public static void Delete(DocId di)
        {
            DataAccessHelper.Execute("DocIdDelete", DataAccessHelper.Database.ImagingTransfers, SqlParams.Delete(di));
        }
    }
}
