using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace Uheaa.Common.DocumentProcessing
{
    public class MutliLineMergeField
    {
        public string PdfMergeFieldName { get; set; }
        public List<string> DataFileHeaders { get; set; }
        public List<string> DataFileValues { get; set; }

        public MutliLineMergeField()
        {
            DataFileHeaders = new List<string>();
            DataFileValues = new List<string>();
        }

        public static MutliLineMergeField Populate(string letterId, string field)
        {
            MutliLineMergeField fields = new MutliLineMergeField();
            fields.PdfMergeFieldName = field;
            fields.DataFileHeaders = DataAccessHelper.ExecuteList<string>("LTDB_GetMultiLineDataFileHeaders", DataAccessHelper.Database.Bsys, letterId.ToSqlParameter("LetterId"), field.ToSqlParameter("Field"));
            return fields;
        }
    }
}
