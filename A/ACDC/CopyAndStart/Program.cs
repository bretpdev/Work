using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CopyAndStart
{
    class Program
    {
        /// <summary>
        /// Arg 1: The source folder to copy from
        /// Arg 2: The name of the exe to start once the operation is complete.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string source = args.First();
            Console.WriteLine("Retrieved Source Directory: " + source);
            string start = args.Skip(1).First();
            Console.WriteLine("Retrieved EXE Name: " + start);
            self = Assembly.GetExecutingAssembly().Location;
            string dest = Path.GetDirectoryName(self);
            Console.WriteLine("Calculated Destinations as: " + dest);
            Copy(source, dest);
            string exePath = Path.Combine(dest, start);
            ProcessStartInfo proc = new ProcessStartInfo(exePath, string.Join(" ", args.Skip(2)));
            proc.UseShellExecute = false;
            Process.Start(proc);
        }
        private static string self;

        private static void Copy(string sourceDirectory, string destDirectory)
        {
            foreach (string path in Directory.GetFiles(sourceDirectory))
            {
                string filename = Path.GetFileName(path);
                string dest = Path.Combine(destDirectory, filename);
                if (dest.ToLower() == self.ToLower()) continue; //don't try to overwrite this exe
                int retryCount = 20;
                while (retryCount > 0)
                {
                    try
                    {
                        Console.WriteLine("Attempting to retrieved file: " + path);
                        File.Copy(path, dest, true);
                        Console.WriteLine("Successfully downloaded file at: " + dest);
                        retryCount = 0;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Unable to retrieve file. " + retryCount + " attempts remaining.");
                        Thread.Sleep(1000);
                        retryCount--;
                    }
                }
            }
            foreach (string path in Directory.GetDirectories(sourceDirectory))
            {
                string directoryName = new DirectoryInfo(path).Name;
                string dest = Path.Combine(destDirectory, directoryName);
                if (!Directory.Exists(dest)) Directory.CreateDirectory(dest);
                Copy(path, dest); 
            }
        }
    }
}
