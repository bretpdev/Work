using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace ImagingTransferFileBuilder
{
    public class DocType
    {
        [PrimaryKey]
        public int DocTypeId{get;set;}
        public string DocTypeValue { get; set; }

        [UsesSproc(DataAccessHelper.Database.ImagingTransfers, "DocTypesSelectAll")]
        public static List<DocType> GetAll()
        {
            return DataAccessHelper.ExecuteList<DocType>("DocTypesSelectAll", DataAccessHelper.Database.ImagingTransfers);
        }
        [UsesSproc(DataAccessHelper.Database.ImagingTransfers, "DocTypeInsert")]
        public static void Insert(DocType dt)
        {
            dt.DocTypeId = (int)DataAccessHelper.ExecuteSingle<decimal>("DocTypeInsert", DataAccessHelper.Database.ImagingTransfers, SqlParams.Insert(dt));
        }
        [UsesSproc(DataAccessHelper.Database.ImagingTransfers, "DocTypeDelete")]
        public static void Delete(DocType dt)
        {
            DataAccessHelper.Execute("DocTypeDelete", DataAccessHelper.Database.ImagingTransfers, SqlParams.Delete(dt));
        }

        public override bool Equals(object obj)
        {
            return (obj as DocType).IsNull(o => o.DocTypeId == this.DocTypeId);
        }
        public override int GetHashCode()
        {
            return this.DocTypeId.GetHashCode();
        }
    }
}
