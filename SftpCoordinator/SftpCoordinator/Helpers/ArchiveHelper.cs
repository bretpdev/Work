using System;
using System.Collections.Generic;
using System.IO;
using Uheaa.Common;

namespace SftpCoordinator
{
    public static class ArchiveHelper
    {
        /// <summary>
        /// Archives the given file and returns the archived path.  Returns null if archiving not available.
        /// </summary>
        public static string ArchivePreEncryption(string filePath, string newFilename)
        {
            return Archive(filePath, newFilename, Settings.UheaaPreEncryptionArchiveLocation);
        }
        /// <summary>
        /// Archives the given file and returns the archived path.  Returns null if archiving not available.
        /// </summary>
        public static string ArchivePreDecryption(string filePath, string newFilename)
        {
            return Archive(filePath, newFilename, Settings.UheaaPreDecryptionArchiveLocation);
        }

        private static string Archive(string filePath, string newFilename, string destRoot)
        {
            if (Directory.Exists(destRoot))
            {
                string destination = Path.Combine(destRoot, newFilename);
                destination = GenericPath.UniqueName(destination);
                FS.Copy(filePath, destination.UpdatePath());
                File.SetLastWriteTime(destination, DateTime.Now);
                return destination;
            }
            return null;
        }

        public static void CleanupArchives()
        {
            RemoveAfterDays(Settings.UheaaPreEncryptionArchiveLocation, Settings.UheaaPreEncryptionRetentionDays);
            RemoveAfterDays(Settings.UheaaPreDecryptionArchiveLocation, Settings.UheaaPreDecryptionRetentionDays);
        }

        private static void RemoveAfterDays(string directory, int days)
        {
            if (!Directory.Exists(directory)) return;
            RemoveAfterDays(Directory.GetFiles(directory), days);
        }

        private static void RemoveAfterDays(IEnumerable<string> files, int days)
        {
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if ((DateTime.Now - fi.LastWriteTime).TotalDays >= days)
                {
                    try
                    {
                        Repeater.TryRepeatedly(() => FS.Delete(file.UpdatePath()));
                    }
                    catch (Exception)
                    {
                        //don't worry about not being able to delete.
                    }
                }
            }
        }
    }
}
