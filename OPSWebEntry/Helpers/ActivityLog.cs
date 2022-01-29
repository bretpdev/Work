using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace OPSWebEntry
{
    public static class ActivityLog
    {
        public static ObservableCollection<LogItem> Log { get; internal set; }
        static ActivityLog()
        {
            TrayIcon.Icon = Properties.Resources.Visualpharm_Icons8_Metro_Style_City_Bank;
            TrayIcon.Visible = true;

            Clear();
        }
        public static void LogError(string error, string clickLocation = null)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Log.Insert(0, new LogItem(error, LogItem.ItemType.Error, clickLocation));
                if (OPSManager.Settings.TrayNotifications)
                    TrayIcon.ShowBalloonTip(5000, "OPS Web Entry", error, System.Windows.Forms.ToolTipIcon.Error);
            }));
        }
        public static void LogNotification(string notification, string clickLocation = null)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Log.Insert(0, new LogItem(notification, LogItem.ItemType.Notification, clickLocation));
            }));
        }

        public static void Clear()
        {
            Log = new ObservableCollection<LogItem>();
        }

        public static System.Windows.Forms.NotifyIcon TrayIcon = new System.Windows.Forms.NotifyIcon();

    }

    public class LogItem
    {
        public enum ItemType
        {
            Error, Notification
        }
        public string ClickLocation { get; internal set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public ItemType Type { get; set; }
        public string Display
        {
            get
            {
                return TimeStamp.ToShortTimeString() + "  " + Text;
            }
        }

        public LogItem(string text, ItemType type, string clickLocation)
        {
            Text = text;
            Type = type;
            TimeStamp = DateTime.Now;
            ClickLocation = clickLocation;
        }
    }

    public class LogItemForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((LogItem)value).Type == LogItem.ItemType.Error ? Brushes.Red : Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
