using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPINTRTLPD
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        public void AddItem(string item)
        {
            this.BeginInvoke(new Action(() =>
            {
                LogBox.Text = "[" + DateTime.Now.ToString() + "]" + Environment.NewLine + item + Environment.NewLine + Environment.NewLine + LogBox.Text;
            }));
        }
    }
}
