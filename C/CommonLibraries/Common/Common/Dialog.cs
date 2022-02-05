using System.Windows.Forms;

namespace Uheaa.Common
{
    public class Dialog
    {
        private MessageBoxIcon Icon { get; set; }
        private Dialog(MessageBoxIcon icon)
        {
            this.Icon = icon;
        }
        static Dialog()
        {
            Warning = new Dialog(MessageBoxIcon.Warning);
            Info = new Dialog(MessageBoxIcon.Information);
            Error = new Dialog(MessageBoxIcon.Error);
            Def = new Dialog(MessageBoxIcon.None);
            Question = new Dialog(MessageBoxIcon.Question);
            Stop = new Dialog(MessageBoxIcon.Stop);
            Hand = new Dialog(MessageBoxIcon.Hand);
        }
        public static Dialog Warning { get; internal set; }
        public static Dialog Info { get; internal set; }
        public static Dialog Error { get; internal set; }
        public static Dialog Def { get; internal set; }
        public static Dialog Question { get; internal set; }
        public static Dialog Stop { get; internal set; }
        public static Dialog Hand { get; internal set; }
        public bool YesNo(string confirmationMessage, string caption = "Confirm", IWin32Window owner = null)
        {
            return Box(confirmationMessage, caption, MessageBoxButtons.YesNo, owner) == DialogResult.Yes;
        }
        public bool? YesNoCancel(string confirmationMessage, string caption = "Confirm", IWin32Window owner = null)
        {
            var result = Box(confirmationMessage, caption, MessageBoxButtons.YesNoCancel, owner);
            if (result == DialogResult.Cancel) return null;
            return result == DialogResult.Yes;
        }
        public void Ok(string alertMessage, string caption = "Alert", IWin32Window owner = null)
        {
            Box(alertMessage, caption, MessageBoxButtons.OK, owner);
        }
        public bool OkCancel(string alertMessage, string caption = "Alert", IWin32Window owner = null)
        {
            return Box(alertMessage, caption, MessageBoxButtons.OKCancel, owner) == DialogResult.OK;
        }
        private DialogResult Box(string message, string caption, MessageBoxButtons buttons, IWin32Window owner)
        {
            if (owner == null)
                return MessageBox.Show(message, caption, buttons, Icon);
            else
                return MessageBox.Show(owner, message, caption, buttons, Icon);
        }
    }
}
