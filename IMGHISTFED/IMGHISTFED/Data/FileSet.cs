using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGHISTFED
{
    public class FileSet
    {
        public FileSet(string pdfFile, string uncommitedPdfFile, string ssn, BorrowerIndexRecord record)
        {
            PdfFile = pdfFile;
            UncommitedPdfFile = uncommitedPdfFile;
            Ssn = ssn;
            IndexRecord = record;
        }

        //public string ImagingFile { get; set; }
        public string PdfFile { get; set; }
        public string UncommitedPdfFile { get; set; }
        public string Ssn { get; set; }
        public BorrowerIndexRecord IndexRecord { get; set; }
    }
}
