using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// For a text field, specify the number of lines/rows the field should show
    /// </summary>
    public class TextBoxLinesAttribute : Attribute
    {
        public int LineCount { get; set; }
        public TextBoxLinesAttribute(int lineCount)
        {
            LineCount = lineCount;
        }
    }
}
