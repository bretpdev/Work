using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace Uheaa.Common.Baa
{
    public class BaaReflectionInterface : ReflectionInterface
    {
        public ReflectionLog Log { get; private set; }
        ReflectionLogSettings settings;
        IntPtr hwnd;
        private string scriptId;
        public BaaReflectionInterface(ReflectionInterface ri,  string scriptId) : base(ri.ReflectionSession)
        {
            this.scriptId = scriptId;
            this.Log = new ReflectionLog();
            settings = new ReflectionLogSettings();
            if (DataAccessHelper.TestMode) //don't run in LIVE
            {
                var uiThread = new Thread(() =>
                {
                    Application.EnableVisualStyles();
                    Application.Run(new UI(Log, scriptId, settings, this));
                });
                uiThread.SetApartmentState(ApartmentState.STA);
                uiThread.Start();
                var originalTitle = ReflectionSession.Caption;
                ReflectionSession.Caption = Guid.NewGuid().ToString();
                hwnd = ImageHelper.GetHwndFromTitle(ReflectionSession.Caption);
                ReflectionSession.Caption = originalTitle;
            }
        }
        public override bool CheckForText(int row, int column, params string[] text)
        {
            bool result = base.CheckForText(row, column, text);
            Log.LogText("Checked for text {0} at {1}, {2} and {3}", string.Join(" OR ", text), row, column, result ? "found it." : "didn't find it.");
            return result;
        }
        public override string GetText(int row, int col, int length)
        {
            string text = base.GetText(row, col, length);
            Log.LogText("Got text at {0}, {1} of length {2} and retrieved: {3}", row, col, length, text);
            return text;
        }
        public override void CloseSession()
        {
            try
            {
                base.CloseSession();
            }
            catch
            {
                //eat com exception
            }
            finally //session may already be closed
            {
                Log.LogText("Session Closed.");

                if (settings.AutoSaveToTDrive)
                    WriteToDisk();
            }
        }

        public void WriteToDisk()
        {
            string logdir = $"T:\\BaaLogs\\{scriptId}\\";
            if (!Directory.Exists(logdir))
                Directory.CreateDirectory(logdir);
            string filename = Path.Combine(logdir, Guid.NewGuid().ToString().Replace("-", "")) + ".html";
            File.WriteAllText(filename, Log.GetHtmlText());
        }
        public override void FastPath(string input)
        {
            Log.LogText("Fast Path to {0}", input);
            base.FastPath(input);
        }
        public override void PutText(int row, int column, string text, Key keyToHit, bool blankFieldFirst)
        {
            base.PutText(row, column, text, keyToHit, blankFieldFirst);
            Log.LogText("Entered {0} at {1}, {2}.{3}", text, row, column, blankFieldFirst ? " Also blanked out the field." : "");
            if (settings.ScreenshotAfterPutText)
                LogScreenshot();
        }
        public override bool Hit(ReflectionInterface.Key keyToHit)
        {
            var result =  base.Hit(keyToHit);
            Log.LogText("Hit key {0}.", keyToHit.ToString());
            if (keyToHit == Key.Enter && settings.ScreenshotAfterEnter)
                LogScreenshot();
            return result;
        }
        private void LogScreenshot()
        {
            var image = ImageHelper.GetImageFromHwnd(hwnd);
            Log.LogScreenshot(image);
        }
    }
}
