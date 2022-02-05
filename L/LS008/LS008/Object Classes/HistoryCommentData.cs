using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace LS008
{
    public class HistoryCommentData
    {
        public string Description { get; set; }
        public DateTime DateRequested { get; set; }
        public string ActivitySeq { get; set; }
        public string ResponseCode { get; set; }
        public string Comment { get; set; }

        public HistoryCommentData(ReflectionInterface ri)
        {
            Description = ri.GetText(13, 2, 28);
            DateRequested = ri.GetText(13, 31, 8).ToDate();
            ActivitySeq = ri.GetText(5, 9, 11);
            ResponseCode = ri.GetTextRemoveUnderscores(15, 2, 5);
            for(int row = 17; (row < 24 && !ri.CheckForText(row, 2, "   ")); row++)
            {
                Comment = Comment + ri.GetText(row, 2, 79);
            }
        }

    }
}
