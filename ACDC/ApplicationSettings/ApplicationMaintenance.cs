using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ApplicationSettings
{
    public partial class ApplicationMaintenance : Form
    {
        private ProcessLogRun LogRun { get; set; }
        private List<Arguments> SelectedArgs { get; set; }
        private List<Arguments> AvailableArgs { get; set; }
        private int SqlUserId { get; set; }
        private Applications SelectedApplication { get; set; }
        private List<Applications> apps { get; set; }
        public DataAccess DA { get; set; }
        public string ImageLocation { get; set; }

        public ApplicationMaintenance(ProcessLogRun logRun, int sqlUserId)
        {
            InitializeComponent();
            apps = new List<Applications>();
            LogRun = logRun;
            DA = new DataAccess(logRun);
            SqlUserId = sqlUserId;
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionNumber.Text = string.Format("Version {0}.{1}.{2}:{3}", version.Major, version.Minor, version.Build, version.Revision);

            LoadComboBoxes();
        }

        private void ApplicationsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ApplicationsList.SelectedIndex > 0)
            {
                Applications app = DA.GetApplications().Where(p => p.ApplicationId == ((Applications)ApplicationsList.SelectedItem).ApplicationId).Single();
                SelectedApplication = app;
                ClearForm();
                New.Text = "New";

                //Set the application type
                if (app.StartingDll.EndsWith(".dll"))
                    TypeDll.Checked = true;
                else if (app.StartingDll.EndsWith(".exe"))
                    TypeExe.Checked = true;
                else if (app.StartingDll.EndsWith(".mdb"))
                    TypeMdb.Checked = true;
                else
                    TypeHttp.Checked = true;

                ApplicationText.Text = app.ApplicationName;
                ApplicationName.Text = app.ApplicationName;
                AccessKey.SelectedText = app.AccessKey;
                StartingClass.Text = app.StartingClass;
                StartingDll.Text = app.StartingDll;
                SourcePath.Text = app.SourcePath;

                //Set the arguments
                foreach (Arguments arg in app.Arguments)
                {
                    SelectedArgs.Add(arg);
                    AvailableArgs.Remove(AvailableArgs.Where(p => p.Argument == arg.Argument).Single());
                    SetArguments();
                }

                Delete.Enabled = true;
                Selected.ClearSelected();
                Available.ClearSelected();

                //Set the image
                string imageLocation = string.Format(DA.GetImagePath("ACDC_Image"), DataAccessHelper.CurrentMode, app.ApplicationName);
                if (File.Exists(imageLocation))
                    ApplicationImage.BackgroundImage = (Image)(new Bitmap(Image.FromFile(imageLocation)));
                else
                    ApplicationImage.BackgroundImage = null;
                Save.Text = "Update";
                Save.Enabled = true;
            }
        }

        private void New_Click(object sender, EventArgs e)
        {
            ClearForm();
            New.Text = "Clear Form";
        }

        private void TypeDll_CheckedChanged(object sender, EventArgs e)
        {
            SourcePath.ReadOnly = true;
            SetSource.Enabled = true;
            Selected.Enabled = true;
            Available.Enabled = true;
            StartingClass.Enabled = true;
            StartingDll.Enabled = true;
            ApplicationName.Enabled = true;
            AccessKey.Enabled = true;
            Upload.Enabled = true;
            dllText.Text = "Starting DLL";
            if (StartingDll.Text.IsPopulated())
                StartingDll.Text = StartingDll.Text.Remove(StartingDll.Text.Length - 4, 4) + ".dll";
        }

        private void TypeExe_CheckedChanged(object sender, EventArgs e)
        {
            SourcePath.ReadOnly = true;
            SetSource.Enabled = true;
            Selected.Enabled = true;
            Available.Enabled = true;
            StartingClass.Enabled = false;
            StartingDll.Enabled = true;
            ApplicationName.Enabled = true;
            AccessKey.Enabled = true;
            Upload.Enabled = true;
            dllText.Text = "Starting EXE";
            if (StartingDll.Text.IsPopulated())
                StartingDll.Text = StartingDll.Text.Remove(StartingDll.Text.Length - 4, 4) + ".exe";
        }

        private void TypeMdb_CheckedChanged(object sender, EventArgs e)
        {
            SourcePath.ReadOnly = true;
            SetSource.Enabled = true;
            Selected.Enabled = true;
            Available.Enabled = true;
            StartingClass.Enabled = false;
            StartingDll.Enabled = true;
            ApplicationName.Enabled = true;
            AccessKey.Enabled = true;
            Upload.Enabled = true;
            dllText.Text = "Starting MDB";
            if (StartingDll.Text.IsPopulated())
                StartingDll.Text = StartingDll.Text.Remove(StartingDll.Text.Length - 4, 4) + ".mdb";
        }

        private void TypeHttp_CheckedChanged(object sender, EventArgs e)
        {
            SourcePath.ReadOnly = false;
            SetSource.Enabled = false;
            StartingClass.Enabled = false;
            StartingDll.Enabled = false;
            Available.Enabled = false;
            Selected.Enabled = false;
            ApplicationName.Enabled = true;
            AccessKey.Enabled = true;
            Upload.Enabled = true;
            SourcePath.Text = @"http:\\";
            dllText.Text = "Starting DLL";
        }

        private void StartingDll_Leave(object sender, EventArgs e)
        {
            if (StartingDll.Text != "" && !(StartingDll.Text.Contains(".dll") || StartingDll.Text.Contains(".exe") || StartingDll.Text.Contains(".mdb")))
            {
                if (TypeDll.Checked)
                    StartingDll.Text = StartingDll.Text + ".dll";
                if (TypeExe.Checked)
                    StartingDll.Text = StartingDll.Text + ".exe";
                if (TypeMdb.Checked)
                    StartingDll.Text = StartingDll.Text + ".mdb";

                StartingDll.Text = StartingDll.Text.Trim();
            }
        }

        private void StartingDll_Enter(object sender, EventArgs e)
        {
            if (StartingDll.Text != "")
            {
                if (TypeDll.Checked)
                    StartingDll.Text = StartingDll.Text.Replace(".dll", "");
                if (TypeExe.Checked)
                    StartingDll.Text = StartingDll.Text.Replace(".exe", "");
                if (TypeMdb.Checked)
                    StartingDll.Text = StartingDll.Text.Replace(".mdb", "");
            }
        }

        private void ApplicationName_TextChanged(object sender, EventArgs e)
        {
            if (ApplicationName.Text.IsPopulated() && SourcePath.Text.IsPopulated())
                Save.Enabled = true;
            else
                Save.Enabled = false;
        }

        private void AddRemove_Click(object sender, EventArgs e)
        {
            if (Selected.SelectedItems.Count > 0)
            {
                Arguments arg = SelectedArgs.Where(p => p.Argument == ((Arguments)Selected.SelectedItem).Argument).Single();
                SelectedArgs.Remove(arg);
                AvailableArgs.Add(arg);
                SetArguments();
            }
            else if (Available.SelectedItems.Count > 0)
            {
                Arguments arg = AvailableArgs.Where(p => p.Argument == ((Arguments)Available.SelectedItem).Argument).Single();
                AvailableArgs.Remove(arg);
                SelectedArgs.Add(arg);
                SetArguments();
            }
            if (Available.SelectedIndex == -1 && Selected.SelectedIndex == -1)
                AddRemove.Visible = false;
        }

        private void Up_Click(object sender, EventArgs e)
        {
            MoveItem(-1);
        }

        private void Down_Click(object sender, EventArgs e)
        {
            MoveItem(1);
        }

        private void Selected_Click(object sender, EventArgs e)
        {
            AddRemove.BackgroundImage = ApplicationSettings.Properties.Resources.Left;
            Available.ClearSelected();
        }

        private void Available_Click(object sender, EventArgs e)
        {
            AddRemove.BackgroundImage = ApplicationSettings.Properties.Resources.Right;
            Selected.ClearSelected();
        }

        private void Available_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Available.SelectedIndex != -1)
                AddRemove.Visible = true;
        }

        private void Selected_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Selected.SelectedIndex != -1)
                AddRemove.Visible = true;
        }

        private void Upload_Click(object sender, EventArgs e)
        {
            OpenFileDialog pic = new OpenFileDialog();
            pic.Title = "Choose the image to upload";
            pic.Filter = "Image Files (*.bmp, *.jpg, *.gif, *.png, *.ico)|*.bmp; *.jpg; *.gif; *.png; *.ico";
            pic.ShowDialog();
            if (pic.FileName.IsPopulated())
            {
                ApplicationImage.BackgroundImage = (Image)(new Bitmap(Image.FromFile(pic.FileName), new Size(50, 50)));
                ImageLocation = pic.FileName;
            }
            else
                ApplicationImage.BackgroundImage = null;
            this.Refresh();
        }

        private void SetSource_Click(object sender, EventArgs e)
        {
            if (TypeDll.Checked || TypeExe.Checked)
            {
                OpenFolderDialog dialog = new OpenFolderDialog();
                dialog.ShowDialog(this.Handle, false);
                if (dialog.Folder.IsPopulated())
                {
                    if (dialog.Folder.Length <= 256)
                    {
                        SourcePath.Text = ReplaceMode(dialog.Folder);
                        if (ApplicationName.Text.IsPopulated())
                            Save.Enabled = true;
                    }
                    else
                        SourcePath.Text = "The Source Path is too long";
                }
            }
            else if (TypeMdb.Checked)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Access Files (*.mdb)|*.mdb";
                dialog.ShowDialog();
                if (dialog.FileName != string.Empty)
                {
                    SourcePath.Text = dialog.FileName;
                    Save.Enabled = true;
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            ApplicationName.Text = ApplicationName.Text.Trim();
            if (apps.Any(p => p.ApplicationName == ApplicationName.Text) && Save.Text == "Save")
            {
                MessageBox.Show(string.Format("There is already an application with the name {0}", ApplicationName.Text), "Application Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Check if the application is being updated
            if (SelectedApplication != null)
                DA.DeleteApplication(SelectedApplication.ApplicationId, SqlUserId);
            //Save the application
            if ((TypeDll.Checked && (StartingClass.Text.IsPopulated() && StartingDll.Text.IsPopulated()))
                || (TypeExe.Checked && (StartingClass.Text.IsNullOrEmpty() && StartingDll.Text.IsPopulated()))
                || (TypeMdb.Checked && (StartingClass.Text.IsNullOrEmpty() && StartingDll.Text.IsPopulated())))
            {
                int appId = DA.AddApplication(ApplicationName.Text, AccessKey.Text, StartingClass.Text, StartingDll.Text, SqlUserId, SourcePath.Text);
                List<Arguments> args = new List<Arguments>();
                for (int i = 0; i < Selected.Items.Count; i++)
                {
                    int argId = ((Arguments)Selected.Items[i]).ArgumentId;
                    DA.AddApplicationArguments(appId, argId, i);
                }
                UploadImage();
            }
            else if (TypeHttp.Checked)
            {
                DA.AddApplication(ApplicationName.Text, "", "", "", SqlUserId, SourcePath.Text);
                UploadImage();
            }
            ClearForm();
            LoadComboBoxes();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            DA.DeleteApplication(DA.GetApplications().Where(p => p.ApplicationName == ApplicationText.Text).Single().ApplicationId, SqlUserId);
            ClearForm();
            LoadComboBoxes();
        }

        public void UploadImage()
        {
            try
            {
                if (ApplicationImage.BackgroundImage != null)
                {
                    //Upload the image to the Images folder on CS1, resize the image to 50x50 and save it as a jpg
                    //The image will be renamed as the same name of the application being inserted
                    Bitmap mp = (Bitmap)ApplicationImage.BackgroundImage;
                    string path = string.Format(DA.GetImagePath("ACDC_Image"), DataAccessHelper.CurrentMode, ApplicationName.Text.Trim());
                    if (File.Exists(path))
                    {
                        if (Dialog.Info.OkCancel("There is already an image assigned. Do you want to delete the old image and add the new one? If you click yes, ACDC will shut down and copy the new image."))
                        {
                            string myLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                            ProcessStartInfo proc = new ProcessStartInfo(Path.Combine(myLocation, "UpdateAndStart.exe"));
                            proc.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\"",
                                DataAccessHelper.CurrentMode.ToString(),
                                ImageLocation,
                                ApplicationName.Text.Trim());
                            proc.UseShellExecute = false;
                            Process.Start(proc);
                            Environment.Exit(1);
                            ACDC.ACDCMain acdcMain = new ACDC.ACDCMain();
                            acdcMain.Exit();
                        }
                    }
                    else
                        mp.Save(path, ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error uploading the image", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
        }

        /// <summary>
        /// Load the data from the database into the combo boxes
        /// </summary>
        private void LoadComboBoxes()
        {
            apps = DA.GetApplications().OrderBy(p => p.ApplicationName).ToList();
            apps.Insert(0, new Applications());
            //Remove the Application Settings so it doesn't get changed or deleted
            apps.Remove(apps.Where(p => p.ApplicationName == "Application Settings").Single());
            ApplicationsList.DataSource = apps;
            ApplicationsList.DisplayMember = "ApplicationName";
            ApplicationsList.ValueMember = "ApplicationId";

            List<AccessKeys> keys = DA.GetAccessKeys().OrderBy(p => p.Application).ToList();
            keys.Insert(0, new AccessKeys());
            AccessKey.DataSource = keys;
            AccessKey.DisplayMember = "Name";

            SelectedArgs = new List<Arguments>();
            AvailableArgs = DA.GetArguments().OrderBy(p => p.Argument).ToList();
            SetArguments();
            Selected.ClearSelected();
            Available.ClearSelected();
        }

        /// <summary>
        /// Sets the data source for the argument list boxes
        /// </summary>
        public void SetArguments()
        {
            Selected.Items.Clear();
            foreach (Arguments arg in SelectedArgs)
            {
                if (!Selected.Items.Contains(arg))
                    Selected.Items.Add(arg);
            }
            Selected.DisplayMember = "Argument";
            Selected.ValueMember = "ArgumentId";

            Available.DataSource = null;
            Available.DataSource = AvailableArgs.OrderByDescending(p => p.Argument).ToList();
            Available.DisplayMember = "Argument";
            Available.ValueMember = "ArgumentId";
        }

        /// <summary>
        /// Moves the selected item up or down
        /// </summary>
        /// <param name="direction">The direction to move the selected item</param>
        private void MoveItem(int direction)
        {
            //Don't try and move it there is not a selected item or it is the first item in the array
            if (Selected.SelectedItem == null || Selected.SelectedIndex < 0)
                return;
            //Increase or decrease the index according to direction being sent in
            int newIndex = Selected.SelectedIndex + direction;
            //Do not move if the new index is greater than the number in the array
            if (newIndex < 0 || newIndex >= Selected.Items.Count)
                return;
            //Get a copy of the selected item
            var selected = Selected.SelectedItem;
            //Remove the item from the listbox
            Selected.Items.Remove(selected);
            //Add the object back to the listbox in the new position
            Selected.Items.Insert(newIndex, selected);
            //Select the new item to show where it is located in the new position
            Selected.SetSelected(newIndex, true);
        }

        /// <summary>
        /// If the path has a mode, replace the mode with {0}
        /// </summary>
        /// <param name="path">The path where the application resides</param>
        /// <returns>a new path replacing the mode, if there is a mode</returns>
        private string ReplaceMode(string path)
        {
            if (path.ToUpper().Contains(@"\DEV\"))
                return path.Replace(@"\Dev\", @"\{0}\");
            else if (path.ToUpper().Contains(@"\TEST\"))
                return path.Replace(@"\Test\", @"\{0}\");
            else if (path.ToUpper().Contains(@"\QA\"))
                return path.Replace(@"\Qa\", @"\{0}\");
            else if (path.ToUpper().Contains(@"\LIVE\"))
                return path.Replace(@"\Live\", @"\{0}\");

            return path;
        }

        /// <summary>
        /// Clears every control on the form
        /// </summary>
        private void ClearForm()
        {
            TypeDll.Select();
            ApplicationsList.SelectedIndex = 0;
            ApplicationName.Text = string.Empty;
            AccessKey.Text = string.Empty;
            StartingClass.Text = string.Empty;
            StartingDll.Text = string.Empty;
            SourcePath.Text = string.Empty;
            Delete.Enabled = false;
            Save.Enabled = true;
            Selected.Enabled = true;
            Available.Enabled = true;
            if (SelectedArgs != null && AvailableArgs != null)
            {
                SelectedArgs.Clear();
                AvailableArgs.Clear();
            }
            AvailableArgs = DA.GetArguments();
            SetArguments();
            ApplicationText.Text = string.Empty;
            ApplicationImage.BackgroundImage = null;
            TypeDll.Checked = false;
            TypeExe.Checked = false;
            TypeMdb.Checked = false;
            TypeHttp.Checked = false;
            Save.Text = "Save";
            Save.Enabled = false;
        }
    }
}