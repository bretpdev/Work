using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace IDRUSERPRO
{
    public class DigitHelper
    {
        public string Value { get; set; }
        public DigitHelper(string value)
        {
            Value = value;
        }
        public bool IsValidDecimal
        {
            get
            {
                return Value.ToDecimalNullable() != null;
            }
        }

        public int DigitsBeforeDecimal
        {
            get
            {
                return Value.Split('.')[0].Length;
            }
        }

        public int DigitsAfterDecimal
        {
            get
            {
                return (Value.Split('.').Skip(1).SingleOrDefault() ?? "").Length;
            }
        }
    }
}
