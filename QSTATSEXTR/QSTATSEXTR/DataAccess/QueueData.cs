using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace QSTATSEXTR
{
    class QueueData
    {
        public DateTime RuntimeDate { get; set; }
        public string Queue { get; set; }
        public long Total { get; set; }
        public long Complete { get; set; }
        public long Critical { get; set; }
        public long Cancelled { get; set; }
        public long Outstanding { get; set; }
        public long Problem { get; set; }
        public long Late { get; set; }
        [DbName("Dept")]
        public string Department { get; set; }

        [DbIgnore]
        public UserDataList ScrapedUserData { get; set; } = new UserDataList();

    }
}
