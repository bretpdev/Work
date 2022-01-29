using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;

namespace MassAssignBatch
{
    public class UserProcessing
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public UserProcessing()
        { }

        public int Process(ProcessLogRun LogRun, List<MassAssignRangeAssignment.SqlUser> users)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0); //0 will hide the console window.
            MassAssignRangeAssignment.RangeAssignment ra = new MassAssignRangeAssignment.RangeAssignment(users, LogRun);
            ra.ShowDialog();
            return 0;
        }
    }
}
