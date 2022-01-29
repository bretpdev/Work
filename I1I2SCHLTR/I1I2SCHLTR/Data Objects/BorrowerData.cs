using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I1I2SCHLTR
{
    public class BorrowerData
    {
        public int CommentDataId { get; set; }
        public string SSN { get; set; }
        public int RunDateId { get; set; }
        public DateTime? TaskProcessedAt { get; set; }
        public DateTime? CommentProcessedAt { get; set; }
        public List<Schools> Schools { get; set; }
    }
}
