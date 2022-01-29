using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace MD
{
    public static class TempHelper
    {
        static string tempBase = Path.Combine(EnterpriseFileSystem.TempFolder,@"MD\");
        static TempHelper()
        {
            if (!Directory.Exists(tempBase))
                FS.CreateDirectory(tempBase);
        }

        public static string GetPath(string path)
        {
            path = Path.Combine(tempBase, path);
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                FS.CreateDirectory(directory);
            if (!Path.HasExtension(path))
                FS.CreateDirectory(path);
            return path;
        }
    }
}
