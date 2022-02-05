using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomatedImageImporter
{
    public partial class MainForm : Form
    {
        public MainForm(string sourceZip, string monitorDirectory, int indexCount)
        {
            InitializeComponent();
            Thread t = new Thread(() =>
            {
                Processor.Process(sourceZip, monitorDirectory, indexCount, ProgressGrid, ActivityView); 
            });
            t.Start();
        }
    }
}
