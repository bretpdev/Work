using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;


namespace ACDC
{
    /// <summary>
    /// Interaction logic for ApplicationButton.xaml
    /// </summary>
    public partial class ApplicationButton : UserControl
    {
        private ProcessLogRun LogRun { get; set; }

        public ApplicationButton(Applications app, int mode, int sqlUserId, string role, List<string> userRoles, ProcessLogRun logRun)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.Application = app;
                AppButton.ToolTip = app.ApplicationName;
                this.Mode = mode;
                this.SqlUserId = sqlUserId;
                this.Role = role;
                this.UserRoles = userRoles;
                Loading = true;
                RefreshBackgroundImage();
            }
            LogRun = logRun;
        }

        public Applications Application { get; internal set; }
        protected int Mode { get; set; }
        protected int SqlUserId { get; set; }
        protected string Role { get; set; }
        protected IEnumerable<string> UserRoles { get; set; }
        protected long LastClick = DateTime.UtcNow.Ticks;

        public string ImagePath
        {
            get
            {
                string path = string.Format(DataAccess.GetImagePath("ACDC_Image"), DataAccessHelper.CurrentMode, Application.ApplicationName);
                if (File.Exists(path))
                    return path;
                else
                    return "";
            }
        }

        private static Random random = new Random();
        private void Refresh()
        {
            LoadingLabel.Visibility = Loading ? Visibility.Visible : Visibility.Hidden;
            BackgroundImage.Effect = Loading ? new BlurEffect() { Radius = 10.0 } : null;
            RefreshBackgroundImage();
        }

        public void RefreshBackgroundImage()
        {
            if (File.Exists(ImagePath))
            {
                AppButton.Background = Brushes.Transparent;
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(ImagePath);
                bitmap.EndInit();
                BackgroundImage.Source = bitmap;
            }
            else
            {
                ButtonGrid.Background = Brushes.Black;
            }
        }

        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            if (Loading)
                return; //button is loading, don't let them click

            List<object> args = new List<object>();
            foreach (var x in Application.Arguments.OrderBy(a => a.ArgumentOrder))
            {
                if (x.ArgumentId == ApplicationArgument.Mode)
                    args.Add(Mode);
                else if (x.ArgumentId == ApplicationArgument.SqlUserID)
                    args.Add(SqlUserId);
                else if (x.ArgumentId == ApplicationArgument.Role)
                    args.Add(Role);
                else if (x.ArgumentId == ApplicationArgument.UserRoles)
                    args.Add(UserRoles);
                else if (x.ArgumentId == ApplicationArgument.ModeString)
                    args.Add(((DataAccessHelper.Mode)Mode).ToString().ToLower());
            }
            string startingFile = "";
            if (!Application.StartingClass.IsNullOrEmpty() && !Application.StartingDll.IsNullOrEmpty())
                startingFile = Directory.GetFiles(Application.DestinationPath).Where(p => p.ToString().EndsWith(Application.StartingDll)).Single();
            else if (Application.StartingClass.IsNullOrEmpty() && !Application.StartingDll.IsNullOrEmpty())
                startingFile = Application.DestinationPath + Application.StartingDll;
            else if (Application.SourcePath.ToLower().Contains("http"))
                startingFile = Application.SourcePath;
            else
                startingFile = Application.DestinationPath + "\\" + Application.StartingDll;
            try
            {
                if (Application.StartingClass != null && startingFile.EndsWith(".dll"))
                {
                    //Creating a new domain so the process logger doesn't bubble up to ACDC when an exception is thrown
                    ObjectHandle libraryObject = Activator.CreateInstanceFrom(AppDomain.CreateDomain(Application.ApplicationName), startingFile,
                        Application.StartingDll.Replace(".dll", "") + "." + Application.StartingClass, true, BindingFlags.Default, null, args.ToArray(), null, (new Object[] { }), null);
                }
                else
                {
                    ProcessStartInfo start = new ProcessStartInfo(startingFile, args.Count > 0 ? " " + args[0] : null);
                    start.WorkingDirectory = System.IO.Path.GetDirectoryName(startingFile);
                    Process.Start(start);
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("There was an error launching {0}. Error: {1}", Application.ApplicationName, ex.Message);
                Dialog.Error.Ok(message, "Error launching application");
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
        }

        private void AppButton_DoubleClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        private bool loading;
        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                if (loading)
                {
                    Thread wait = new Thread(() =>
                    {
                        Thread.Sleep(1000);//only display loading message if loading for more than second
                        Dispatcher.Invoke(new Action(() => Refresh()));
                    });
                    wait.SetApartmentState(ApartmentState.STA);
                    wait.Start();
                }
                else
                    Refresh();
            }
        }
    }
}