using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddDateForFSAReports
{
    public partial class DateToRun : Form
    {
        public string SelectedDate { get; set; }
        public DateToRun()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedDate = textBox1.Text;
        }
    }
}
