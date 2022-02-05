using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    [Serializable]
    public class ColumnLayoutInfo
    {
        public bool NotSet { get { return Columns.Count == 0; } }
        public List<ColumnInfo> Columns { get; set; }
        public ColumnLayoutInfo()
        {
            Columns = new List<ColumnInfo>();
        }

        public ColumnLayoutInfo(DataGridView source)
            : this()
        {
            foreach (DataGridViewColumn c in source.Columns)
                Columns.Add(new ColumnInfo() { Name = c.Name, Width = c.Width, Index = c.DisplayIndex });
        }

        public void OrderGrid(DataGridView toOrder)
        {
            foreach (DataGridViewColumn c in toOrder.Columns)
            {
                var info = Columns.FirstOrDefault(o => o.Name == c.Name);
                if (info != null)
                {
                    c.Width = info.Width;
                    c.DisplayIndex = info.Index;
                }
            }
        }

        public class ColumnInfo
        {
            public string Name { get; set; }
            public int Width { get; set; }
            public int Index { get; set; }
        }
    }
}
