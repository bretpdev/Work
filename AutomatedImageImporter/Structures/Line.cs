using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedImageImporter
{
    public class Line
    {
        public string Text { get; set; }
        public int LineNumber { get; set; }
        public Line(string text, int lineNumber)
        {
            this.Text = text;
            this.LineNumber = lineNumber;
        }
    }

}
