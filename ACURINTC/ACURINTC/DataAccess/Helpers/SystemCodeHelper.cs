using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    public class SystemCodeHelper
    {
        public List<SystemCode> Codes { get; private set; }
        public SystemCodeHelper(DataAccess da)
        {
            this.Codes = da.SystemCodes();
        }
        public SystemCode GetSystemCode(SystemSource source)
        {
            return this.Codes.SingleOrDefault(o => o.SystemSourceId == source);
        }
    }
}
