using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace PMTCANCL
{
    public class UserValidator
    {
        private Dictionary<int, UserRole> AllowedRoles { get; set; }
        private DataAccess DA { get; set; }
        private List<UserRole> CurrentUserRoles { get; set; }
        private ProcessLogRun LogRun { get; set; }

        public UserValidator(DataAccess DA, ProcessLogRun logRun)
        {
            this.DA = DA;
            InitAllowedRoles();
            CurrentUserRoles = DA.GetUserID();
            LogRun = logRun;
        }

        public bool UserHasFedAccess()
        {
            if (AllowedRoles == null)
            {
                InitAllowedRoles();
            }

            if (CurrentUserRoles == null)
            {
                CurrentUserRoles = DA.GetUserID();
            }

            //User roles could still be null if the person running the program has no user ID in the system
            if (CurrentUserRoles != null)
            {
                foreach (UserRole role in CurrentUserRoles)
                {
                    if (AllowedRoles.ContainsKey(role.Role))
                    {
                        if(AllowedRoles[role.Role].FedAccess)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool UserHasUheaaAccess()
        {
            if (AllowedRoles == null)
            {
                InitAllowedRoles();
            }

            if (CurrentUserRoles == null)
            {
                CurrentUserRoles = DA.GetUserID();
            }

            //User roles could still be null if the person running the program has no user ID in the system
            if (CurrentUserRoles != null)
            {
                foreach (UserRole role in CurrentUserRoles)
                {
                    if (AllowedRoles.ContainsKey(role.Role))
                    {
                        if (AllowedRoles[role.Role].UheaaAccess)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }



        public bool ValidateUser()
        {
            if(AllowedRoles == null)
            {
                InitAllowedRoles();
            }

            if(CurrentUserRoles == null)
            {
                CurrentUserRoles = DA.GetUserID();
            }

            if(CurrentUserRoles != null)
            {
                foreach (UserRole role in CurrentUserRoles)
                {
                    if (AllowedRoles.ContainsKey(role.Role))
                    {
                        Console.WriteLine("Welcome " + AllowedRoles[role.Role].Description + ", " + Environment.UserName);
                        return true;
                    }
                }
            }
            LogRun.AddNotification("User does not have access to use the script. User: " + Environment.UserName, NotificationType.HandledException, NotificationSeverityType.Warning);
            MessageBox.Show("User does not have access to use the script. Exiting...", "Invalid Permissions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void InitAllowedRoles()
        {
            List<UserRole> allowedRoles = DA.GetRoles();
            AllowedRoles = new Dictionary<int, UserRole>();
            foreach(UserRole role in allowedRoles)
            {
                AllowedRoles.Add(role.Role, role);
            }
#if DEBUG
            AllowedRoles.Add(7, new UserRole(7, "Applications Development - Programmer", true, true));
            AllowedRoles.Add(8, new UserRole(8, "Applications Development - Manager", true, true));
#endif

        }
    }
}
