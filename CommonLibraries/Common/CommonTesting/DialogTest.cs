using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace CommonTesting
{
    public partial class DialogTest : Form
    {
        public DialogTest()
        {
            InitializeComponent();
        }
        const string message = "Messagebox Message";
        const string caption = "Messagebox Title";
        private void Button_Click(object sender, EventArgs e)
        {
            string buttonName = (sender as Button).Name;
            Dialog d = null;
            if (buttonName.StartsWith("Warning"))
                d = Dialog.Warning;
            else if (buttonName.StartsWith("Info"))
                d = Dialog.Info;
            else if (buttonName.StartsWith("Error"))
                d = Dialog.Error;
            else if (buttonName.StartsWith("Default"))
                d = Dialog.Def;
            bool? result = null;
            if (buttonName.Contains("OkCancel"))
                result = d.OkCancel(message, caption);
            else if (buttonName.Contains("Ok"))
                d.Ok(message, caption);
            else if (buttonName.Contains("YesNoCancel"))
                result = d.YesNoCancel(message, caption);
            else if (buttonName.Contains("YesNo"))
                result = d.YesNo(message, caption);
            Dialog.Def.Ok("Result was: " + result.ToString());
        }
    }
}
