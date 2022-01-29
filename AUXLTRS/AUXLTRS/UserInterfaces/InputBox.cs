using System.Linq;
using System.Windows.Forms;
//using Uheaa.Common.WinForms;

namespace AUXLTRS
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        private DialogResult CallHiddenShowDialog()
        {
            return this.ShowDialog();
        }

        public static InputBoxResults ShowDialog(string text, string title)
        {
            InputBox box = new InputBox();
            box.DisplayMessage.Text = text;
            if (box.DisplayMessage.Lines.Count() > 4)
                box.DisplayMessage.ScrollBars = ScrollBars.Vertical;
            box.Text = title;
            InputBoxResults results = new InputBoxResults();
            results.Result = box.CallHiddenShowDialog();
            results.UserProvidedText = box.UserInput.Text;
            return results;
        }

        public static InputBoxResults ShowDialog(string text, string title, string textForTextBox)
        {
            InputBox box = new InputBox();
            box.DisplayMessage.Text = text;
            if (box.DisplayMessage.Lines.Count() > 4)
                box.DisplayMessage.ScrollBars = ScrollBars.Vertical;
            box.Text = title;
            box.UserInput.Text = textForTextBox;
            InputBoxResults results = new InputBoxResults();
            results.Result = box.CallHiddenShowDialog();
            results.UserProvidedText = box.UserInput.Text;
            return results;
        }

        private void InputBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (e.CloseReason.Equals(DialogResult.Cancel) || e.CloseReason.Equals(DialogResult.OK))
            if (this.DialogResult == DialogResult.OK)
                e.Cancel = false;
            else
                UserInput.Text = "";
        }

        private void OK_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
