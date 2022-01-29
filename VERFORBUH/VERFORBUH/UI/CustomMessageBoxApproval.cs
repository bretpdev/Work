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
    public partial class CustomMessageBoxApproval : Form
    {
        public CustomMessageBoxApproval(string endDate, string title, string text1, string text2, bool approved)
        {
            InitializeComponent();
            EndDate.Text += endDate;
            if (!approved)
            {
                Icon.Image = global::VERFORBUH.Properties.Resources.Dialog_warning_icon1;
            }
            else
            {
                Icon.Image = global::VERFORBUH.Properties.Resources.webdev_ok_icon___Copy1;
                EndDate.ForeColor = Color.Green;
            }

            this.Text = title;

            Text1.Text = text1;
            Text2.Text = text2;
        }
    }
}
