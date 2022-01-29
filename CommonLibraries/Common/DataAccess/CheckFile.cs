using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common
{
    public static class CheckFile
    {
        public static string FileName { get; set; }

        /// <summary>
        /// Verifies the file is in the correct region, that it exists, that it is not empty and that there is only one copy
        /// </summary>
        /// <param name="path">The path and file name, file extension included. If SAS, do not include filename and extension, just the folder.</param>
        /// <param name="hasExtension">If the path has an extension (.xlsx, .doc, .txt), set to true.</param>
        /// <param name="searchPattern">If SAS file, the search pattern is the SAS filename with .*</param>
        /// <returns>True if the file passes all checks, false if there is an error with the file</returns>
        public static string Check(string path, bool hasExtension = false, string searchPattern = null)
        {
            bool didPass = true;

            if (!ValidateRegion(path))
                didPass = false;
            else if (!FileExists(path, searchPattern))
                didPass = false;
            else if (MultipleFiles(path, hasExtension, searchPattern))
                didPass = false;
            else if (FileEmpty())
            {
                didPass = false;
                FS.Delete(FileName);
            }

            if (didPass)
                return FileName;
            else
                return string.Empty;
        }

        /// <summary>
        /// Validates that the file is being opened in the correct region
        /// </summary>
        /// <param name="path">The path and file name, file extension included</param>
        /// <returns>True if in the correct region, false if not</returns>
        private static bool ValidateRegion(string path)
        {
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
                if (!FileSystemHelper.IsSecureLocation(path))
                {
                    MessageBox.Show("The file is in the wrong region.", "Wrong Region", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            return true;
        }

        /// <summary>
        /// Checks if the file exits
        /// </summary>
        /// <param name="path">The path and file name, file extension included</param>
        /// <param name="searchPattern">The SAS file name with .* on the end</param>
        /// <returns>True if the file exists, false if it dose not</returns>
        private static bool FileExists(string path, string searchPattern)
        {
            if (!string.IsNullOrEmpty(searchPattern))
            {
                var files = Directory.GetFiles(path, searchPattern);
                if (!files.Any())
                {
                    MessageBox.Show(string.Format("There is not a file {0} available to process.", searchPattern), "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                if (File.Exists(path))
                    return true;
                else
                {
                    MessageBox.Show("There is not a file available to process.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if there are multiple files
        /// </summary>
        /// <param name="path">The path and file name, file extension included</param>
        /// <returns>True if there are multiple files, false if there are not</returns>
        private static bool MultipleFiles(string path, bool hasExtension, string searchPattern)
        {
            string folder = path.Substring(0, path.Length - (path.Length - path.LastIndexOf("\\") - 1));
            string extension = hasExtension ? path.Substring(path.LastIndexOf(".")) : "";
            string fileName = searchPattern == "" ? path.Substring(path.LastIndexOf("\\") + 1, path.Length - path.LastIndexOf("\\") - 1) : searchPattern;

            IEnumerable<string> files;
            if (hasExtension)
            {
                fileName = fileName.Remove(fileName.LastIndexOf("."));
                files = Directory.GetFiles(folder, extension).Where(p => p.ToString().Contains(fileName));
            }
            else
                files = Directory.GetFiles(folder, fileName);

            if (files.Count() > 1)
            {
                MessageBox.Show(string.Format("There are multiple {0} files to process. Please remove all invalid files.",searchPattern), "Multiple Files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
                FileName = files.First();

            return false;
        }

        /// <summary>
        /// Checks if the file is empty
        /// </summary>
        /// <param name="path">The path and file name, file extension included</param>
        /// <returns>True if file is empty, false if it is not</returns>
        private static bool FileEmpty()
        {
            using (StreamReader sr = new StreamR(FileName))
            {
                sr.ReadLine();//Header
                if (sr.EndOfStream)
                {
                    MessageBox.Show(string.Format("The file {0} is empty.", FileName), "Empty File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts a file to use DOS newline characters (CR/LF) instead of UNIX newline characters (LF).
        /// </summary>
        /// <param name="fileName">The full path and name of the file to convert.</param>
        /// <returns></returns>
        public static string Unix2DosNewline(string fileName)
        {
            string tempFile = EnterpriseFileSystem.TempFolder + "Unix2Dos";
            try
            {
                using (StreamReader sr = new StreamR(fileName))
                {
                    using (StreamWriter sw = new StreamW(tempFile))
                    {
                        char previousChar = '\0';
                        while (!sr.EndOfStream)
                        {
                            char currentChar = Convert.ToChar(sr.Read());
                            if (currentChar == '\n' && previousChar != '\r')
                                sw.Write("\r\n");
                            else
                                sw.Write(currentChar);

                            previousChar = currentChar;
                        }
                    }
                }

                FS.Copy(tempFile, fileName, true);
            }
            finally
            {
                FS.Delete(tempFile);
            }
            return null;
        }
    }
}
