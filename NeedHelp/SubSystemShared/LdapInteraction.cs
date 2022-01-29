using System;
using System.DirectoryServices;
using System.Security.Principal;

namespace SubSystemShared
{
    public class LdapInteraction
    {
        public string SID { get; set; }
        public string WindowsUserName { get; set; }
        const string DOMAIN = "UHEAA";

        /// <summary>
        /// Authenticates user against Active Directory.  Also, makes note of Security Identifier for use through the rest of the portal.
        /// </summary>
        public bool IsAuthenticated(string username, string password)
        {
            string domainAndUsername = DOMAIN + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(string.Empty, domainAndUsername, password);

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                //get security identifier
                NTAccount account = new NTAccount(domainAndUsername);
                SecurityIdentifier sid = (SecurityIdentifier)account.Translate(typeof(SecurityIdentifier));

                //move important values to accessible properties
                SID = sid.Value;
                WindowsUserName = username;
				return true;
			}
            catch (Exception)
            {
                return false;
            }
		}//IsAuthenticated()

        /// <summary>
        /// Syncs Database data with Active Directory if needed.
        /// </summary>
        public void SyncDatabaseFromActiveDirectory()
        {
            //TODO: Check For PortalDataAccess
            //if (PortalDataAccess.ActiveDirectorySyncNeeded())
            //{
            //    //update all user records to show as not current employees
            //    PortalDataAccess.ClearAllCurrentEmployeesAndEmailAddresses();

            //    //put together search criteria and properties to return
            //    DirectorySearcher search = new DirectorySearcher();
            //    search.SearchRoot = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local");
            //    search.Filter = "(objectCategory=user)";
            //    search.PropertiesToLoad.Add("cn");
            //    search.PropertiesToLoad.Add("mail");
            //    search.PropertiesToLoad.Add("objectSid");
            //    search.PropertiesToLoad.Add("SAMAccountName");
            //    SearchResultCollection results = search.FindAll();
            //    //try
            //    //{
            //    //     results = search.FindAll();
            //    //}
            //    //catch (Exception ex)
            //    ////{
            //    //    string path = @"T:\Exception.txt";
            //    //    using (StreamWriter sw = new StreamWriter(path))
            //    //    {
            //    //        sw.WriteLine("Exception Found");
            //    //    }
            //    //}

            //    int i = 0;

            //    foreach (SearchResult item in results)
            //    {
            //        i++;
            //        //get SID out of byte array
            //        string sid = (new SecurityIdentifier((byte[])item.Properties["objectSid"][0], 0)).Value;
            //        try
            //        {
            //            //add and/or update user record
            //            PortalDataAccess.UpdateUserRecord(sid, item.Properties["cn"][0] as string, item.Properties["mail"][0] as string);

            //            //used only at conversion time - PortalDataAccess.UpdateUserRecordForConversion(sid, item.Properties["cn"][0] as string, item.Properties["mail"][0] as string, item.Properties["SAMAccountName"][0] as string);
            //        }
            //        catch (ArgumentOutOfRangeException ex) //if one of the arguements isn't found then skip to user
            //        {
            //            //do nothing just don't croak
            //        }
            //    }

            //    //mark that sync has occured
            //    PortalDataAccess.UpdateActiveDirectorySyncTracker();
            //}
        }//SyncDatabaseWithActiveDirectory()

        /// <summary>
        /// Returns the associated SID for a given Windows user ID, or an empty string if one doesn't appear to exist.
        /// </summary>
        public static string GetActiveDirectorySid(string windowsUserID)
        {
            DirectorySearcher search = new DirectorySearcher();
            search.SearchRoot = new DirectoryEntry("LDAP://OU=USHE,DC=uheaa,DC=ushe,DC=local");
            search.Filter = string.Format("(SAMAccountName={0})", windowsUserID);
            //search.Filter = string.Format("(objectCategory=user)", windowsUserID);
            search.PropertiesToLoad.Add("objectSid");
            SearchResult result = search.FindOne();
			if (result == null)
			{
				return "";
			}
			else
            {
                return (new SecurityIdentifier((byte[])result.Properties["objectSid"][0], 0)).Value;
            }
		}//GetActiveDirectorySid()
    }//class
}//namespace
