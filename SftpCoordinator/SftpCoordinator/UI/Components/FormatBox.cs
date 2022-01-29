using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common.WinForms;

namespace SftpCoordinator
{
    public class FormatBox : TableLayoutPanel
    {
        public new string Text { get { return TextBox.Text; } set { TextBox.Text = value; } }
        public ValidFormatTextBox TextBox { get; set; }
        private Button editor { get; set; }
        public FormatBox()
        {
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            TextBox = new ValidFormatTextBox();
            TextBox.TextChanged += (o, ea) => this.OnTextChanged(ea);
            TextBox.Width = 300;
            this.Controls.Add(TextBox, 0, 0);

            editor = new Button();
            editor.Height = 28;
            editor.Text = "Editor...";
            editor.Click += (o, ea) =>
            {
                FormatEditor fe = new FormatEditor(TextBox.Text);
                FormPopoverManager pop = new FormPopoverManager(fe);
                if (pop.ShowPopoverDialog(this.ParentForm()) == true)
                    TextBox.Text = fe.NewFormat;
            };
            this.Controls.Add(editor, 1, 0);
            this.AutoSize = true;
        }
    }

    public class ValidFormatTextBox : ValidatableTextBox
    {
        public override ValidationResults Validate()
        {
            try
            {
                string rename = Renamer.Rename(Text, "test.txt", DateTime.Now);
                if (Path.GetInvalidFileNameChars().Intersect(rename).Count() > 0)
                    return new ValidationResults(false, "Format string contains invalid characters");
                return base.Validate();
            }
            catch (FormatException)
            {
                return new ValidationResults(false, "Format string contains invalid characters");
            }
        }
    }
}
