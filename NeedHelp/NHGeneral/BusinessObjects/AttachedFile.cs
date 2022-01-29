using System;
using System.IO;

namespace NHGeneral
{
    public class AttachedFile
    {
        public string DisplayText
        {
            get
            {
                return FileName + " - " + FileDate.ToShortDateString() + " " + FileDate.ToShortTimeString();
            }
        }
        public string FilePath { get; set; }
        public string FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }
        public DateTime FileDate { get; set; }
        public AttachedFile(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            FilePath = fi.FullName;
            FileDate = fi.CreationTime;
        }
    }
}