using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LegalAWGAnswer
{
    public partial class frmGarn : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        } 

        public decimal garnValue { get; set; }
        public int selection { get; set; }

        public frmGarn()
        {

            InitializeComponent();

        }



        private void Garn_Load(object sender, EventArgs e)
        {

        }

        private void OKbtn_Click(object sender, EventArgs e)
        {
            try{
                if (Convert.ToDecimal(textBox1.Text.Replace("$","")) > 0)
                {
                    garnValue = Convert.ToDecimal(textBox1.Text);
                    this.Hide();
                }
            }
            catch{
                MessageBox.Show("Value is not numeric. Please enter dollar value of garnishment.");
            }
  
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You have pressed Cancel, this will end the script.  Are you sure you want to end the script?", "Critical Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                selection = 1;
                this.Hide();
            }

        }

    }
}
