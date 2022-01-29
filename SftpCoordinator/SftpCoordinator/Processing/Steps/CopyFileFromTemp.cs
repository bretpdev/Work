using System;
using Uheaa.Common;

namespace SftpCoordinator
{
    /// <summary>
    /// Copies the file to a temporary location for additional processing
    /// </summary>
    class CopyFileFromTemp : FileProcessStep
    {
        public override System.Linq.Expressions.Expression<Func<ActivityLog, bool?>> ActivityLogProperty
        {
            get { return (a) => a.CopySuccessful; }
        }

        public override FileOpResults Process(FileOpResults previous, ProjectFile projectFile, Project project, RunHistory rh)
        {
            Console.WriteLine($"Copy Files from temp called on file: {previous.FilePath}");
            string name = previous.ChangedFilename;
            if (!projectFile.RenameFormatString.IsNullOrEmpty())
            {
                name = Renamer.Rename(projectFile.RenameFormatString, previous.ChangedFilename, rh.StartedOn);
                if (projectFile.CompressFile && !name.ToLower().EndsWith(".zip")) //add zip extension if it was lost
                    name += ".zip";
            }
            string destinationPath = GenericPath.Combine(projectFile.CalculatedDestinationRoot, name);
            Console.WriteLine($"File to be moved from: {previous.FilePath} to {destinationPath}");

            if (Settings.AutoResolveNamingConflicts)
            {
                if (string.IsNullOrEmpty(projectFile.AggregationFormatString))
                    destinationPath = GenericPath.UniqueName(destinationPath);

                Console.WriteLine($"AutoResolveNamingConflicts changed destination to: {destinationPath} for file: {previous.FilePath}");
            }
            else if (GenericFile.Exists(destinationPath)) //destination file exists
            {
                if (string.IsNullOrEmpty(projectFile.AggregationFormatString)) //we aren't aggregating, so finding the same name is bad
                    return FileOpResults.Fail(previous, "Destination file already exists: {0}", destinationPath);
            }

            var folder = GenericPath.GetDirectoryName(destinationPath);

            if (!GenericDirectory.Exists(folder))
            {
                Console.WriteLine($"Directory didnt exist.  Creating directory...: {folder}");

                try
                {
                    GenericDirectory.CreateDirectory(folder);
                }
                catch (Exception ex)
                {
                    return FileOpResults.Fail(previous, "Couldn't create needed directory: {0}\r\n{1}", folder, ex.ToString());
                }
            }
            try
            {
                Console.WriteLine($"File Copying from: {previous.FilePath} to: {destinationPath}");
                if (string.IsNullOrEmpty(projectFile.AggregationFormatString))
                    GenericFile.Copy(previous.FilePath, destinationPath);
                else
                    GenericFile.Copy(previous.FilePath, destinationPath, true);
                previous.CleanupPaths.Add(previous.FilePath);

                return FileOpResults.Success(previous, destinationPath);
            }
            catch (Exception ex)
            {
                return FileOpResults.Fail(previous, "Error copying file {0} to {1}\r\n{2}", previous.FilePath, destinationPath, ex.ToString());
            }
        }
    }
}
