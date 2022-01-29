using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common
{
    //public static class EverTemp
    //{
    //    /// <summary>
    //    /// Check to see if the current application is running from the T drive, and if not, copy it to the T drive and restart it from there.
    //    /// </summary>
    //    public static bool EnsureTDrive(string[] args)
    //    {
    //        return EnsureDrive(args, "T");
    //    }

    //    public static bool EnsureTOrCDrive(string[] args)
    //    {
    //        return EnsureDrive(args, "T", "C");
    //    }
    //    private static bool EnsureDrive(string[] args, params string[] acceptableDriveLetters)
    //    {
    //        var location = Assembly.GetEntryAssembly().Location;
    //        if (!acceptableDriveLetters.Any(o => location.StartsWith(o + ":")))
    //        {
    //            var existingFolder = Path.GetDirectoryName(location);
    //            string newLocation = Path.Combine("T:\\EverTemp\\" + Guid.NewGuid().ToString(), Path.GetFileName(existingFolder));
    //            if (!Directory.Exists(newLocation))
    //                FS.CreateDirectory(newLocation);
    //            foreach (var file in Directory.GetFiles(existingFolder))
    //                FS.Copy(file, Path.Combine(newLocation, Path.GetFileName(file)), true);

    //            ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(newLocation, Path.GetFileName(location)));
    //            psi.Arguments = string.Join(" ", args.Select(o => "\"" + o + "\""));
    //            if (psi.Arguments.EndsWith("\""))
    //                psi.Arguments = psi.Arguments.Substring(0, psi.Arguments.Length - 1);
    //            Proc.Start(psi);
    //            return false;
    //        }
    //        return true;
    //    }
    //}
}
