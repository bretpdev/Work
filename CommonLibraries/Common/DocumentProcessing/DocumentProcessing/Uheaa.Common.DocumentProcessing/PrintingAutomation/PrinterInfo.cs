using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.DocumentProcessing
{
    public class PrinterInfo
    {
        public short Duplex { get; set; }
        public short Orientation { get; set; }
        public short Source { get; set; }
        public short Size { get; set; }
        public string DefaultPrinter { get; set; }

        private bool UsedReadonlyCtor { get; set; }

        public PrinterSettings Settings { get; set; }

        /// <summary>
        /// Set the printer to Duplex or Simplex from given value.
        /// </summary>
        /// <param name="isDuplex">True to change the settings to Duplex, False for simplex</param>

        [Obsolete("Do not use this Ctor", false)]
        public PrinterInfo(bool isDuplex) : this()
        {
            if (isDuplex)
                Duplex = 2;
            else
                Duplex = 1;
        }

        /// <summary>
        /// Only use the Ctor if you want readonly access to the printer settings
        /// </summary>
        public PrinterInfo()
        {
            Settings = new PrinterSettings();
            Orientation = 15;
            Source = 1;
            Size = 1;

            DefaultPrinter = Settings.PrinterName;
        }

        /// <summary>
        /// Changes the default printer settings to Simplex or Duplex based upon the Duplex Property.
        /// </summary>
        /// <returns>True if the settings were changed, False if not</returns>
        [Obsolete("Do not use this Method", false)]
        public bool ChangePrinterSettings()
        {
            return new Printer().ChangePrinterSetting(this);
        }


        public bool ChangePrinterSettings(bool duplex)
        {

            if (duplex)
                Duplex = 2;
            else
                Duplex = 1;

            return new Printer().ChangePrinterSetting(this);
        }
    }
}