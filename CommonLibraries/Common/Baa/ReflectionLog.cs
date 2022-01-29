using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.Baa
{
    public class ReflectionLog
    {
        public ReflectionLog()
        {
            Events = new List<LogEvent>();
        }
        public delegate void EventLoggedDelegate(LogEvent loggedEvent);
        public event EventLoggedDelegate EventLogged;
        public Func<string> GetHtmlText { get; set; }
        public List<LogEvent> Events { get; set; }

        public void LogText(string text, params object[] args)
        {
            var textEvent = new TextLogEvent(string.Format(text, args));
            Events.Add(textEvent);
            EventLogged?.Invoke(textEvent);
        }
        public void LogScreenshot(Image image)
        {
            var imageEvent = new ImageLogEvent(image);
            Events.Add(imageEvent);
            EventLogged?.Invoke(imageEvent);
        }

        public class LogEvent
        {
            public DateTime Timestamp { get; private set; }
            public LogEvent()
            {
                Timestamp = DateTime.Now;
            }
        }
        public class TextLogEvent : LogEvent
        {
            public string Text { get; private set; }
            public TextLogEvent(string text) : base()
            {
                this.Text = text;
            }
        }
        public class ImageLogEvent : LogEvent
        {
            public Image Image { get; private set; }
            public ImageLogEvent(Image image)
                : base()
            {
                this.Image = image;
            }
        }
    }
}
