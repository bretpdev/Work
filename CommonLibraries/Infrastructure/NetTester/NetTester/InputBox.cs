using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetTester
{
    public partial class InputBox : Form
    {
        public InputBox(string label, char? passwordChar = null)
        {
            InitializeComponent();
            this.Text = string.Format(this.Text, label);
            this.MainLabel.Text = label;
            if (passwordChar.HasValue)
                InputText.PasswordChar = passwordChar.Value;
        }

        public string Input { get { return InputText.Text; } }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
