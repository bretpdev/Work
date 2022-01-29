using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.IO;
using Uheaa.Common.DataAccess;

namespace ACDC
{
    public class Applications
    {
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public string AccessKey { get; set; }
        public string Application { get; set; }
        public string SourcePath { get; set; }
        public string StartingDll { get; set; }
        public string StartingClass { get; set; }
        public List<Arguments> Arguments { get; set; }
        public string DestinationPath
        {
            get
            {
                string path = string.Format("{0}{1}\\", EnterpriseFileSystem.GetPath("ACDC_Folder"),  ApplicationName);
                return (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Live || path.Replace(" ", "").ToLower().Contains("needhelp") || path.ToLower().Contains("subsystem")) ?
                    path : string.Format(@"{0}{1}\", path, DataAccessHelper.CurrentMode.ToString());
            }
        }

        public Applications()
        {
            Arguments = new List<Arguments>();
        }
    }

    public class Arguments
    {
        public ApplicationArgument ArgumentId { get; set; }
        public string Argument { get; set; }
        public string ArgumentDescription { get; set; }
        public int ArgumentOrder { get; set; }
    }

    public enum ApplicationArgument
    {
        Mode = 1,
        SqlUserID,
        Role,
        UserRoles,
        ModeString
    }
}
