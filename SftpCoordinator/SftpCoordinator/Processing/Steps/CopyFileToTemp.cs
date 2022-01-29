using System;
using System.IO;
using System.Linq.Expressions;

namespace SftpCoordinator
{
    /// <summary>
    /// After processing is complete, copies the file from temp to its final destination.
    /// </summary>
    public class CopyFileToTemp : FileProcessStep
    {
        public override Expression<Func<ActivityLog, bool?>> ActivityLogProperty
        {
            get { return (a) => a.CopySuccessful; }
        }
        public override FileOpResults Process(FileOpResults previous, ProjectFile projectFile, Project project, RunHistory rh)
        {
            Console.WriteLine($"Copy file to temp called on source file: {previous.FilePath}");
            if (!Directory.Exists(Settings.TempFolderLocation))
            {
                try
                {
                    Directory.CreateDirectory(Settings.TempFolderLocation);
                }
                catch (Exception ex)
                {
                    return FileOpResults.Fail(previous, "Couldn't create temp directory. {0}", ex.ToString());
                }
            }
            if (!GenericFile.Exists(previous.FilePath))
            {
                return FileOpResults.Fail(previous, "Could not find source file: {0}" + previous.FilePath);
            }

            Guid guid = Guid.NewGuid();
            string tempPath = Path.Combine(Settings.TempFolderLocation, guid.ToString());
            if (previous.FilePath.EndsWith(CryptographyHelper.Extension))
                tempPath += CryptographyHelper.Extension;

            try
            {
                GenericFile.Copy(previous.FilePath, tempPath);
                previous.CleanupPaths.Add(tempPath);
            }
            catch (Exception e)
            {
                return FileOpResults.Fail(previous, "Couldn't copy file to temp folder.  {0}", e.ToString());
            }

            Console.WriteLine($"Successfully copied file to temp: {previous.FilePath}");
            return FileOpResults.Success(previous, tempPath);
        }
    }
}
