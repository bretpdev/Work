using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace SasCoordinator
{
    public class SasSoftwareHelper
    {
        private readonly TempHelper temp = new TempHelper();
        public string GetConnectionCode(SasRegion region, bool testMode)
        {
            string file = "Duster.sas"; 
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, "Connections");
            path = Path.Combine(path, file);
            return File.ReadAllText(path);
        }

        public string CreateTempFile(string fileContents)
        {
            return temp.CreateTempFile(fileContents);
        }

        public void ClearOldTempDirectoryFiles(int minAgeInDays = 7)
        {
            temp.ClearTempDirectory(minAgeInDays);
        }

        public string AesLinkLocation { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"AES\AESLink.NET\plink.exe"); } }
        public string FindSasExecutable()
        {
            string sasBase32Directory = @"C:\Program Files\SASHome2\x86\SASFoundation\9.4\";
            if (File.Exists(Path.Combine(@"C:\Program Files\SASHome\x86\SASFoundation\9.4\","sas.exe")))
                return Path.Combine(@"C:\Program Files\SASHome\x86\SASFoundation\9.4\", "sas.exe");

            return Path.Combine(sasBase32Directory, "sas.exe");
        }

        class SasResult
        {
            public string Location { get; set; }
            public decimal Version { get; set; }
        }
    }

}
