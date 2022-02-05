using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SUBSCRPRT
{
    public partial class PrinterSelection : Form
    {
        public PrinterSelection()
        {
            InitializeComponent();

            LoadPrinters();
        }

        private void LoadPrinters()
        {
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                PrinterLbox.Items.Add(printer.ToString());
            }
        }

        private void PrinterLbox_DoubleClick(object sender, EventArgs e)
        {
            if (PrinterLbox.SelectedItem != null)
            {
                MyPrinters.SetDefaultPrinter(PrinterLbox.SelectedItem.ToString());
                DialogResult = DialogResult.OK;
            }
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            if (PrinterLbox.SelectedItem != null)
            {
                MyPrinters.SetDefaultPrinter(PrinterLbox.SelectedItem.ToString());
                DialogResult = DialogResult.OK;
            }
        }
    }

    public static class MyPrinters
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);
    }
}