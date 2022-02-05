using System;
using System.Drawing;

namespace CentralizedPrintingProcess
{
    public interface IJobStatus
    {
        string TitleText { get; set; }
        Color TitleColor { get; set; }
        void LogItem(string text, params object[] values);
    }

    public class ConsoleJobStatus : IJobStatus
    {
        public ConsoleJobStatus(string title)
        {
            this.title = title;
        }
        public Color TitleColor
        {
            get { return Color.Transparent; }
            set { }
        }

        private string title;
        public string TitleText
        {
            get { return title; }
            set { title = value; }
        }

        public void LogItem(string text, params object[] values)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + title + ": " + string.Format(text, values));
        }
    }
}