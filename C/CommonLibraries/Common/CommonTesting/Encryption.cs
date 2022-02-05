using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace CommonTesting
{
    public partial class Encryption : Form
    {
        public Encryption()
        {
            InitializeComponent();
            this.AllowTransparency = true;
            this.Opacity = 0.12;
        }

        private void SourceText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string text = SourceText.Text;
                DecryptedText.Text = LegacyCryptography.Decrypt(text, LegacyCryptography.Keys.NoradOPS);
            }
            catch
            {

            }
        }
    }
}
