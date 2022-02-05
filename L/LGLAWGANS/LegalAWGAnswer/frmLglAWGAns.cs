using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Q;

using Key = Q.ReflectionInterface.Key;

namespace LegalAWGAnswer
{

    public partial class frmLglAWGAns : FormBase
    {
        public int selection { get; set; }
        public string acctOrSSN { get; set; }

        public frmLglAWGAns(String SSN)
        {
            InitializeComponent();   
            selection = 0;
            txtSSN.Text = SSN;
        }

       

        private void OKbtn_Click(object sender, EventArgs e)
        {
            if (selection == 0)
            {
                MessageBox.Show("Please select an appropriate radio button or Cancel to quit.");
            }
            else if (txtSSN.Text == "") 
            {
                MessageBox.Show("Please enter an SSN or Account Number.");
            }
            else if (txtSSN.Text.Length < 9)
            {
                MessageBox.Show("Please enter a full SSN or ten digit Account Number.");
            }
            else
            {
                acctOrSSN = txtSSN.Text.ToString();
                this.Hide();
            }
        }

        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            selection = -1;
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            selection = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            selection = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            selection = 3;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            selection = 4;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            selection = 5;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            selection = 6;
        }
    }
}
