using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common
{
    public class KvpValidArgAttribute : Attribute
    {
        public bool Validate(string value)
        {
            return validate(value);
        }
        private Func<string, bool> validate;
        public KvpValidArgAttribute()
        {
            validate = new Func<string, bool>((s) => true);
        }
        public KvpValidArgAttribute(params string[] allowedValues)
        {
            validate = new Func<string, bool>((s) => allowedValues.Contains(s.ToLower()));
        }
        public KvpValidArgAttribute(Func<string, bool> customValidation)
        {
            validate = customValidation;
        }
    }
}
