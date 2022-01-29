using System;
using System.IO;
using System.Linq;

namespace SftpCoordinator
{
    public static class GenericPath
    {
        public static string Combine(string root, string path)
        {
            if (FtpHelper.IsFtp(root))
            {
                if (!root.EndsWith("/")) root += "/";
                if (path.StartsWith("/")) path = path.Substring(1);
                return Uri.UnescapeDataString(new Uri(new Uri(root), path).AbsoluteUri);
            }
            if (root == "\\") //UNC root
                return root + path;
            return Path.Combine(root, path.Trim('\\'));
        }

        public static string GetDirectoryName(string path)
        {
            if (FtpHelper.IsFtp(path))
            {
                Uri uri = new Uri(path);
                return uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length);
            }
            return Path.GetDirectoryName(path);
        }

        public static string UniqueName(string path)
        {
            return UniqueName(path, (s) => GenericFile.Exists(s));
        }
        public static string UniqueName(string name, Func<string, bool> check)
        {
            string destination = name;
            int tries = 0;
            while (check(destination))
                destination = name + ".u" + (tries++).ToString().PadLeft(4, '0');
            return destination;
        }
        public static string RemoveExtensions(string path)
        {
            string[] applicable = { "zip", "pgp" };
            foreach (string ext in applicable)
            {
                string dotExt = "." + ext;
                if (path.ToLower().EndsWith(dotExt))
                    return path.Substring(0, path.Length - dotExt.Length);
            }
            return path;
        }
        public static string GetExtension(string path)
        {
            int last = path.LastIndexOf('.');
            if (last == -1)
                return "";
            return path.Substring(last + 1);
        }
    }
}
