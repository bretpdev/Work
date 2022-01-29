﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uheaa
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(string scriptId, Exception ex, int scriptLogId)
        {
            InitializeComponent();

            StackTraceBox.Text = ex.ToString();
            TitleBox.Text = scriptId;
            LogIdBox.Text = scriptLogId.ToString();

            this.Text += " - " + DateTime.Now.ToString();
            
            StackTraceBox.Focus();
        }

        private void DebugButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
