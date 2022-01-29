using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace UpdateAndStart
{
    class Program
    {
        static void Main(string[] args)
        {
            string mode = args.First();
            Console.WriteLine("Retrieving Mode: {0}", mode);

            string imageLocation = args.Skip(1).First();
            Console.WriteLine("Retrieving Image Directory: {0}", imageLocation);

            string imageNewName = args.Skip(2).First();
            string destination = string.Format(@"\\cs1\Standards\Desktop Software\ACDC\{0}\Images\{1}.jpg", mode, imageNewName);
            Console.WriteLine("Moving image: {0} to {1}", imageLocation, destination);


            if (File.Exists(destination))
                File.Delete(destination);

            Console.WriteLine("Copying image");
            File.Copy(imageLocation, destination);

            string acdc = "";
            if (mode.ToUpper() != "LIVE")
                acdc = @"C:\Enterprise Program Files\ACDC\ACDC\Dev\ACDC.exe";
            else
            acdc = @"C:\Enterprise Program Files\ACDC\ACDC\ACDC.exe";
            ProcessStartInfo proc = new ProcessStartInfo(acdc);
            proc.Arguments = "\"" + mode + "\"";
            proc.UseShellExecute = false;

            Console.WriteLine("Killing ACDC");
            Process p = Process.GetProcessesByName("ACDC").First();
            p.Kill();
            Thread.Sleep(2000);
            Console.WriteLine("Restarting ACDC");
            Process.Start(proc);
            Environment.Exit(0);
        }
    }
}