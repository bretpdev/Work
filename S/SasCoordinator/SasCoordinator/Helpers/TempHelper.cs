using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace SasCoordinator
{
    class TempHelper
    {
        readonly string TempFolder = Path.Combine(EnterpriseFileSystem.TempFolder, "SasCoordinator");
        public string GetTempFileName()
        {
            string filename = Guid.NewGuid().ToString().Replace("-", "");
            if (!Directory.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);
            return Path.Combine(TempFolder, filename);
        }

        public string CreateTempFile(string fileContents)
        {
            string filename = GetTempFileName();
            File.AppendAllText(filename, fileContents);
            return filename;
        }

        public void ClearTempDirectory(int minAgeInDays = 21)
        {
            ClearDirectory(TempFolder, minAgeInDays);
        }

        private void ClearDirectory(string directory, int minAgeInDays)
        {
            foreach (var dir in Directory.GetDirectories(directory))
                ClearDirectory(dir, minAgeInDays);
            foreach (var file in Directory.GetFiles(directory))
            {
                double age = (DateTime.UtcNow - new FileInfo(file).CreationTimeUtc).TotalDays;
                if (age >= minAgeInDays)
                    File.Delete(file);
            }
        }
    }
}
