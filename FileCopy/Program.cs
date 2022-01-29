using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> files = new List<string>() {"UNWS14.NWS14R6.*","UNWS14.NWS14R23.*","UNWS14.NWS14R24.*","UNWS14.NWS14R25.*",
                                                        "SCRA - FED R4*","UNWS91.NWS91R2*","UNWS91.NWS91R3*","UNWS91.NWS91R4*"};

            foreach (string file in files)
            {
                List<string> foundFiles = Directory.GetFiles(@"Z:\Archive\SAS\", file).ToList();
                foreach (string foundFile in foundFiles)
                {

                    FileInfo fi = new FileInfo(foundFile);
                    if (fi.LastWriteTime > new DateTime(2015, 07, 10))
                    {
                        File.Copy(foundFile, Path.Combine(@"C:\Serf_file", Path.GetFileName(foundFile)));
                    }
                }
            }

        }
    }
}
