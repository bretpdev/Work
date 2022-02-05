using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PYOFFLTRFD
{
    class MyDataGridView : DataGridView
    {
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
			if (e.RowIndex == -1) //ignore header click
				return;
            this.Rows[e.RowIndex].Selected = !this.Rows[e.RowIndex].Selected;
        }

        protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
        {
        }
    }
}