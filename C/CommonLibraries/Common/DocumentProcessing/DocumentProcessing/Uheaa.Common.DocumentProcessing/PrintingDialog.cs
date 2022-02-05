using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uheaa.Common.DocumentProcessing
{
    public partial class PrintingDialog : Form
    {
        public PrintingDialog(string letterId, string step = "Printing")
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            Message.Text = string.Format(Message.Text, letterId, step);
        }

        public void StartDisplay()
        {
            Task.Factory.StartNew(() =>
            {
                ShowDialog();
            });
        }

        public void EndDisplay()
        {
            this.BeginInvoke(new Action(() => this.Close()));
        }
    }
}
