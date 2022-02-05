using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public class PrinterData
    {
        public short Duplex { get; set; }
        public short Orientation { get; set; }
        public short Source { get; set; }
        public short Size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDuplex"></param>
        public PrinterData(bool isDuplex)
        {
            if (isDuplex)
                Duplex = 2;
            else
                Duplex = 1;

            Orientation = 15;
            Source = 1;
            Size = 1;
        }
    }
}
