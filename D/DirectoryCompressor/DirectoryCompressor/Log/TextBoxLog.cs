using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectoryCompressor
{
    class TextBoxLog : Log
    {
        TextBox logBox;
        Label countLabel;
        public TextBoxLog(TextBox logBox, Label countLabel)
        {
            this.logBox = logBox;
            this.countLabel = countLabel;
        }
        protected override void HandleWrite(string formattedText)
        {
            logBox.Invoke(new Action(() => logBox.Text = logBox.Text.Insert(0, formattedText + Environment.NewLine)));
        }
        protected override void HandleFoundFile()
        {
            countLabel.Invoke(new Action(() => countLabel.Text = "Found " + foundFileCount + "+ files."));
        }
        public override void DoneFindingFiles()
        {
            countLabel.Invoke(new Action(() => countLabel.Text = "Found " + foundFileCount + " files."));
        }
    }

}
