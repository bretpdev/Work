using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            //expecting one location per arg
            var minDate = new DateTime(2019, 1, 1);
            File.WriteAllLines(@"T:\UHEAA Code list.txt", args.SelectMany(o => GetModifiedFiles(o, minDate)));
        }

        static string[] exclusions = new string[] {
            "uheaa.common", "interop.reflection.dll", "interop.dmatrixlib.dll", "interop.rfcomapilib.dll","WHOAMI.dll",
            "interop.vba.dll", "vshost.exe", "vshost.dll", "itextsharp.dll", "q.dll", "mincap.dll", "Ionic.Zip.dll", "System.dll", "Newtonsoft.Json.dll",
            "Microsoft.Office.Interop.Word.dll", "Microsoft.Vbe.Interop.dll", "stdole.dll", "office.dll", "Interop.OSSMTP.dll", "Xceed.Wpf.Toolkit.dll",
            "SessionTester.exe", "WinSCP.exe", "WinSCPnet.dll"
        };
        private static List<string> GetModifiedFiles(string directory)
        {
            List<string> results = new List<string>();
            foreach (var file in Directory.GetFiles(directory))
            {
                string f = file.ToLower();
                if (f.EndsWith(".dll") || f.EndsWith(".exe"))
                    if (!exclusions.Any(o => f.Contains(o.ToLower())))
                            results.Add(new FileInfo(file).LastWriteTime.ToString("MM-dd-yyyy") + " " + file);
            }
            foreach (var dir in Directory.GetDirectories(directory))
            {
                results.AddRange(GetModifiedFiles(dir));
            }
            return results;
        }
    }
}
