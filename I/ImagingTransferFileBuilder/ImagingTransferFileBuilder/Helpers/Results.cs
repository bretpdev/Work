using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImagingTransferFileBuilder
{
    public static class Results
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
        public static void LogError(string formatError, params string[] parameters)
        {
            LogError(string.Format(formatError, parameters));
        }
        public static void LogError(string error)
        {
            if (results != null)
            {
                results.Items.Add(new Result() { Text = error, Type = Result.ResultType.Error });
                copyButton.Enabled = true;
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
                results.Items.Add(new Result() { Text = notification, Type = Result.ResultType.Notification});
                copyButton.Enabled = true;
                ScrollToBottom();
            }
        }

        public static void Clear()
        {
            if (results != null)
            {
                results.Items.Clear();
                results.Refresh();
                copyButton.Enabled = false;
            }
        }

        private static void ScrollToBottom()
        {
            if (results != null)
            {
                results.SelectedIndex = results.Items.Count - 1;
                results.SelectedIndex = -1;
            }
        }
        private static ListBox results;
        public static void RegisterListBox(ListBox lb)
        {
            results = lb;
        }
        private static Button copyButton;
        public static void RegisterCopyButton(Button b)
        {
            copyButton = b;
        }

        public static string ToString()
        {
            string output = "";
            foreach (Result r in results.Items)
                output += r.Text + Environment.NewLine;
            return output.Trim() + "";
        }
    }
}
