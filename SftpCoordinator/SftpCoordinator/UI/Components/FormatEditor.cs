using System;
using System.Windows.Forms;
using Uheaa.Common;

namespace SftpCoordinator
{
    public partial class FormatEditor : Form
    {
        public FormatEditor(string format)
        {
            InitializeComponent();
            DateTimeBox.Value = DateTime.Now;
            FormatStringBox.Text = format;
        }

        private void Input_TextChanged(object sender, EventArgs e)
        {
            try
            {
                OutputBox.Text = Renamer.Rename(FormatStringBox.Text, FilenameBox.Text, DateTimeBox.Value);
            }
            catch (FormatException)
            {
                OutputBox.Text = "[error]";
            }
        }

        public string NewFormat { get { return FormatStringBox.Text; } }

        private void Insert(string token, string format = null)
        {
            string text = "[[" + token;
            if (!format.IsNullOrEmpty())
                text += ":" + format;t:
            text += "]]";
            FormatStringBox.Paste(text);
        }


        private void DateOnlyButton_Click(object sender, EventArgs e)
        {
            Insert("date", "MM-dd-yyyy");
        }

        private void TimeOnlyButton_Click(object sender, EventArgs e)
        {
            Insert("date", "hh.mm.ss");
        }

        private void DateTimeButton_Click(object sender, EventArgs e)
        {
            Insert("date", "MM-dd-yyyy hh.mm.ss");
        }

        private void FilenameButton_Click(object sender, EventArgs e)
        {
            Insert("file");
        }

        private void FileExtButton_Click(object sender, EventArgs e)
        {
            Insert("ext");
        }
    }
}
