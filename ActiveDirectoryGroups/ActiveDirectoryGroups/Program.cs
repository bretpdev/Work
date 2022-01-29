using System;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace ActiveDirectoryGroups
{
    public class Program
	{
		[STAThread]
		public static void Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "ActiveDirectoryGroups") || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return;

            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun logRun = new ProcessLogRun("ActiveDirectoryGroups", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.Uheaa, DataAccessHelper.CurrentMode);

            frmActiveDirectoryGroups groups = new frmActiveDirectoryGroups(logRun);
            groups.ShowDialog();
		}
	}
}