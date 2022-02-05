using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSeparator
{
    public class EOJReport
    {
        public string FileName { get; set; }
        public string SaveLocation { get; set; }
        public int RowCount { get; set; }
        public int NumberOfNewFiles { get; set; }
        public int RowsPerFile { get; set; }
        public List<NewFiles> NewFile { get; set; }

        public EOJReport()
        {
            NewFile = new List<NewFiles>();
        }
    }

    public class NewFiles
    {
        public string FileName { get; set; }
        public int RowCount { get; set; }
    }
}
