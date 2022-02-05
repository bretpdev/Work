using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace PMTCANCL
{
    public class UserQuery
    {
        public DataAccessHelper.Region region { get; set; }
        public bool? processed { get; set; }
        public DateTime madeAfter { get; set; }
        public string borrower { get; set; }
    }
}
