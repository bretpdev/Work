using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTNOTFORB
{
    public class FileDetails
    {
        public string FilePattern { get; internal set; }
        public string NewestFile { get; set; }
        public string ActiviyComment { get; internal set; }
        public bool NeedsLetter { get; internal set; }

        public FileDetails(string filePattern, string comment, bool needsLetter)
        {
            FilePattern = filePattern;
            ActiviyComment = comment;
            NeedsLetter = needsLetter;
        }
    }
}
