using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common
{
    public static class FileSystemHelper
    {
        /// <summary>
        /// Checks if the specific directory exists and alerts the user if it does not.  Useful for UI dialogs.
        /// </summary>
        /// <param name="directory">The directory to check.</param>
        /// <param name="secure">Whether the location must be a secure CornerStone location.</param>
        /// <param name="notFoundFormattedMessage">The message to display if the directory does not exist.  {0} will be replaced with the directory path.</param>
        /// <param name="notSecureFormattedMessage">The message to display if the directory is not a secure CornerStone location.  {0} will be replaced with the directory path</param>
        /// <returns>True if the directory exists, false if it does not.  If secure, returns false if location is not secure.</returns>
        public static bool CheckDirectory(string directory, bool secure = true, string notFoundFormattedMessage = "Directory does not exist: {0}", string notSecureFormattedMessage = "Directory not a secure location: {0}")
        {
            if (!IsSecureLocation(directory) && secure)
                MessageBox.Show(string.Format(notSecureFormattedMessage, directory));
            else if (!Directory.Exists(directory))
                MessageBox.Show(string.Format(notFoundFormattedMessage, directory));
            else
                return true;
            return false;
        }
        /// <summary>
        /// Checks if the specific directory exists and alerts the user if it does not.  Useful for UI dialogs.
        /// </summary>
        /// <param name="file">The file to check.</param>
        /// <param name="secure">Whether the location must be a secure CornerStone location.</param>
        /// <param name="notFoundFormattedMessage">The message to display if the file does not exist.  {0} will be replaced with the file path.</param>
        /// <param name="notSecureFormattedMessage">The message to display if the file is not in a secure CornerStone location.  {0} will be replaced with the file path</param>
        /// <returns>True if the directory exists, false if it does not.  If secure, returns false if location is not secure.</returns>
        public static bool CheckFile(string file, bool secure = true, string notFoundFormattedMessage = "File does not exist: {0}", string notSecureFormattedMessage = "File not in a secure location: {0}")
        {
            if (!IsSecureLocation(file) && secure)
                MessageBox.Show(string.Format(notSecureFormattedMessage, file));
            else if (!File.Exists(file))
                MessageBox.Show(string.Format(notFoundFormattedMessage, file));
            else
                return true;
            return false;
        }


        public static string DeleteOldFilesReturnMostCurrent(string path, string searchPattern)
        {
            string[] foundFiles = Directory.GetFiles(path, searchPattern).Select(o => o).ToArray();

            if (foundFiles.Count() == 0)
                return null;

            string newestFile = Path.GetFullPath(foundFiles[0]);
            foreach (string file in foundFiles)
            {
                if (File.GetLastWriteTime(file) < File.GetLastWriteTime(newestFile))
                {
                    FS.Delete(file);
                }
                else if (File.GetLastWriteTime(file) > File.GetLastWriteTime(newestFile))
                {
                    FS.Delete(newestFile);
                    newestFile = file;
                }
            }

            return newestFile;
        }

        /// <summary>
        /// Check if a directory is a secure CornerStone directory
        /// </summary>
        public static bool IsSecureLocation(string location)
        {
#if DEBUG
            return true;
#endif
            location = location.ToLower();
            return location.StartsWith("z:") || location.StartsWith("q:\\cs ") || location.StartsWith("y:") || location.StartsWith(@"\\imguheaaprodprc\federalimport");
        }

        [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WNetGetConnection(
            [MarshalAs(UnmanagedType.LPTStr)] string localName,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
            ref int length);
        /// <summary>
        /// Given a path, returns the UNC path or the original. (No exceptions
        /// are raised by this function directly). For example, "P:\2008-02-29"
        /// might return: "\\networkserver\Shares\Photos\2008-02-09"
        /// </summary>
        /// <param name="originalPath">The path to convert to a UNC Path</param>
        /// <returns>A UNC path. If a network drive letter is specified, the
        /// drive letter is converted to a UNC or network path. If the 
        /// originalPath cannot be converted, it is returned unchanged.</returns>
        public static string GetUNCPath(string originalPath)
        {
            StringBuilder sb = new StringBuilder(512);
            int size = sb.Capacity;

            // look for the {LETTER}: combination ...
            if (originalPath.Length > 2 && originalPath[1] == ':')
            {
                // don't use char.IsLetter here - as that can be misleading
                // the only valid drive letters are a-z && A-Z.
                char c = originalPath[0];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    int error = WNetGetConnection(originalPath.Substring(0, 2),
                        sb, ref size);
                    if (error == 0)
                    {
                        DirectoryInfo dir = new DirectoryInfo(originalPath);

                        string path = Path.GetFullPath(originalPath)
                            .Substring(Path.GetPathRoot(originalPath).Length);
                        return Path.Combine(sb.ToString().TrimEnd(), path);
                    }
                }
            }

            return originalPath;
        }

        /// <summary>
        /// Receives a comma delimited file with a header row and parses it into a Data Table
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Data Table with the data from the file passed in.</returns>
        public static DataTable CreateDataTableFromFile(string fileName)
        {
            DataTable dt = new DataTable();

            using (StreamR sr = new StreamR(fileName))
            {
                //Assign the header row
                List<string> headerFields = sr.ReadLine().SplitAndRemoveQuotes(",");
                foreach (string headerField in headerFields)
                {
                    dt.Columns.Add(headerField);
                }

                //Assign the file data to data rows
                while (!sr.EndOfStream)
                {
                    List<string> dataFields = sr.ReadLine().SplitAndRemoveQuotes(",");
                    if (dataFields.Count == dt.Columns.Count)
                        dt.Rows.Add(dataFields.ToArray());
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(string.Format("Problem reading file {0}:{1}", fileName, Environment.NewLine));
                        sb.Append(string.Format("Expected {0} fields, but found {1} fields in the line starting with {2}", dt.Columns.Count,
                            dataFields.Count, dataFields[0]));
                        throw new Exception(sb.ToString());
                    }
                }
            }

            return dt;
        }
    }
}
