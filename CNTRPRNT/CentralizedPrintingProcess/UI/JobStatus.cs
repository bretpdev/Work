using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CentralizedPrintingProcess
{
    public partial class JobStatus : UserControl, IJobStatus
    {
        public JobStatus()
        {
            InitializeComponent();
        }

        public string TitleText
        {
            get
            {
                return TitleLabel.Text;
            }
            set
            {
                SafeInvoke(() => TitleLabel.Text = value);
            }
        }

        public Color TitleColor
        {
            get
            {
                return TitleLabel.BackColor;
            }
            set
            {
                SafeInvoke(() => TitleLabel.BackColor = value);
            }
        }

        public void LogItem(string text, params object[] values)
        {
            SafeInvoke(() =>
            {
                text = DateTime.Now.ToString("hh:mm ") + text;
                LogText.AppendText(string.Format(text, values) + Environment.NewLine);
            });
        }

        private void SafeInvoke(Action a)
        {
            if (IsHandleCreated)
                base.Invoke(a);
            else
                a();
        }
    }
}
