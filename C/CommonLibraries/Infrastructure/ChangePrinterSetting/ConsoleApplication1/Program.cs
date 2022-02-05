using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using Uheaa.Common.DocumentProcessing;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            PrinterSettings print = new PrinterSettings();


            Printer p = new Printer();
            PrinterData PS = new PrinterData(false);
            bool success = p.ChangePrinterSetting(print.PrinterName, PS);
        }
    }
}
