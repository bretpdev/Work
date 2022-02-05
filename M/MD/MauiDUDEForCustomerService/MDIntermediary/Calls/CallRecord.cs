using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    public class CallRecord
    {
        public int ReasonId { get; set; }
        public string Comments { get; set; }
        public string LetterId { get; set; }
        public bool IsCornerstone { get; set; }
        public bool IsOutbound { get; set; }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "[calls].RecordCall")]
        public static void Insert(CallRecord record)
        {
            DataAccessHelper.Execute("[calls].RecordCall", DataAccessHelper.Database.MauiDude, SqlParams.Generate(record));
        }
    }
}
