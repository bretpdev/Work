using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetTester
{
    public partial class MainForm : Form
    {
        private NtFile loadedFile = null;
        bool shouldClose = false;
        public MainForm(string initialPath)
        {
            InitializeComponent();
            if (string.IsNullOrWhiteSpace(initialPath))
                NewFile();
            else
            {
                OpenFile(initialPath);
                if (loadedFile.AutoLaunch)
                {
                    if (HasUpdate())
                        UpdateLocalCopy();
                    Launch();
                    shouldClose = true;
                }
            }

        }
        private void Bind()
        {
            NetworkLocationText.DataBindings.Clear();
            NetworkLocationText.DataBindings.Add("Text", loadedFile, "Location", true, DataSourceUpdateMode.OnPropertyChanged);
            ArgumentsText.DataBindings.Clear();
            ArgumentsText.DataBindings.Add("Text", loadedFile, "Arguments", true, DataSourceUpdateMode.OnPropertyChanged);
            AutoUpdateBox.DataBindings.Clear();
            AutoUpdateBox.DataBindings.Add("Checked", loadedFile, "AutoLaunch", true, DataSourceUpdateMode.OnPropertyChanged);

            SetButtons();
        }
        private void NewFile()
        {
            loadedFile = new NtFile();
            this.Text = "Network Test - NEW";
            Bind();
        }
        private void OpenFile(string path)
        {
            path = ValidatePath(path);
            var newFile = new NtFile();
            string errorMessage = null;
            try
            {
                var results = newFile.Load(path);
                if (!results.Successful)
                    errorMessage = string.Format("Could not open {0}, the following arguments were invalid: {1}", loadedFile.FileName, string.Join(",", results.UnknownKeys));
                newFile.Location = ValidatePath(newFile.Location);
            }
            catch (Exception ex)
            {
                errorMessage = string.Format("Unable to open file {0}: {1}", path, ex.ToString());
            }
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "NetTester");
                return;
            }
            loadedFile = newFile;
            this.Text = "Network Test - " + path;
            Bind();
        }

        private string ValidatePath(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
                return path;
            string replacedPath = path.Replace("X:", @"\\UHEAA-FS\SEAS")
                                      .Replace("Y:", @"\\UHEAA-FS\DEVSEASCS")
                                      .Replace("Z:", @"\\UHEAA-FS\SEASCS")
                                      .Replace("T:", @"\\UHEAA-FS\DomainUsersData\" + Environment.UserName + @"\Temp");
            return replacedPath;
        }

        private void SetButtons()
        {
            string status = null;
            bool updateEnabled = UpdateButton.Enabled;
            string launchText = LaunchButton.Text;
            bool launchEnabled = LaunchButton.Enabled;
            StatusLabel.Text = "Calculating Status...";
            Task.Factory.StartNew(new Action(() =>
            {
                if (string.IsNullOrWhiteSpace(loadedFile.Location) || !File.Exists(loadedFile.Location))
                {
                    status = "No Valid Location Set";
                    updateEnabled = launchEnabled = false;
                }
                else
                {
                    launchEnabled = updateEnabled = true;
                    launchText = "Update and Launch Local Copy";
                    if (!LocalExists())
                        status = "No Local Copy Found";
                    else if (HasUpdate())
                        status = "Network Has Update";
                    else
                    {
                        updateEnabled = false;
                        launchText = "Launch Local Copy";
                        status = "Local Copy Up-To-Date";
                    }
                }

                this.BeginInvoke(new Action(() =>
                {
                    if (string.IsNullOrWhiteSpace(loadedFile.FileName))
                        SaveMenu.Enabled = false;
                    else
                        SaveMenu.Enabled = true;
                    StatusLabel.Text = "Status: " + status;
                    UpdateButton.Enabled = updateEnabled;
                    LaunchButton.Enabled = launchEnabled;
                    LaunchButton.Text = launchText;
                }));
            }));
        }

        private void SettingsMenu_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog(this);
        }

        string filter = "NetTester File (*.nt)|*.nt";
        private void OpenMenu_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = filter;
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    OpenFile(ofd.FileName);
            }
        }

        private void SaveMenu_Click(object sender, EventArgs e)
        {
            try
            {
                loadedFile.Save();
            }
            catch (Exception ex)
            {
                string message = string.Format("Unable to save file to {0}: {1}", loadedFile.FileName, ex.ToString());
                MessageBox.Show(message, "NetTester");
            }
        }

        private void SaveAsMenu_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = filter;
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        loadedFile.Save(sfd.FileName);
                        OpenFile(sfd.FileName);
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Unable to save file to {0}: {1}", loadedFile.FileName, ex.ToString());
                        MessageBox.Show(message, "NetTester");
                    }
                }
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            UpdateLocalCopy();
            SetButtons();
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            UpdateLocalCopy();
            Launch();
            SetButtons();
        }

        private bool LocalExists()
        {
            string localPath = loadedFile.GetLocalLocation(Properties.Settings.Default.DownloadLocation);
            return Directory.Exists(localPath);
        }

        private bool HasUpdate()
        {
            string localPath = loadedFile.GetLocalLocation(Properties.Settings.Default.DownloadLocation);
            localPath = ValidatePath(localPath);
            if (!Directory.Exists(localPath))
                return true;
            foreach (var networkFile in Directory.GetFiles(Path.GetDirectoryName(loadedFile.Location)))
            {
                var localFile = Path.Combine(localPath, Path.GetFileName(networkFile));
                if (!File.Exists(localFile))
                    return true;
                using (var md5 = MD5.Create())
                using (var localStream = File.OpenRead(localFile))
                using (var networkStream = File.OpenRead(networkFile))
                    if (!md5.ComputeHash(localStream).SequenceEqual(md5.ComputeHash(networkStream)))
                        return true;
            }
            return false;
        }

        private void UpdateLocalCopy()
        {
            string localPath = loadedFile.GetLocalLocation(Properties.Settings.Default.DownloadLocation);
            localPath = ValidatePath(localPath);
            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);
            foreach (var localFile in Directory.GetFiles(localPath))
                File.Delete(localFile);
            foreach (var networkFile in Directory.GetFiles(Path.GetDirectoryName(loadedFile.Location)))
            {
                string localFile = Path.Combine(localPath, Path.GetFileName(networkFile));
                File.Copy(networkFile, localFile);
            }
        }

        private void Launch()
        {
            string localLocation = loadedFile.GetLocalLocation(Properties.Settings.Default.DownloadLocation);
            localLocation = Path.Combine(localLocation, Path.GetFileName(loadedFile.Location));
            localLocation = ValidatePath(localLocation);
            var args = loadedFile.Arguments;
            #region Username/Password arguments
            string usernameKey = "{{session_user_id}}";
            string passwordKey = "{{session_password}}";
            string username = Properties.Settings.Default.SessionUsername;
            string password = Properties.Settings.Default.SessionPassword;
            if (args.Contains(usernameKey))
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    using (var user = new InputBox("Session User ID"))
                    {
                        if (user.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            username = user.Input;
                        else
                            return;
                    }
                }
            }
            if (args.Contains(passwordKey))
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    using (var pass = new InputBox("Session Password", '*'))
                    {
                        if (pass.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            password = pass.Input;
                        else
                            return;
                    }
                }
            }
            args = args.Replace(usernameKey, username).Replace(passwordKey, password);
            #endregion
            #region Prompted Args
            string promptKey = "{{prompt:";
            while (args.Contains(promptKey))
            {
                int promptStart = args.IndexOf(promptKey) + promptKey.Length;
                int promptEnd = args.IndexOf("}}", promptStart);
                string prompt = args.Substring(promptStart, promptEnd - promptStart).Trim();
                using (var promptBox = new InputBox(prompt))
                {
                    if (promptBox.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        args = args.Substring(0, promptStart - promptKey.Length) + promptBox.Input + args.Substring(promptEnd + 2);
                    else
                        return;
                }
            }
            #endregion
            Process.Start(localLocation, args);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "EXE Files (*.exe)|*.exe";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    NetworkLocationText.Text = ofd.FileName;
                }
            }
        }

        private void NetworkLocationText_TextChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                SetButtons();
            }));
        }

        private void AssociateButton_Click(object sender, EventArgs e)
        {
            if (Uheaa.Common.Dialog.Def.YesNo("Attempt to associate *.nt files?"))
            {
                string path = Application.ExecutablePath;
                Process.Start("Associator.exe", path);
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            OpenFile(files[0]);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (shouldClose)
                this.Close();
        }
    }
}
