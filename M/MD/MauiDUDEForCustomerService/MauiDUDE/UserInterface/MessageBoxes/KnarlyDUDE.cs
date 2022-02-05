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
    public partial class KnarlyDUDE : Form
    {
        public static void ShowKnarlyDude(string message, string title, bool center = false)
        {
            using(KnarlyDUDE kd = new KnarlyDUDE())
            {
                kd.labelMessage.Text = message;
                kd.Text = $"Knarly Dude: {title}";
                if(center)
                {
                    kd.labelMessage.TextAlign = ContentAlignment.MiddleCenter;
                }
                else
                {
                    kd.labelMessage.TextAlign = ContentAlignment.MiddleLeft;
                }
                SystemSounds.Beep.Play();
                kd.ShowDialog();
            }
        }

        public KnarlyDUDE()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
