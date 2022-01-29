using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class PreviousBorrower
    {
        public string Name { get; set; }
        public string SSN { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
