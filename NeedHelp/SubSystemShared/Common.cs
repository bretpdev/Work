using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace SubSystemShared
{
    public class Common
    {
        /// <summary>
        /// Checks the role assigned to the user in CSYS and compare it to their active directory group
        /// </summary>
        public static string AuthenticateUser(string userName, ProcessLogRun logRun)
        {
            string errorMessage = null;
            string caption = null;
            string role = null;
            try
            {
                //Gets all the AD groups for the user and sets the _userRoles list
                List<string> _userRoles = GetActiveDirectoryGroups(userName);
                //Gets all the available roles from the database
                List<string> roles = DataAccessBaseShared.Roles();
                //Gets a list of all the roles the user has assigned that are available in the database
                List<string> userRoles = roles.Intersect(_userRoles).ToList();
                if (userRoles.Count == 1)
                {
                    string CsysRole = DataAccessBaseShared.UserAssignedRole(userName);
                    if (userRoles.Contains(CsysRole))
                        role = CsysRole;
                    else
                    {
                        string message = string.Format("You have been set up by IT with the Active Directory Role: \r\n{0}\r\n\r\n You are set up in the database with the Role: \r\n{1}\r\n\r\n Please contact System Support.", userRoles.Single(), CsysRole);
                        throw new Exception(message);
                    }
                }
                else if (userRoles.Count > 1)
                {
                    string message = string.Format("You have been set up by IT with more than one Role. Please contact System Support to have this corrected.");
                    throw new Exception(message);
                }
                else
                    throw new Exception("You do not have the correct Active Directory group for this program. Please contact System Support.");
            }
            catch (SqlException ex)
            {
                errorMessage = "It looks like you have not been set up to use ACDC. Please tell System Support that you are not recognized by the CSYS database.\r\n\r\n" + ex.Message;
                caption = "Not Recognized by CSYS";
                logRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                caption = "Invalid Access";
                logRun.AddNotification(errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
            if (errorMessage.IsPopulated() || role.IsNullOrEmpty())
            {
                Dialog.Warning.Ok(errorMessage, caption);
                Environment.Exit(0);
            }
            return role;
        }

        /// <summary>
        /// Get list of active directory groups for the user logged in
        /// </summary>
        public static List<string> GetActiveDirectoryGroups(string userName)
        {
            List<string> userRoles = new List<string>();
            using (DirectoryEntry searchEntry = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local"))
            {
                DirectorySearcher searcher = new DirectorySearcher();
                searcher.SearchRoot = searchEntry;
                searcher.Filter = string.Format("SAMAccountName={0}", userName);
                SearchResult result = searcher.FindOne();
                if (result != null)
                {
                    ResultPropertyCollection attributes = result.Properties;
                    foreach (string prop in attributes["memberOf"])
                    {
                        int equalsIndex = prop.IndexOf("=", 1);
                        int commaIndex = prop.IndexOf(",", 1);
                        if (equalsIndex >= 0)
                        {
                            userRoles.Add(prop.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                        }
                    }
                }
            }
            return userRoles;
        }
    }
}
