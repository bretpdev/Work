using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Uheaa.Common;

namespace GitEmailVersionHook
{
    class Program
    {
        const string versionBegin = "<!-- Version ";
        const string versionEnd = " -->";
        const string version = versionBegin + "{0}" + versionEnd;
        const string GitInstallPath = @"C:\Program Files\Git\bin\git.exe";
        /// <summary>
        /// Adds or updates the last line of all email templates to a Version Number.
        /// </summary>
        static void Main(string[] args)
        {
            var workingDirectory = Assembly.GetExecutingAssembly().Location; //.git/hooks/.exe
            workingDirectory = Path.GetDirectoryName(workingDirectory); //.git/hooks
            workingDirectory = Path.GetDirectoryName(workingDirectory); //.git
            workingDirectory = Path.GetDirectoryName(workingDirectory); //project root

            var files = GetModifiedFiles(workingDirectory).Where(o => o.ToLower().EndsWith(".htm") || o.ToLower().EndsWith(".html"));
            foreach (var templateFile in files)
            {
                var lines = File.ReadAllLines(templateFile).ToList();
                while (string.IsNullOrWhiteSpace(lines.Last()))
                    lines.RemoveAt(lines.Count - 1);
                var lastLine = lines.LastOrDefault() ?? "";
                int versionNumber = lastLine.Replace(versionBegin, "").Replace(versionEnd, "").ToIntNullable() ?? 0;
                if (versionNumber == 0)
                    lines.Add(""); //no existing version number, add a line for it
                versionNumber++;
                lines[lines.Count - 1] = string.Format(version, versionNumber);
                var tempFileName = templateFile + "_backup";
                File.Copy(templateFile, tempFileName);
                File.Delete(templateFile);
                File.WriteAllText(templateFile, string.Join(Environment.NewLine, lines));
                File.Delete(tempFileName);
                GitAdd(workingDirectory, templateFile);
            }
        }

        private static ProcessStartInfo GetGit(string workingDirectory, string arguments)
        {
            var psi = new ProcessStartInfo(GitInstallPath, arguments)
            {
                WorkingDirectory = workingDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            return psi;
        }

        private static List<string> GetModifiedFiles(string workingDirectory)
        {
            var results = new List<string>();

            var process = Process.Start(GetGit(workingDirectory, "status --porcelain"));
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine().Trim();
                if (line.StartsWith("M") || line.StartsWith("A"))
                {
                    var localFileName = line.Substring(2).Trim();
                    localFileName = Path.Combine(workingDirectory, localFileName);
                    results.Add(localFileName);
                }
            }
            return results;
        }

        private static void GitAdd(string workingDirectory, string fileName)
        {
            string localFileName = fileName.Substring(workingDirectory.Length).Trim('\\');
            var psi = GetGit(workingDirectory, "add " + localFileName);
            Process.Start(psi).WaitForExit();
        }
    }
}
