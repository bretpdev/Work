using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class WhoaDUDE : Form
    {
        public static void ShowWhoaDUDE(string message, string title, bool center = false)
        {
            using (WhoaDUDE wd = new WhoaDUDE())
            {
                try
                {
                    wd.labelMessage.Text = message;
                    wd.Text = $"Whoa DUDE: {title}";
                    if (center)
                    {
                        wd.labelMessage.TextAlign = ContentAlignment.MiddleCenter;

                    }
                    else
                    {
                        wd.labelMessage.TextAlign = ContentAlignment.MiddleLeft;
                    }
                    SystemSounds.Beep.Play();
                    wd.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Message", $"Whoa DUDE: {title}", MessageBoxButtons.OK);
                }
            }
        }

        public WhoaDUDE()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
