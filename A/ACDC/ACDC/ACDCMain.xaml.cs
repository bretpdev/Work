using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using SubSystemShared;


namespace ACDC
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    partial class ACDCMain : Window
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr adHandle);

        #region "Moving Window Code"

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ReleaseCapture();
                SendMessage((new WindowInteropHelper(this).Handle), WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        private int _sqlUserId { get; set; } //Remains 0 until AuthenticateUser() is called.
        private List<string> _userRoles { get; set; }
        private string role { get; set; }
        private string userName = Environment.UserName.ToLower();
        ProcessLogRun LogRun { get; set; }

        public ACDCMain()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            SetMode();
            LogRun = new ProcessLogRun("ACDC", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
            if (!CheckStartingLocation())
            {
                LogRun.AddNotification("Attempting to launch appliation from network location", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                LogRun.LogEnd();
                this.Close();
                return;
            }
#if !DEBUG
            if (UpdateHelper.NewVersionAvailable)
                UpdateHelper.Update();
#endif

            List<Process> adProcess = Process.GetProcessesByName("ACDC").ToList();

            //Check to see if there is already an instance of ACDC running.
            if (adProcess.Count() > 1)
            {
                MessageBox.Show("There is already an instance of ACDC running", "ACDC already running", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                InitializeComponent();
                this.Show();

                SetFormControls();
                role = Common.AuthenticateUser(userName, LogRun);
                _userRoles = Common.GetActiveDirectoryGroups(userName);
                //Set _sqlUserId from the database.
                _sqlUserId = DataAccess.SqlUserId(userName);
                if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live && !(role.Contains("Application Development") || role.Contains("Systems Support") || role.Contains("Business Systems Analyst")))
                {
                    string message = "You are starting ACDC in Development mode but are not a member of the Development team. If you are trying to launch ACDC in live, contact a member of Systems Support.";
                    Dialog.Warning.Ok(message, "Development Mode");
                }
                try
                {
                    MakeOptionsVisibleAccordingToAccess();
                }
                catch (Exception ex)
                {
                    string message = "ACDC encountered an issue when adding applications to the form.";
                    LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Checks the starting location and makes sure the user does not start from a network drive
        /// </summary>
        /// <returns></returns>
        private bool CheckStartingLocation()
        {
#if !DEBUG
            var location = Assembly.GetEntryAssembly().Location;
            if (location.Contains("cs1"))
            {
                Dialog.Info.Ok(@"You are attempting to start this from a network drive. Please start the application from C:\Enterprise Program Files\ACDC\");
                return false;
            }
#endif
            return true;
        }

        /// <summary>
        /// Set the current mode to be sent to applications being launched
        /// </summary>
        private void SetMode()
        {
            string[] args = Environment.GetCommandLineArgs();
            string arg = args.Count() > 1 ? args[1].ToUpper() : "";
            switch (arg.ToUpper())
            {
                case "QA":
                    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.QA;
                    break;
                case "TEST":
                    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Test;
                    break;
                case "DEV":
                    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
                    break;
                case "LIVE":
                    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
                    break;
                default:
                    DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
                    break;
            }
        }

        /// <summary>
        /// Add the version to the form and set the background color to Red when in dev mode.
        /// </summary>
        private void SetFormControls()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            VersionNumber.Content = string.Format("Version {0}.{1}.{2}:{3}", version.Major, version.Minor, version.Build, version.Revision);
            Mode.Content = DataAccessHelper.CurrentMode.ToString();

            if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live)
            {
                rectangle1.Fill = Brushes.Red;
            }
        }

        /// <summary>
        /// Add buttons to the form for each application the user has access to
        /// </summary>
        public void MakeOptionsVisibleAccordingToAccess()
        {
            List<Applications> allApps = DataAccess.Applications();
            List<Applications> apps = DataAccess.Applications()
                                             .Where(app => string.IsNullOrEmpty(app.AccessKey) || DataAccess.UserHasAccess(role, app.AccessKey))
                                             .OrderBy(p => p.ApplicationName).ToList();

            RemoveApplications(allApps, apps);
            List<ApplicationButton> buttons = new List<ApplicationButton>();
            AddButtonsToForm(apps, buttons);

            Thread loadThread = new Thread(() =>
            {
                CopyApplicationToLocal(buttons);
            });
            loadThread.SetApartmentState(ApartmentState.STA);
            loadThread.Start();
        }

        /// <summary>
        /// Gets a list of all applicaitons the user has access to and copies them to the local machine
        /// </summary>
        /// <param name="buttons"></param>
        private void CopyApplicationToLocal(List<ApplicationButton> buttons)
        {
            foreach (ApplicationButton button in buttons)
            {
                Applications app = button.Application;
                app.Arguments = new List<Arguments>(DataAccess.Arguments(app.ApplicationID));

                //Set the source path, it needs to be set for all types
                app.SourcePath = string.Format(app.SourcePath, DataAccessHelper.CurrentMode.ToString());
                //Set the destination and copy files for everything other than a URL, no files needed
                if (!app.SourcePath.ToUpper().Contains("HTTP"))
                {
                    if (!CreateLocalCopyLibrary(app.SourcePath, app.DestinationPath))
                    {
                        Dispatcher.Invoke(new Action(() => MainPanel.Children.Remove(button)));
                        SyncWidth();
                        if (Directory.Exists(app.DestinationPath) && !app.DestinationPath.Contains("Images"))
                            DeleteDirectory(app.DestinationPath);
                    }
                    Dispatcher.Invoke(new Action(() => button.RefreshBackgroundImage()));
                }
                Dispatcher.Invoke(new Action(() => button.Loading = false));
            }
        }

        /// <summary>
        /// Adds buttons to the main UI for each application that was loaded to the local machine.
        /// </summary>
        /// <param name="apps">List of applications user has access to</param>
        /// <param name="buttons">List of all buttons that applications will be added to</param>
        private void AddButtonsToForm(List<Applications> apps, List<ApplicationButton> buttons)
        {
            foreach (Applications app in apps)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    ApplicationButton button = new ApplicationButton(app, (int)DataAccessHelper.CurrentMode, _sqlUserId, role, _userRoles, LogRun);
                    buttons.Add(button);
                    MainPanel.Children.Add(button);
                }));
            }
            SyncWidth();
        }

        /// <summary>
        /// Removes any applications that were not loaded to the local machine or that the user does not have access to.
        /// </summary>
        /// <param name="allApps">All available applications</param>
        /// <param name="apps">The applications the user has access to</param>
        private void RemoveApplications(List<Applications> allApps, List<Applications> apps)
        {
            //Delete software from local machine if they do not have access
            foreach (Applications application in new List<Applications>(allApps))
            {
                if (!apps.Any(p => p.ApplicationID == application.ApplicationID))
                {
                    if (Directory.Exists(application.DestinationPath))
                        DeleteDirectory(application.DestinationPath);
                }
            }
        }

        /// <summary>
        /// Deletes a folder and everything in it. Adds a notification to processlogger if not successful.
        /// </summary>
        /// <param name="path">The path of the folder to be deleted</param>
        private void DeleteDirectory(string path)
        {
            try
            {
                Repeater.TryRepeatedly(() => Directory.Delete(path, true));
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error deleting file(s)", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        /// <summary>
        /// Copies all file from the applicaiton folder and subfolders from the network to the local machine
        /// </summary>
        /// <param name="networkLocation">Location of the network folder</param>
        /// <param name="localLocation">Location of the local folder</param>
        /// <param name="app">The application that is being copied to the local location</param>
        private bool CreateLocalCopyLibrary(string networkLocation, string localLocation)
        {
            DirectoryInfo source = new DirectoryInfo(networkLocation);
            DirectoryInfo destination = new DirectoryInfo(localLocation);

            if (!Directory.Exists(localLocation))
                Directory.CreateDirectory(localLocation);

            //Copy all applications that are built in Access
            if (networkLocation.ToLower().EndsWith(".mdb"))
            {
                if (Directory.Exists(localLocation) == false)
                    Directory.CreateDirectory(localLocation);

                string localFile = Path.Combine(localLocation, Path.GetFileName(networkLocation));

                if (!File.Exists(localFile) || new FileInfo(networkLocation).Length != new FileInfo(localFile).Length)
                    File.Copy(networkLocation, localFile, true);
                return true;
            }

            if (source.Exists)
            {
                //Create a subfolders and copy all files in each subfolder
                foreach (DirectoryInfo dir in source.GetDirectories())
                    CreateLocalCopyLibrary(dir.FullName, destination.CreateSubdirectory(dir.Name).FullName);
            }
            else
                return false;

            //Copy the files for the current folder
            if (!CopyFiles(source, destination))
                return false;

            return true;
        }

        /// <summary>
        /// Copys the files from the source to the destination folder
        /// </summary>
        /// <param name="source">Source location, most likely in CS1</param>
        /// <param name="destination">Destination location, most likely in Enterprise Program Files</param>
        /// <returns>True if all files copy</returns>
        private bool CopyFiles(DirectoryInfo source, DirectoryInfo destination)
        {
            foreach (FileInfo file in source.GetFiles())
            {
                string destinationFile = Path.Combine(destination.FullName, file.Name);

                try
                {
                    if (FilesAreDifferent(file, destinationFile))
                        File.Copy(file.FullName, destinationFile, true);
                }
                catch (Exception ex)
                {
                    string message = string.Format("The file {0} is in use.  Please close the application using this file and relaunch ACDC.\n\n{1}", destinationFile, ex.Message);
                    Dialog.Info.Ok(message, "File copy error");
                    return false;
                }
            }
            return true;
        }

        private bool FilesAreDifferent(FileInfo source, string destination)
        {
            if (!File.Exists(destination))
                return true; //local copy doesn't exist
            var dest = new FileInfo(destination);
            if (source.LastWriteTime != dest.LastWriteTime)
                return true;  //network location is newer
            else if (source.Length != dest.Length)
                return true;  //files aren't same size
            else if (source.Extension.ToLower().IsIn(".exe", ".dll"))
            {
                string sourceVersion = FileVersionInfo.GetVersionInfo(source.FullName).FileVersion;
                string destVersion = FileVersionInfo.GetVersionInfo(destination).FileVersion;
                if (sourceVersion != destVersion)
                    return true; //everything else is identical, but network is clearly on a different assembly version
            }
            return false;
        }

        /// <summary>
        /// Sync's the width of the form depending on the number of apps added
        /// </summary>
        private void SyncWidth()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                MainPanel.Width = Math.Ceiling(Math.Sqrt(MainPanel.Children.Count)) * 60;
            }));
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClose.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            btnClose.Visibility = Visibility.Hidden;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            LogRun.LogEnd();
            this.Close();
        }

        public void Exit()
        {
            LogRun.LogEnd();
            Environment.Exit(0);
        }

    }
}