using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Uheaa.Common;

namespace MD
{
    public static class VersionHelper
    {
        public static Version VersionObject { get { return Assembly.GetExecutingAssembly().GetName().Version; } }
        public static string Version { get { return VersionObject.ToString(); } }
        public static string RelevantVersion { get { return "{0}.{1}.{2}".FormatWith(VersionObject.Major, VersionObject.Minor, VersionObject.Build); } }
    }
}
