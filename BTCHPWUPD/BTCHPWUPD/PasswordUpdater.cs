using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace BTCHPWUPD
{
    class PasswordUpdater
    {
        private ReflectionInterface RI { get; set; }
        private DataAccess DA { get; set; }
        private BaseWordData PasswordData { get; set; }
        public PasswordUpdater(DataAccess da)
        {
            DA = da;
            PasswordData = DA.GetBaseWordAndFormat();
            RI = new ReflectionInterface();
        }

        public int Process()
        {
            List<UserIdData> userIds = DA.GetUserIds();
            List<string> usernamesNotUpdated = new List<string>();

            foreach (UserIdData userId in userIds)
            {
                RI.Hit(ReflectionInterface.Key.Clear);
                Thread.Sleep(1000);//Sleep to allow Session to catch up
                RI.Hit(ReflectionInterface.Key.Clear);
                Thread.Sleep(1000);//Sleep to allow Session to catch up
                RI.PutText(16, 12, "PHEAA", ReflectionInterface.Key.Enter);
                string newPassword = CalculateNewPassword(userId.Password);
                if(newPassword == userId.Password)
                {
                    RI.LogOut();
                    Thread.Sleep(1000);
                    continue;
                }

                RI.PutText(20, 18, userId.UserName);
                RI.PutText(20, 40, userId.Password);
                if (newPassword.IsNullOrEmpty())
                {
                    
                    RI.LogOut();
                    Thread.Sleep(1000);
                    continue;
                }
                RI.PutText(20, 65, newPassword, ReflectionInterface.Key.Enter);

                if (!RI.CheckForText(18, 11, "*** YOUR PASSWORD HAS BEEN UPDATED."))
                {
                    usernamesNotUpdated.Add(userId.UserName);
                    Program.PLR.AddNotification(string.Format("The User ID:{0} was not updated.  Session Message:{1}", userId.UserName, RI.Message), Uheaa.Common.ProcessLogger.NotificationType.ErrorReport, Uheaa.Common.ProcessLogger.NotificationSeverityType.Critical);
                }
                else
                    DA.UpdatePassword(userId.UserName, newPassword);

                RI.LogOut();
                Thread.Sleep(1000);
            }
            if (usernamesNotUpdated.Any())
            {
                string body = string.Format("The following users were not updated.  Please review. {0}{1}", Environment.NewLine, string.Join(Environment.NewLine, usernamesNotUpdated));
                Program.PLR.AddNotification(body, Uheaa.Common.ProcessLogger.NotificationType.ErrorReport, Uheaa.Common.ProcessLogger.NotificationSeverityType.Critical);
                EmailHelper.SendMail(DataAccessHelper.TestMode, "SSHELP@utahsbr.edu", "BTCHPWUPD@utahsbr.edu", "Passwords not updated.", body, "", EmailHelper.EmailImportance.High, true);
            }

            RI.CloseSession();

            return 0;
        }

        private string CalculateNewPassword(string currentPassword)
        {
            List<string> format = CreateFormat();
            string newPassword = string.Empty;
            foreach (string item in format)
            {
                if (item == "B1")
                    newPassword += PasswordData.BaseWord.Substring(0, 1);
                else if (item == "B2")
                    newPassword += PasswordData.BaseWord.Substring(1, 1);
                else if (item == "B3")
                    newPassword += PasswordData.BaseWord.Substring(2, 1);
                else if (item == "B4")
                    newPassword += PasswordData.BaseWord.Substring(3, 1);
                else if (item == "M1")
                    newPassword += DateTime.Now.Month.ToString().PadLeft(2, '0').Substring(0, 1);
                else if (item == "M2")
                    newPassword += DateTime.Now.Month.ToString().PadLeft(2, '0').Substring(1, 1);
                else if (item == "Y1")
                    newPassword += DateTime.Now.Year.ToString().Substring(2, 1);
                else if (item == "Y2")
                    newPassword += DateTime.Now.Year.ToString().Substring(3, 1);
                else
                    throw new Exception(string.Format("An unexpected format of {0} was found.  Please review.", item));
            }
            if (newPassword == currentPassword)
                return null;
            else
                return newPassword;
        }

        private List<string> CreateFormat()
        {
            List<string> format = new List<string>();
            format.Add(PasswordData.Format.Substring(0, 2));
            format.Add(PasswordData.Format.Substring(2, 2));
            format.Add(PasswordData.Format.Substring(4, 2));
            format.Add(PasswordData.Format.Substring(6, 2));
            format.Add(PasswordData.Format.Substring(8, 2));
            format.Add(PasswordData.Format.Substring(10, 2));
            format.Add(PasswordData.Format.Substring(12, 2));
            format.Add(PasswordData.Format.Substring(14, 2));

            return format;
        }
    }
}
