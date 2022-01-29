using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using WinForms = System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;

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

        public enum Mode
        {
            Live = 0,
            QA = 1,
            Test = 2,
            Dev = 3
        }

        private readonly DataAccess _dataAccess;
        private int _sqlUserId; //Remains 0 until AuthenticateUser() is called.
        private List<string> _userRoles;
        private string role;
        private Mode _mode;
        private string userName;

        public ACDCMain()
        {
            List<Process> adProcess = Process.GetProcessesByName("ACDC").ToList();

            userName = "dpili";// Environment.UserName;

            //Check to see if there is alread an instance of ACDC running.
            if (adProcess.Count() > 1)
            {
                MessageBox.Show("There is already an instance of ACDC running", "ACDC already running", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                InitializeComponent();
                this.Show();
                _mode = GetMode();
                _dataAccess = new DataAccess(_mode);
                AuthenticateUser();
                try
                {
                    MakeOptionsVisibleAccordingToAccess();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace);
                }
            }
        }

        private Mode GetMode()
        {
            //TODO: Changed to use with DataAccessHelper
            string[] args = Environment.GetCommandLineArgs();
            switch (args.Length == 1 ? "LIVE" : args[1].ToUpper())
            {
                case "QA":
                    return Mode.QA;
                case "TEST":
                    return Mode.Test;
                case "DEV":
                    return Mode.Dev;
                case "LIVE":
                    return Mode.Live;
                default:
                    throw new Exception("Unknown Mode");
            }
        }

        public void AuthenticateUser()
        {
            string errorMessage = null;
            string caption = null;
            try
            {
                //Gets all the AD groups for the user and sets the _userRoles list
                GetActiveDirectoryGroups();
                //Gets all the available roles from the database
                List<string> roles = _dataAccess.GetRoles();
                //Gets a list of all the roles the user has assigned that are available in the database
                List<string> userRoles = roles.Intersect(_userRoles).ToList();
                if (userRoles.Count > 0)
                {
                    string CsysRole = _dataAccess.GetUserAssignedRole(userName);
                    if (userRoles.Contains(CsysRole))
                        role = CsysRole;
                    else
                        throw new Exception("Your Active Directory Role does not match the Role assigned to you in the database. Please contact System Support");
                }
                else
                {
                    throw new Exception("You do not have the correct Active Directory group for this program. Please contact System Support.");
                }
                //Set _sqlUserId from the database.
                _sqlUserId = _dataAccess.GetSqlUserId(userName);
            }
            catch (SqlException ex)
            {
                errorMessage = "It looks like you have not been set up to use ACDC. Please tell System Support that you are not recognized by the CSYS database.\r\n\r\n" + ex.Message;
                caption = "Not Recognized by CSYS";
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                caption = "Invalid Access";
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                WinForms.MessageBox.Show(errorMessage, caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Warning);
                Environment.Exit(0);
            }

        }//AuthenticateUser()

        private void GetActiveDirectoryGroups()
        {
            _userRoles = new List<string>();
            using (DirectoryEntry searchEntry = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local"))
            {
                DirectorySearcher searcher = new DirectorySearcher();
                searcher.SearchRoot = searchEntry;
                searcher.Filter = string.Format("SAMAccountName={0}", userName);
                SearchResult result = searcher.FindOne();
                if (result != null)
                {
                    ResultPropertyCollection attributes = result.Properties;
                    foreach (string prop in attributes["memberOf"])
                    {
                        int equalsIndex = prop.IndexOf("=", 1);
                        int commaIndex = prop.IndexOf(",", 1);
                        if (equalsIndex >= 0)
                        {
                            _userRoles.Add(prop.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                        }
                    }
                }
            }
        }

        private void CheckIfFolderExists(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        private bool CreateLocalCopyLibrary(string networkLocation, string localLocation, Applications app)
        {
            if (File.Exists(localLocation) == false || File.GetLastWriteTime(localLocation) != File.GetLastWriteTime(networkLocation))
            {
                try
                {
                    File.Copy(networkLocation, localLocation, true);
                    return true;
                }
                catch (FileNotFoundException)
                {
#if !DEBUG
                    string message = string.Format("You have been given access to {0} but it has not been promoted or the file is missing. Please contact SSHELP@utahsbr.edu for help", app.Application_Name);
                    WinForms.MessageBox.Show(message, "File Missing", WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
                    if (File.Exists(localLocation))
                        File.Delete(localLocation);
#endif
                    return false;
                }
                catch (IOException)
                {
                    string message = "An updated version of the application {0} was found but it can't be updated to your PC because the old application has already been loaded.  Do you want to continue using the old application?";
                    message += "  Click Yes to continue using the old application or click No to shut down the application. (Once the application is shut down you can start it up again and the new application can be loaded to your PC.)";
                    message = string.Format(message, app.Application_Name);
                    if (WinForms.MessageBox.Show(message, "New Application Found", WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Warning) != WinForms.DialogResult.Yes) { Environment.Exit(0); }
                    return false;
                }
            }//if
            return true;
        }//CreateLocalCopyLibrary()

        public void MakeOptionsVisibleAccordingToAccess()
        {
            //TODO: do the access check in the stored procedure, not in the code.
            IEnumerable<Applications> apps = _dataAccess.GetApplications()
                                             .Where(app => string.IsNullOrEmpty(app.Access_Key) || _dataAccess.UserHasAccess(role, app.Access_Key))
                                             .OrderBy(p => p.Application_Name);
            List<ApplicationButton> buttons = new List<ApplicationButton>();
            foreach (Applications app in apps)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    app.Files = new List<AssociatedFile>(_dataAccess.GetAssociatedFiles(app.Application_ID));
                    ApplicationButton button = new ApplicationButton(app, (int)_mode, _sqlUserId, role, _userRoles);
                    buttons.Add(button);
                    MainPanel.Children.Add(button);
                }));
            }
            SyncWidth();

            Thread loadThread = new Thread(() =>
            {

                foreach (ApplicationButton button in buttons)
                {
                    Applications app = button.Application;
                    app.Arguments = new List<Arguments>(_dataAccess.GetArguments(app.Application_ID));

                    bool invalidApp = false;
                    foreach (AssociatedFile file in app.Files)
                    {
                        //Set the source path, it needs to be set for all types
                        file.Source_Path = string.Format(file.Source_Path, _mode);
                        //Set the destination and copy files for everything other than a URL, no files needed
                        if (file.File_Type != "url")
                        {
                            file.Destination_Path = string.Format(file.Destination_Path, _mode);
                            CheckIfFolderExists(file.Destination_Path);
                            //if (file.Full_Destination_Path == button.ImagePath)
                            //    button.ReleaseBackgroundImage();
                            if (!CreateLocalCopyLibrary(file.Full_Source_Path, file.Full_Destination_Path, app))
                            {
                                Directory.Delete(file.Destination_Path, true);
                                invalidApp = true;
                                break;
                            }
                        }
                    }
                    if (invalidApp)
                    {
                        Dispatcher.Invoke(new Action(() => MainPanel.Children.Remove(button)));
                        SyncWidth();
                        foreach (AssociatedFile file in app.Files)
                            if (Directory.Exists(file.Destination_Path) && !file.Destination_Path.Contains("Images"))
                                Directory.Delete(file.Destination_Path, true);
                    }
                    else
                        Dispatcher.Invoke(new Action(() => button.Loading = false));
                }
            });
            loadThread.SetApartmentState(ApartmentState.STA);
            loadThread.Start();

        }//MakeOptionsVisibleAccordingToAccess()

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
            this.Close();
        }

    }//class
}//namespace
