using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Uheaa.Common.WinForms
{
    public class ColoredTabControl : TabControl
    {
        public ColoredTabControl()
        {
            //base.DrawMode = TabDrawMode.OwnerDrawFixed;
            //this.DrawItem += ColoredTabControl_DrawItem;
            
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
        }

        private void ColoredTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            //TabRenderer.DrawTabItem(e.Graphics, e.Bounds, TabPages[e.Index].Text, e.Font, TabItemState.Normal);
            //var style = new VisualStyleRenderer(VisualStyleElement.Tab.TopTabItem.Hot);
            //style.DrawBackground(e.Graphics, e.Bounds);
            //style.DrawText(e.Graphics, e.Bounds, TabPages[e.Index].Text);
        }

        [Browsable(false)]
        public new TabDrawMode DrawMode { get; set; }
    }
}
