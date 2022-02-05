using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;

namespace Uheaa.Common
{
    /// <summary>
    /// Wrappers around the System.IO.File and System.IO.Directory functionality.
    /// Mitigated in VeraCode.
    /// </summary>
    public static class FS
    {
        /// <summary>
        /// Deletes a file or folder.
        /// </summary>
        /// <param name="recursive">If path is a Directory and recursive is True, deletes all subcontents of the directory.</param>
        public static void Delete(string path, bool recursive = true)
        {
            ValidatePath(path);
            if (File.Exists(path))
                File.Delete(path);
            else if (Directory.Exists(path))
                Directory.Delete(path, recursive);
        }

        public static void WriteAllText(string path, string contents)
        {
            ValidatePath(path);
            File.WriteAllText(path, contents);
        }

        public static void WriteAllLines(string path, string[] contents)
        {
            ValidatePath(path);
            File.WriteAllLines(path, contents);
        }

        public static string ReadAllText(string path)
        {
            ValidatePath(path);
            return File.ReadAllText(path);
        }
        public static string[] ReadAllLines(string path)
        {
            ValidatePath(path);
            return File.ReadAllLines(path);
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite = false)
        {
            ValidatePath(sourceFileName);
            ValidatePath(destFileName);
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        public static void CreateDirectory(string path)
        {
            ValidatePath(path);
            Directory.CreateDirectory(path);
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            ValidatePath(sourceFileName);
            ValidatePath(destFileName);
            File.Move(sourceFileName, destFileName);
        }

        public static FileStream Create(string path)
        {
            ValidatePath(path);
            return File.Create(path);
        }

        public static FileStream OpenRead(string path)
        {
            ValidatePath(path);
            return File.OpenRead(path);
        }

        public static FileStream OpenWrite(string path)
        {
            ValidatePath(path);
            return File.OpenWrite(path);
        }

        static readonly string[] AllowedRoots = new string[] { "H:", "M:", "O:", "T:", "Q:", "X:", "Y:", "Z:", @"\\imgprodkofax\", @"\\imgdevkofax\","C:\\Enterprise Program Files", "C:\\inetpub", @"\\UHEAA-FS\DomainUsersData", @"\\FSUHEAAXYZ\SEAS", @"\\FSUHEAAXYZ\SEASCS", @"\\FSUHEAAXYZ\DEVSEASCS", @"\\FSUHEAAQ\Restricted" };

        public static void ValidatePath(string path)
        {
            if (!AllowedRoots.Any(o => path.StartsWith(o)))
                throw new SecurityException($"Path {path} has a disallowed file root.");
            if (path.Contains(".."))
                throw new SecurityException($"Path {path} contains illegal directory traversal.");
            foreach (var illegal in Path.GetInvalidPathChars())
                if (path.Contains(illegal))
                    throw new SecurityException($"Path {path} contains illegal character {illegal}");
        }
    }
}