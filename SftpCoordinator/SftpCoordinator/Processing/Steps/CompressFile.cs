using Ionic.Zip;
using System;
using System.IO;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    /// <summary>
    /// If applicable, compresses the given file (zip).
    /// </summary>
    class CompressFile : FileProcessStep
    {
        public override System.Linq.Expressions.Expression<Func<ActivityLog, bool?>> ActivityLogProperty
        {
            get { return (a) => a.CompressionSuccessful; }
        }

        public override FileOpResults Process(FileOpResults previous, ProjectFile projectFile, Project project, RunHistory rh)
        {
            if (!projectFile.CompressFile)
                return FileOpResults.NotApplicable(previous);
            try
            {
                Console.WriteLine($"Compression process called for file: {previous.FilePath}");
                string path = previous.FilePath + ".zip";
                string changedName = previous.ChangedFilename + ".zip";
                if (!string.IsNullOrEmpty(projectFile.AggregationFormatString))
                {
                    string name = Renamer.Rename(projectFile.AggregationFormatString, previous.ChangedFilename, rh.StartedOn) + ".zip";
                    string existing = GenericPath.Combine(projectFile.DestinationRoot, name);
                    changedName = name;
                    //change path from guid to formatted aggregation string
                    path = Path.Combine(new FileInfo(path).Directory.FullName, name);
                    previous.CleanupPaths.Add(path); //cleanup this file later
                    if (GenericFile.Exists(existing) && !GenericFile.Exists(path))
                        //aggregate zip already exists and we aren't currently using it, pull it down so we can add to it
                        GenericFile.Copy(existing, path);
                }

                using (ZipFile zf = new ZipFile(path))
                {
                    var entry = zf.AddFile(previous.FilePath, "/");
                    entry.FileName = GenericPath.UniqueName(previous.ChangedFilename, (s) => zf[s] != null); ;
                    zf.Save();
                    var repeaterResult = Repeater.TryRepeatedly(() => FS.Delete(previous.FilePath.UpdatePath()));
                    if(!repeaterResult.Successful)
                        return FileOpResults.Fail(previous,"Unable to cleanup after compression. {0}", repeaterResult.CaughtExceptions.Count > 0 ? repeaterResult.CaughtExceptions[0].ToString() : "No exceptions found") ;

                    var results = FileOpResults.Success(previous, path);
                    results.ChangedFilename = changedName;
                    return results;
                }
            }
            catch (Exception ex)
            {
                return FileOpResults.Fail(previous, "Unable to compress file. {0}", ex.ToString());
            }
        }
    }
}
