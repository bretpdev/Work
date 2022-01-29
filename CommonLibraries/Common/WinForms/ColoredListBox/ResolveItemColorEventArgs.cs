using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class ResolveItemColorEventArgs
    {
        public object Item { get; set; }
        public Color BackgroundColor { get; set; }
        public Color HighlightColor { get; set; }
        public int ItemIndex { get; set; }
        public ResolveItemColorEventArgs(int index, object item) : this(index, item, Color.White, SystemColors.Highlight) { }
        public ResolveItemColorEventArgs(int index, object item, Color currentBackground, Color currentHighlight)
        {
            Item = item;
            ItemIndex = index;
            BackgroundColor = currentBackground;
            HighlightColor = currentHighlight;
        }
    }
}
