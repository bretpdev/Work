using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INVOSAMPFD
{
    interface ReportItem { }
    class ReportImage : ReportItem
    {
        public Image Image { get; set; }
    }
    class ReportText : ReportItem
    {
        public string Text { get; set; }
    }
    class ReportError : ReportText { }
    class ReportTable : ReportItem
    {
        public ReportTable()
        {
            TableRows = new List<string[]>();
        }
        public string TableHeader { get; set; }
        public List<string[]> TableRows { get; set; }

        public void AddRow(params string[] items)
        {
            TableRows.Add(items);
        }
    }
}
