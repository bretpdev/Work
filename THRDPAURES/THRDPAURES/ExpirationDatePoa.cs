using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace THRDPAURES
{
    public partial class ExpirationDatePoa : Form
    {
        public string ExpirationDate { get; set; }
        public ExpirationDatePoa()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExpirationDate = PoaDate.Text;
            DialogResult = DialogResult.OK;
        }

        private void PoaDate_TextChanged(object sender, EventArgs e)
        {
            if (PoaDate.Text.Length > 7)
            {
                DateTime val = new DateTime();

                button1.Enabled = DateTime.TryParse(PoaDate.Text, out val);
            }
            else
                button1.Enabled = false;

        }
    }
}
