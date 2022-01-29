using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLMPMTPST
{
    public class FileSearchResult
    {
        public bool FileFound { get; set; }
        public string Error { get; set; }
        public string FileName { get; set; }

        public FileSearchResult(bool fileFound, string error, string fileName = "")
        {
            FileFound = fileFound;
            Error = error;
            FileName = fileName;
        }

    }
}
