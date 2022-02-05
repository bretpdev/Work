using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCURINT
{
    public class ResponseFile
    {
        public int ResponseFileId { get; set; }
        public int RunId { get; set; }
        public string ResponseFileName { get; set; }
        public string ArchivedFileName { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
