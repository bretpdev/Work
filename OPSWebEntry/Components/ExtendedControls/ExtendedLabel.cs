using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace OPSWebEntry
{
    /// <summary>
    /// Contains additional properties to simplify XAML markup.
    /// </summary>
    public class ExtendedLabel : Label
    {
        /// <summary>
        /// Get or set the Grid.Row, Grid.Column, Grid.RowSpan, and Grid.ColSpan in the following format:
        /// {Col}:{ColSpan|Default 1},{Row}:{RowSpan|Default 1}
        /// Example: "2,3:2" = Row: 2, RowSpan: 1, Column: 3, ColSpan: 2
        /// </summary>
        public string GridPos
        {
            get
            {
                return GridPosHelper.GetGridPosString(this);
            }
            set
            {
                GridPosHelper.SetGridPosString(this, value);
            }
        }
    }
}
