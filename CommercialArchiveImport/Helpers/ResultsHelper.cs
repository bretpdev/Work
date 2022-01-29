using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommercialArchiveImport
{
    public static class ResultsHelper
    {
        public class Result
        {
            public enum ResultType
            {
                Error, Notification
            }
            public string Text { get; set; }
            public ResultType Type { get; set; }
        }
        public static void LogError(string formatError, params object[] parameters)
        {
            LogError(string.Format(formatError, parameters));
        }
        public static void LogError(string error)
        {
            if (results != null)
            {
                AddResult(new Result() { Text = error, Type = Result.ResultType.Error });
                ScrollToBottom();
            }
        }
        public static void Finished()
        {
            if (results.Items.Count == 0)
                LogNotification("No errors found.");
        }
        public static void LogNotification(string notification)
        {
            if (results != null)
            {
                AddResult(new Result() { Text = notification, Type = Result.ResultType.Notification });
                ScrollToBottom();
            }
        }

        public static void Clear()
        {
            if (results != null)
            {
                Invoke(results, () =>
                {
                    results.Items.Clear();
                });
            }
        }

        private static void ScrollToBottom()
        {
            if (results != null)
            {
                Invoke(results, () =>
                {
                    results.SelectedIndex = results.Items.Count - 1;
                    results.SelectedIndex = -1;
                });
            }
        }

        private static void AddResult(Result r)
        {
            Invoke(results, () =>
            {
                results.Items.Add(r);
            });
        }

        private static void Invoke(Control c, Action a)
        {
            c.Invoke(a);
        }

        private static ListBox results;
        public static void RegisterListBox(ListBox lb)
        {
            results = lb;
            lb.DrawMode = DrawMode.OwnerDrawFixed;
            lb.DrawItem += (o, e) =>
            {
                if (e.Index < 0)
                    return;
                var item = (ResultsHelper.Result)lb.Items[e.Index];
                e.DrawBackground();
                Brush b = null;
                if (item.Type == ResultsHelper.Result.ResultType.Notification)
                    b = Brushes.Black;
                else
                    b = Brushes.Red;
                e.Graphics.DrawString(item.Text, e.Font, b, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            };
            lb.Resize += (o, e) =>
            {
                lb.Refresh();
            };
        }
    }

}
