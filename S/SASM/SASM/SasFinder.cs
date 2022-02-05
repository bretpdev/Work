using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SASM
{
    public class SasFinder
    {
        public static string FindSasExecutable()
        {
            string[] sasBase64Directory = new string[] { @"C:\Program Files\SASHome\SASFoundation\", @"C:\Program Files\SASHome2\SASFoundation\" };
            string[] sasBase32Directory = new string[] { @"C:\Program Files\SASHome\x86\SASFoundation\", @"C:\Program Files\SASHome2\x86\SASFoundation\" };
            SasResult sas64 = FindHighestSasInstallation(sasBase64Directory);
            SasResult sas32 = FindHighestSasInstallation(sasBase32Directory);

            SasResult highest = null;
            if (sas64 != null && sas32 != null)
            {
                if (sas64.Version >= sas32.Version)
                    highest = sas64;
                else
                    highest = sas32;
            }
            else if (sas64 != null)
                highest = sas64;
            else if (sas32 != null)
                highest = sas32;
            else
            {
                MessageBox.Show("Couldn't find any SAS installations.");
                Application.Exit();
            }
            return Path.Combine(highest.Location, "sas.exe");
        }

        private static SasResult FindHighestSasInstallation(string[] baseDirectories)
        {
            decimal highestBase = 0;
            string highestBaseDirectory = null;
            foreach (string baseDirectory in baseDirectories)
                if (Directory.Exists(baseDirectory))
                    foreach (string directory in Directory.GetDirectories(baseDirectory))
                        if (File.Exists(Path.Combine(directory, "sas.exe")))
                        {
                            decimal outie = 0.0m;
                            if (decimal.TryParse(Path.GetFileName(directory), out outie))
                            {
                                if (outie > 9.0m && outie > highestBase)
                                {
                                    highestBase = outie;
                                    highestBaseDirectory = directory;
                                }
                            }
                        }
            if (highestBaseDirectory == null)
                return null;
            return new SasResult() { Location = highestBaseDirectory, Version = highestBase };
        }

        class SasResult
        {
            public string Location { get; set; }
            public decimal Version { get; set; }
        }
    }
}
