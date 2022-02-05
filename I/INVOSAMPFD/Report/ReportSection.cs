using MinCap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace INVOSAMPFD
{
    class ReportSection
    {
        public ReportSection()
        {
            Items = new List<ReportItem>();
        }
        public string Header { get; set; }
        public List<ReportItem> Items { get; set; }

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        static IntPtr ReflectionWindowHandle = IntPtr.Zero;
        public ReportItem AddScreenshot(Reflection.Session session)
        {
            if (ReflectionWindowHandle == IntPtr.Zero)
            {
                string oldName = session.Caption;
                string newName = Guid.NewGuid().ToString();
                session.Caption = newName;
                ReflectionWindowHandle = FindWindowByCaption(IntPtr.Zero, newName);
                session.Caption = oldName;
            }
            var snap = WindowSnap.GetWindowSnap(ReflectionWindowHandle, false);
            if (snap == null)
                snap = WindowSnap.GetWindowSnap(ReflectionWindowHandle, true);
            var imageItem = new ReportImage() { Image = snap.Image };
            this.Items.Add(imageItem);
            return imageItem;
        }

        public ReportText AddDescription(string description)
        {
            var item = new ReportText() { Text = description };
            this.Items.Add(item);
            return item;
        }

        public ReportError AddError(string error)
        {
            var item = new ReportError() { Text = error };
            this.Items.Add(item);
            return item;
        }

        public ReportTable CreateTable(string header)
        {
            ReportTable rt = new ReportTable() { TableHeader = header };
            this.Items.Add(rt);
            return rt;
        }
    }
}
