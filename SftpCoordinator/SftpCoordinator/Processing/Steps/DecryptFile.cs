using System;

namespace SftpCoordinator
{
    /// <summary>
    /// Decrypts the given file (pgp).
    /// </summary>
    public class DecryptFile : FileProcessStep
    {
        public override System.Linq.Expressions.Expression<Func<ActivityLog, bool?>> ActivityLogProperty
        {
            get { return (a) => a.DecryptionSuccessful; }
        }

        public override FileOpResults Process(FileOpResults previous, ProjectFile projectFile, Project project, RunHistory rh)
        {
            if (!projectFile.DecryptFile)
                return FileOpResults.NotApplicable(previous);

            Console.WriteLine($"Decrpyt processing called on: {previous.FilePath}");
            if (!CryptographyHelper.KeysAvailable)
                return FileOpResults.Fail(previous, "Unable to access one or more public or private keys.");
            else if (!CryptographyHelper.GpgAvailable)
                return FileOpResults.Fail(previous, "Unable to access GPG decryption software.");
            else if (!previous.FilePath.EndsWith(CryptographyHelper.Extension))
                return FileOpResults.Fail(previous, "File was marked for decryption but did not end in {0}. {1}", CryptographyHelper.Extension, previous.FilePath);
            else
            {
                try
                {
                    string archiveLocation = ArchiveHelper.ArchivePreDecryption(previous.FilePath, previous.ChangedFilename);
                    string tempPath = CryptographyHelper.Decrypt(previous.FilePath);
                    var results = FileOpResults.Success(previous, tempPath);
                    results.PreDecryptionArchiveLocation = archiveLocation;
                    results.ChangedFilename = GenericPath.RemoveExtensions(results.ChangedFilename);
                    return FileOpResults.Success(previous, tempPath); 
                }
                catch (Exception ex)
                {
                    return FileOpResults.Fail(previous, "Unable to cleanup after decryption. {0}", ex.ToString());
                }
            }
        }
    }
}
