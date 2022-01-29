using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common
{
    public class KvpRequiredArgAttribute : KvpValidArgAttribute
    {
        public KvpRequiredArgAttribute() : base() { }
        public KvpRequiredArgAttribute(params string[] allowedValues) : base(allowedValues) { }
    }
}
