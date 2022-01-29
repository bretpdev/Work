using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INCARBWRS
{
    class Helper
    {
        [Flags]
        public enum LoanStatus
        {
            None = 0,
            Default = 1,
            NonDefault = 2
        }

        [Flags]
        public enum LoanType
        {
            Compass = 1,
            Ffel = 2,
            NonFfel = 4
        }
    }
}
