using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAMTHCALL
{
    public class ExcelHeader : Attribute
    {
        public string HeaderName { get; set; }

        public ExcelHeader(string headerName)
        {
            HeaderName = headerName;
        }
    }
}
