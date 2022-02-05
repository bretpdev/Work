using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Uheaa.Common;

namespace OPSWebEntry
{
    public static class GridPosHelper
    {
        static readonly string gridRegex;
        static GridPosHelper()
        {
            string integer = @"(\d+)"; //matches a single whole number (multiple digits allowed)
            //matches a single whole number (multiple digits allowed), but only when directly preceeded by a colon
            string colonInteger = @"(?:[:](\d+))?"; 
            string comma = @"[,]"; //matches a single comma
            gridRegex = integer + colonInteger + comma + integer + colonInteger;
        }
        public static string GetGridPosString(Control control)
        {
            var row = (int?)control.GetValue(Grid.RowProperty);
            var rowSpan = (int?)control.GetValue(Grid.RowSpanProperty);
            var col = (int?)control.GetValue(Grid.ColumnProperty);
            var colSpan = (int?)control.GetValue(Grid.ColumnSpanProperty);

            if (row == null && col == null)
                return null;
            return row + (rowSpan != null ? ":" + rowSpan : null) + "," + col + (colSpan != null ? ":" + colSpan : null);
        }

        public static void SetGridPosString(Control control, string gridPosString)
        {
            Regex r = new Regex(gridRegex);
            System.Text.RegularExpressions.Match m = r.Match(gridPosString);
            if (!m.Success)
                throw new FormatException("GridPos must be in the following format: {Col}:{ColSpan|Default 1},{Row}:{RowSpan|Default 1}");

            var col = m.Groups[1].Value;
            control.SetValue(Grid.ColumnProperty, col.ToInt());

            var colSpan = m.Groups[2].Value;
            control.SetValue(Grid.ColumnSpanProperty, !string.IsNullOrEmpty(colSpan) ? colSpan.ToInt() : 1);

            var row = m.Groups[3].Value;
            control.SetValue(Grid.RowProperty, row.ToInt());

            var rowSpan = m.Groups[4].Value;
            control.SetValue(Grid.RowSpanProperty, !string.IsNullOrEmpty(rowSpan) ? rowSpan.ToInt() : 1);
        }

    }
}
