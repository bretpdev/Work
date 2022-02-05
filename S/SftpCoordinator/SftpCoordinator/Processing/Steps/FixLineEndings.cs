using System;
using System.IO;
using System.Linq.Expressions;
using Uheaa.Common;

namespace SftpCoordinator
{
    /// <summary>
    /// Replaces UNIX line endings with Windows line endings.
    /// </summary>
    public class FixLineEndings : FileProcessStep
    {
        public override Expression<Func<ActivityLog, bool?>> ActivityLogProperty
        {
            get { return (a) => a.FixLineEndingsSuccessful; }
        }

        public override FileOpResults Process(FileOpResults previous, ProjectFile projectFile, Project project, RunHistory rh)
        {
            if (!projectFile.FixLineEndings)
                return FileOpResults.NotApplicable(previous);

            Console.WriteLine($"Fix Line Endings called for file: {previous.FilePath}");
            Guid guid = Guid.NewGuid();
            string fixedTemp = Path.Combine(Settings.TempFolderLocation, guid.ToString());

            using (StreamR sr = new StreamR(previous.FilePath))
            {
                using (StreamWriter sw = new StreamWriter(fixedTemp))
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        sw.Write(line); //don't include newline initially
                        int pos = line.Length;
                        while ((line = sr.ReadLine()) != null)
                        {
                            sw.Write(Environment.NewLine); pos += Environment.NewLine.Length;
                            sw.Write(line); pos += line.Length;
                        }
                        if (pos != sr.BaseStream.Position) //streamreader likes to skip a trailing newline
                            sw.Write(Environment.NewLine); //add final newline if applicable
                    }
                }
            }
            Console.WriteLine($"Line endings fixed for file: {previous.FilePath}.  New file named: {fixedTemp}");
            Console.WriteLine($"Deleting bad line endings file: {previous.FilePath}");
            try
            {
                Repeater.TryRepeatedly(() => FS.Delete(previous.FilePath.UpdatePath()));
                previous.CleanupPaths.Add(fixedTemp);
            }
            catch (IOException)
            {
                return FileOpResults.Fail(previous, "Error deleting temp file {0}", previous.FilePath);
            }
            return FileOpResults.Success(previous, fixedTemp);
        }
    }
}
