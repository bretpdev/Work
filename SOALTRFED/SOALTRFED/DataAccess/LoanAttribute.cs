using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOALTRFED
{
    public class LoanAttribute : Attribute
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public string Format { get; set; }

        public LoanAttribute(string title, int order, string format = "")
        {
            Title = title;
            Order = order;
            Format = format;
        }
    }
}
