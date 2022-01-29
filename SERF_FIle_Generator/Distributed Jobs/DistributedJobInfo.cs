using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
    public class DistributedJobInfo
    {
        public WorkspaceInfo Workspace { get; set; }
        public string JobDirectory { get; set; }
        public string QueueDirectory { get; set; }
        public string CompleteDirectory { get; set; }

        public List<string> FileIdentifiers { get; set; }

        public DistributedJobInfo(WorkspaceInfo workspace, string jobName)
        {
            Workspace = workspace;
            JobDirectory = Path.Combine(workspace.InProgressDirectory, jobName);
            FileIdentifiers = new List<string>();
            QueueDirectory = Path.Combine(JobDirectory, "Queue");
            CompleteDirectory = Path.Combine(JobDirectory, "Complete");
            if (Directory.Exists(QueueDirectory))
                foreach (string identifier in Directory.GetDirectories(QueueDirectory))
                {
                    string name = Path.GetFileName(identifier);
                    FileIdentifiers.Add(name);
                }
        }

        public void CreateDirectories()
        {
            Directory.CreateDirectory(JobDirectory);
            Directory.CreateDirectory(QueueDirectory);
            Directory.CreateDirectory(CompleteDirectory);
        }

        public void RegisterFileIdentifier(string identifier)
        {
            FileIdentifiers.Add(identifier);
            Directory.CreateDirectory(Path.Combine(QueueDirectory, identifier));
            Directory.CreateDirectory(Path.Combine(CompleteDirectory, identifier));
        }

        public void Complete()
        {
            foreach (string identifier in FileIdentifiers)
            {
                string sourceDirectory = Path.Combine(CompleteDirectory, identifier);
                string destDirectory = Path.Combine(Workspace.CompleteDirectory, Path.GetFileName(JobDirectory), identifier);
                Directory.CreateDirectory(destDirectory);
                foreach (string file in Directory.GetFiles(sourceDirectory))
                    File.Move(file, Path.Combine(destDirectory, Path.GetFileName(file)));
                Directory.Delete(sourceDirectory);
                Directory.Delete(Path.Combine(QueueDirectory, identifier));
            }
            Directory.Delete(CompleteDirectory);
            Directory.Delete(QueueDirectory);
            Directory.Delete(JobDirectory);
        }
    }

}
