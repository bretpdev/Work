using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common;
using WinSCP;

namespace SftpCoordinator
{
    public static class GenericDirectory
    {
        public static bool Exists(string path)
        {
            if (FtpHelper.IsFtp(path))
            {
                using (Session session = new Session())
                {
                    session.Open(FtpHelper.GetOptions(path));
                    return session.FileExists(FtpHelper.StripHostInfo(path));
                }
            }
            else
                return Directory.Exists(path);
        }

        public static void CreateDirectory(string path)
        {
            if (FtpHelper.IsFtp(path))
            {
                using (Session session = new Session())
                {
                    session.Open(FtpHelper.GetOptions(path));
                    session.CreateDirectory(FtpHelper.StripHostInfo(path));
                }
            }
            else
                FS.CreateDirectory(path);
        }

        public static IEnumerable<string> GetFiles(string path, string searchPattern, string antiSearchPattern)
        {
            List<string> files = new List<string>();
            if (FtpHelper.IsFtp(path))
                using (Session session = new Session())
                {
                    session.Open(FtpHelper.GetOptions(path));
                    foreach (RemoteFileInfo file in session.ListDirectory(FtpHelper.StripHostInfo(path)).Files)
                        if (file.Name != "." && file.Name != ".." && SearchPattern.IsMatch(file.Name, searchPattern, antiSearchPattern))
                            files.Add(GenericPath.Combine(path, file.Name));
                }
            else
                foreach (string file in Directory.GetFiles(path))
                    if (SearchPattern.IsMatch(Path.GetFileName(file), searchPattern, antiSearchPattern))
                        files.Add(file);
            return files.Where(o => !o.EndsWith("Thumbs.db"));  //don't move any thumbs.db files
        }
    }
}
