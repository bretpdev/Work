using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEFMNTPPC
{
    class PPCData
    {
        public string AccountNumber{ get; set; }
        public string SSN { get; set; }
        public string  CLUID{ get; set; }
        public string OldBeginDate { get; set; }
        public string NewBeginDate { get; set; }
        public string OldEndDate { get; set; }
        public string NewEndDate { get; set; }
        public string ChangeDate { get; set; }
        public string DefermentType { get; set; }
        public int FileLineNumber { get; set; }
    }
}
