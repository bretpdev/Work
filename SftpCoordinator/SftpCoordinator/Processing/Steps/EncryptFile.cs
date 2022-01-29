using System;
using System.IO;
using Uheaa.Common;

namespace SftpCoordinator
{
    /// <summary>
    /// Encrypts the given file (pgp).
    /// </summary>
    public class EncryptFile : FileProcessStep
    {
        public override System.Linq.Expressions.Expression<Func<ActivityLog, bool?>> ActivityLogProperty
        {
            get { return (a) => a.EncryptionSuccessful; }
        }

        public override FileOpResults Process(FileOpResults previous, ProjectFile projectFile, Project project, RunHistory rh)
        {
            if (!projectFile.EncryptFile)
                return FileOpResults.NotApplicable(previous);

            Console.WriteLine($"Encryption process called for file: {previous.FilePath}");
            if (!CryptographyHelper.KeysAvailable)
                return FileOpResults.Fail(previous, "Unable to access one or more public or private keys.");
            else if (!CryptographyHelper.GpgAvailable)
                return FileOpResults.Fail(previous, "Unable to access GPG encryption software.");
            else
            {
                try
                {
                    string archive = ArchiveHelper.ArchivePreEncryption(previous.FilePath, previous.ChangedFilename);
                    string tempPath = CryptographyHelper.Encrypt(previous.FilePath);
                    Repeater.TryRepeatedly(() => FS.Delete(previous.FilePath.UpdatePath()));
                    FileOpResults results = FileOpResults.Success(previous, tempPath);
                    results.ChangedFilename = GenericFile.AddExtension(previous.ChangedFilename, CryptographyHelper.Extension);
                    results.PreEncryptionArchiveLocation = archive;
                    return results;
                }
                catch (Exception ex)
                {
                    return FileOpResults.Fail(previous, "Unable to cleanup after encryption. {0}", ex.ToString());
                }
            }

        }
    }
}
