using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagingTransferFileBuilder
{
    public static class Util
    {
        public static string Code()
        {
            return Code("U0000", DateTime.Now);
        }
        public static string Code(DateTime saleDate)
        {
            return Code("U0000", saleDate);
        }
        public static string Code(string dealID, DateTime saleDate)
        {
            return string.Format("COLL_700502_898502_{2}_{0:yyyyMMdd}_{1:yyyyMMddhhmmss}", saleDate, DateTime.Now, dealID);
        }
    }
}
