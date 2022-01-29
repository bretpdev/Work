using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTester
{
    class NtFile
    {
        public string FileName { get; set; }
        public string Location { get; set; }
        public string Arguments { get; set; }
        public bool AutoLaunch { get; set; }

        public LoadResults Load(string filename)
        {
            this.FileName = filename;
            var results = new LoadResults();
            foreach (var line in File.ReadAllLines(filename))
            {
                int colon = line.IndexOf(':');
                var key = line.Substring(0, colon).ToLower();
                var val = line.Substring(colon + 1).Trim();
                if (key == "location")
                    Location = val;
                else if (key == "arguments")
                    Arguments = val;
                else if (key == "autolaunch")
                    AutoLaunch = new string[] { "yes", "1", "true" }.Contains(val.ToLower());
                else
                    results.UnknownKeys.Add(key);
            }
            return results;
        }

        public void Save(string filename = null)
        {
            if (filename == null)
                filename = FileName;
            string file = "Location: {0}\r\nArguments: {1}\r\nAutoLaunch: {2}";
            file = string.Format(file, Location, Arguments, AutoLaunch.ToString());
            File.WriteAllText(filename, file);
            FileName = filename;
        }

        public class LoadResults
        {
            public bool Successful { get { return !UnknownKeys.Any(); } }
            public List<string> UnknownKeys { get; private set; }
            public LoadResults()
            {
                UnknownKeys = new List<string>();
            }
        }

        public string GetLocalLocation(string downloadLocation)
        {
            string hash = (this.FileName ?? "").ToLower().GetHashCode().ToString();
            return Path.Combine(downloadLocation, hash);
        }
    }
}
