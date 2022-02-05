using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
    public class WorkspaceInfo
    {
        public string WorkspaceDirectory { get; set; }
        public string InProgressDirectory { get; set; }
        public string CompleteDirectory { get; set; }
        public bool ValidWorkspace
        {
            get
            {
                return Directory.Exists(InProgressDirectory) && Directory.Exists(CompleteDirectory) && Directory.GetDirectories(WorkspaceDirectory).Length == 2;
            }
        }
        public WorkspaceInfo(string workspaceDirectory)
        {
            WorkspaceDirectory = workspaceDirectory;
            try
            {
                InProgressDirectory = Path.Combine(workspaceDirectory, "In Progress");
                CompleteDirectory = Path.Combine(workspaceDirectory, "Complete");
            }
            catch (Exception)
            {
                //eat exception, workspace will show as invalid
            }
        }
    }
}
