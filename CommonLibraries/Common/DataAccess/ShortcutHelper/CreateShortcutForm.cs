using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uheaa.Common.DataAccess
{
    public partial class CreateShortcutForm : Form
    {
        string title = null;
        public CreateShortcutForm(string title)
        {
            InitializeComponent();
            this.title = title;
            this.Text = string.Format(this.Text, title);
        }

        private void DevButton_Click(object sender, EventArgs e)
        {
            CreateShortcut("dev");
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            CreateShortcut("test");
        }

        private void QAButton_Click(object sender, EventArgs e)
        {
            CreateShortcut("qa");
        }

        private void LiveButton_Click(object sender, EventArgs e)
        {
            CreateShortcut("live");
        }

        private void CreateShortcut(string postFix)
        {
            string exePath = Assembly.GetEntryAssembly().Location;
            string shortcutPath = exePath.Replace(".exe", " - " + postFix.ToUpper() + ".lnk");
            if (System.IO.File.Exists(shortcutPath))
                FS.Delete(shortcutPath);
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = exePath;
            shortcut.Arguments = postFix.ToLower();
            shortcut.Save();

            Proc.Start(shortcutPath);

            this.Close();
        }
    }
}
