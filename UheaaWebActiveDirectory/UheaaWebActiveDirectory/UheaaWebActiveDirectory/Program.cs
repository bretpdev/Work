using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UheaaWebActiveDirectory
{
    static class Program
    {
        const int SUCCESS = 0;
        const int FAILURE = 1;
        static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "Uheaa - Active Directory Cache"))
                return FAILURE;
            var plr = new ProcessLogRun("UWACTIVDIR", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);
            var da = new DataAccess(plr.LDA);
            var ad = new ActiveDirectoryHelper();

            Console.WriteLine("Beginning Group Sync");
            da.SyncGroups();
            Console.WriteLine("Group Sync Complete");

            var thresholdDate = da.GetServerDate();
            Console.WriteLine("Beginning Active Directory Cache");
            Console.WriteLine($"Current Server Time: {thresholdDate}");

            var allGroups = da.GetActiveDirectoryRoles();
            var byActiveDirectoryGroupName = allGroups.GroupBy(o => o.ActiveDirectoryRoleName);
            Console.WriteLine($"Found {byActiveDirectoryGroupName.Count()} Groups to Cache");
            foreach (var activeDirectoryGroupName in byActiveDirectoryGroupName)
            {
                var groupMembers = ad.GetGroupMembers(activeDirectoryGroupName.Key);
                var matchingRoles = activeDirectoryGroupName.ToList();
                Console.WriteLine($"Group {activeDirectoryGroupName.Key}: Caching {groupMembers.Count} User(s)");
                foreach (var groupMember in groupMembers)
                    da.SetUserAndGroups(groupMember.AccountName, matchingRoles.Select(o => o.GroupId));
            }
            Console.WriteLine("Finished Group Caching.  Inactivating unused Users.");
            da.RemoveOldUsers(thresholdDate);

            Console.WriteLine("Active Directory Cache Complete");
            if (Debugger.IsAttached)
                Console.ReadKey();
            return SUCCESS;
        }
    }
}
