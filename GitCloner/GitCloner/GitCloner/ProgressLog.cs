using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitCloner
{
    class ProgressLog
    {
        public TextBox ProgressBox { get; set; }
        public MainForm Form { get; set; }
        public ProgressLog(TextBox progressBox, MainForm form)
        {
            ProgressBox = progressBox;
            Form = form;
        }

        public void Status(string status)
        {
            Form.BeginInvoke(new Action(() =>
            {
                Form.Text = "Git Cloner - " + status;
            }));
            Log(status);
        }
        public void Log(string message, bool includeTimestamp = true)
        {
            Form.BeginInvoke(new Action(() =>
            {
                string timestamp = DateTime.Now.ToString("hh:MM:ss");
                if (ProgressBox.TextLength > 0)
                    ProgressBox.AppendText(Environment.NewLine);
                if (includeTimestamp)
                    ProgressBox.AppendText(timestamp + " " + message);
                else
                    ProgressBox.AppendText(message);
            }));

        }
    }
}
