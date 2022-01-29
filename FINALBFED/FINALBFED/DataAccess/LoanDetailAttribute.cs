using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINALBFED
{
    public class LoanDetailAttribute : Attribute
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public string Format { get; set; }

        public LoanDetailAttribute(string title, int order, string format = "")
        {
            Title = title;
            Order = order;
            Format = format;
        }
    }
}
