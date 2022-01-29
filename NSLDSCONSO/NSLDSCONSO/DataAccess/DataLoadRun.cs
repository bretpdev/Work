using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSLDSCONSO
{
    public class DataLoadRun
    {
        public int DataLoadRunId { get; set; }
        public DateTime StartedOn { get; set; }
        public string StartedBy { get; set; }
        public DateTime? EndedOn { get; set; }
        public int BorrowerCount { get; set; }
        public string Filename { get; set; }
        public int ActualBorrowerCount { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
