using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.CommonScreens
{
    static class AccessLog
    {
        private class AccessEntry
        {
            public string Path { get; set; }
            public string ScreenCode { get; set; }
            public string Timestamp { get; set; }
        }
        const string logName = "CommonScreenAccessLog.uheaalog";
        static List<AccessEntry> entriesLoggedInThisInstance = new List<AccessEntry>();
        public static void RegisterAccess(string screenCode)
        {
            string logPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), logName);
            if (entriesLoggedInThisInstance.Any(o => o.Path == logPath && o.ScreenCode == screenCode))
                return; //already logged this entry

            string callingPath = Assembly.GetEntryAssembly().Location;
            List<AccessEntry> entries = new List<AccessEntry>();
            if (File.Exists(logPath))
                entries = CsvHelper.ParseTo<AccessEntry>(File.ReadAllLines(logPath)).ValidLines.Select(o => o.ParsedEntity).ToList();

            AccessEntry entry = entries.SingleOrDefault(o => o.Path == callingPath && o.ScreenCode == screenCode);
            if (entry == null)
                entry = new AccessEntry();
            entry.Path = callingPath;
            entry.ScreenCode = screenCode;
            entry.Timestamp = DateTime.Now.ToString();
            entriesLoggedInThisInstance.Add(entry);
            if (!entries.Contains(entry))
                entries.Add(entry);

            List<string> lines = new List<string>();
            lines.Add("Path,ScreenCode,Timestamp");
            lines.AddRange(entries.Select(o => CsvHelper.SimpleEncode(new string[] { o.Path, o.ScreenCode, o.Timestamp })));
            File.WriteAllLines(logPath, lines.ToArray());
        }
    }
}
