using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace INVOSAMPFD
{
    /// <summary>
    /// Represents the structural components of the final document.
    /// This class is also capable of compiling its data into a word document.
    /// </summary>
    class Report
    {
        public string Ssn { get; set; }
        public bool HasErrors { get; set; }
        public Report(string ssn)
        {
            Sections = new List<ReportSection>();
            this.Ssn = ssn;
        }
        public List<ReportSection> Sections { get; set; }

        /// <summary>
        /// Create a new ReportSection with the given header, and add it to the list of sections.
        /// </summary>
        public ReportSection AddSections(string sectionHeader)
        {
            ReportSection rs = new ReportSection();
            rs.Header = sectionHeader;
            Sections.Add(rs);
            return rs;
        }
    }
}
