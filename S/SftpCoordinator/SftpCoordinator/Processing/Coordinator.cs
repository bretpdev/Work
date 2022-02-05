using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;

namespace SftpCoordinator
{
    public class Coordinator
    {
        RunHistory rh;
        ProcessLogRun plr;
        public RunHistory Run(ProcessLogRun PLR)
        {
            Console.WriteLine("Starting up...");
            plr = PLR;
            rh = RunHistory.CreateNewRun();
            foreach (Project p in Project.GetAllProjects())
                ProcessProject(p);
            ArchiveHelper.CleanupArchives();
            RunHistory.FinishRun(rh);
            ErrorReport.GenerateErrorReport(rh, plr);
            return rh;
        }

        protected void ProcessProject(Project p)
        {
            Console.WriteLine($"Processing project: {p.Name}");
            List<FileJobInfo> jobs = new List<FileJobInfo>();
            foreach (ProjectFile pf in ProjectFile.GetProjectFilesByProject(p))
                jobs.AddRange(ProcessFiles(p, pf));
            foreach (FileJobInfo job in jobs)
            {
                Console.WriteLine($"Calling cleanup on ActivityLogId: {job.ActivityLogInfo.ActivityLogId}");
                job.Cleanup();
            }
        }

        protected List<FileJobInfo> ProcessFiles(Project p, ProjectFile pf)
        {
            Console.WriteLine($"Processing project file id: {pf.ProjectFileId} from project: {p.Name}");
            List<FileJobInfo> jobs = new List<FileJobInfo>();
            if (!GenericDirectory.Exists(pf.CalculatedSourceRoot))
            {
                string message = $"Unable to find source root for ProjectFileId: {pf.ProjectFileId} from project: {p.Name}";
                Console.WriteLine(message);
                plr.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                return new List<FileJobInfo>();
            }
            var files = GenericDirectory.GetFiles(pf.CalculatedSourceRoot, pf.SearchPattern, pf.AntiSearchPattern);
            Parallel.ForEach(files, file => 
            {
                Console.WriteLine($"Checking file for changes: {file}");
                int sleepTime = 20 * 1000; //20 seconds
                long lastSize = new FileInfo(file).Length;
                Thread.Sleep(sleepTime);
                long curSize = new FileInfo(file).Length;
                while (curSize != lastSize)
                {
                    string message = $"File {file} increased in size from {lastSize}b to {curSize}b.";
                    Console.WriteLine(message);
                    plr.AddNotification(message, NotificationType.EndOfJob, NotificationSeverityType.Warning);
                    Thread.Sleep(sleepTime);
                    lastSize = curSize;
                    curSize = new FileInfo(file).Length;
                }
                Console.WriteLine($"Found file ready for processing: {file}");
                FileJobInfo job = new FileJobInfo(p, pf, rh, file, plr);
                Console.WriteLine($"Calling file processing for ActivityLogId: {job.ActivityLogInfo.ActivityLogId}");
                if (job.Process())
                {
                    Console.WriteLine($"Processing successful.  Adding job to list of jobs to run cleanup on.  ActivityLogId: {job.ActivityLogInfo.ActivityLogId}");
                    jobs.Add(job);
                }
            });
            return jobs;
        }
    }
}
