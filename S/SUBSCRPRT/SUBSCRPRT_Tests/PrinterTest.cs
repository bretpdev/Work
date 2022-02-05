using System.Drawing.Printing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;

namespace SUBSCRPRT_Tests
{
    [TestClass]
    public class PrinterTest
    {
        [TestMethod]
        public void DefaultPrinterChanged()
        {
            //Get the current default printer
            PrinterSettings printer = new PrinterSettings();
            string printerName = printer.PrinterName;

            //Get list of all installed printers
            var printers = PrinterSettings.InstalledPrinters;

            //Set the printer to a new printer. Check to make sure the first printer is not the same as the current default
            if (printers[0].ToString() == printerName)
                MyPrinters.SetDefaultPrinter(printers[1]);
            else
                MyPrinters.SetDefaultPrinter(printers[0]);

            //Get the new printer name
            PrinterSettings newPrinter = new PrinterSettings();
            string newPrinterName = newPrinter.PrinterName;

            Assert.AreNotSame(printerName, newPrinterName);

            //Set default back to where it was
            MyPrinters.SetDefaultPrinter(printerName);
        }
    }

    public static class MyPrinters
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);
    }
}