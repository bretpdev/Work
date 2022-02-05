using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VERFORBUH
{
    public partial class CustomMessageBoxDenial : Form
    {
        public CustomMessageBoxDenial(string message)
        {
            InitializeComponent();
            Message.Text = message;
        }
    }
}
