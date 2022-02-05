using System;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace TIMETRAKUP
{
    public class TimeTrackingUpdate
    {
        public TimeTrackingUpdate(int mode)
        {
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)mode;
            string msg = DatabaseAccessHelper.GenerateSprocAccessAlert(Assembly.GetExecutingAssembly());
            if (msg != null)
            {
                MessageBox.Show(msg);
                return;
            }

            UpdateTime time = new UpdateTime((DataAccessHelper.Mode)mode);
            Application.EnableVisualStyles();
            Application.Run(time);
        }
    }
}
