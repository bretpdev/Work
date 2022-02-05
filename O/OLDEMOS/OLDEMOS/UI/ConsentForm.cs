using System;
using System.Windows.Forms;
using Uheaa.Common.Scripts;

namespace OLDEMOS
{
    public partial class ConsentForm : BaseForm
    {
        ReflectionInterface ses { get; set; } = Helper.RI;

        public ConsentForm()
        {
            InitializeComponent();
            Async(LoadWarningMessage);
        }

        private void PF3()
        {
            ses.Hit(ReflectionInterface.Key.F3);
            this.DialogResult = DialogResult.Cancel;
        }

        private void PF10()
        {
            ses.Hit(ReflectionInterface.Key.F10);
            this.DialogResult = DialogResult.OK;
        }

        private void LoadWarningMessage()
        {
            string warningText = "";
            for (int i = 1; i <= 23; i++)
            {
                string line = ses.GetText(i, 1, 80).Trim();
                if (string.IsNullOrEmpty(line))
                    warningText += Environment.NewLine + Environment.NewLine;
                else
                    warningText += line + " ";
            }
            warningText = warningText.Trim();
            if (this.IsHandleCreated)
                BeginInvoke(() => WarningLabel.Text = warningText);
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F3)
            {
                e.Handled = true;
                PF3();
            }
            if (e.KeyData == Keys.F10)
            {
                e.Handled = true;
                PF10();
            }
        }

        private void PF3Button_Click(object sender, EventArgs e)
        {
            PF3();
        }

        private void PF10Button_Click(object sender, EventArgs e)
        {
            PF10();
        }

        private void GreetingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                PF3();
        }
    }
}