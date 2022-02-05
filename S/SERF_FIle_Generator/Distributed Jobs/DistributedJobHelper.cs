using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERF_File_Generator
{
    public static class DistributedJobHelper
    {
        public static DistributedJobInfo InitializeJobScaffold(WorkspaceInfo workspaceInfo, string jobName)
        {
            var info = new DistributedJobInfo(workspaceInfo, jobName);
            info.CreateDirectories();
            return info;
        }

        /// <summary>
        /// Populate the scaffold with the given borrowers.  Returns the number of files generated.
        /// </summary>
        public static int PopulateJobScaffold(DistributedJobInfo info, string fileIdentifier, IEnumerable<string> borrowerSsns, int countPerFile = 100)
        {
            info.RegisterFileIdentifier(fileIdentifier);

            int batches = 0;
            IEnumerable<string> current = null;
            do
            {
                current = borrowerSsns.Skip(batches * countPerFile).Take(countPerFile);
                if (current.Any())
                {
                    string guid = Guid.NewGuid().ToString();
                    batches++;
                    string file = Path.Combine(info.QueueDirectory, fileIdentifier, guid);
                    File.WriteAllLines(file, current.ToArray());
                }
            } while (current.Any());
            return batches;
        }

        public static string FindAndWorkBatch(DistributedJobInfo job)
        {
            foreach (string identifier in job.FileIdentifiers)
            {
                string directory = Path.Combine(job.QueueDirectory, identifier);
                if (!Directory.Exists(directory))
                    return null;
                foreach (string file in Directory.GetFiles(directory))
                {
                    List<string> ssns = new List<string>();
                    try
                    {
                        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        using (StreamReader stream = new StreamReader(fs))
                        {
                            string line = null;
                            while ((line = stream.ReadLine()) != null)
                                ssns.Add(line.Trim());
                        }
                        File.Delete(file); //loaded all ssns, delete file
                    }
                    catch (IOException)
                    {
                        continue; //someone else got this file, go find another
                    }
                    WorkBatch(job, identifier, ssns);
                    return file;
                }
            }
            return null;
        }

        private static void WorkBatch(DistributedJobInfo job, string identifier, List<string> ssns)
        {
            string directory = @"C:\SERF_File\";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            directory = Path.Combine(directory, "Temp");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            string tempFile = Path.Combine(directory, Guid.NewGuid().ToString());
            ClassFileWriter writer = new ClassFileWriter(tempFile);
            Task.Factory.StartNew(writer.Process); //process async
            Parallel.ForEach(ssns, (ssn) =>
            {
                RecordGenerator.CreateFiles(ssn, identifier, writer);
            });
            writer.Stop();
            File.Move(tempFile, Path.Combine(job.CompleteDirectory, identifier, Guid.NewGuid().ToString()));
        }
    }

}
