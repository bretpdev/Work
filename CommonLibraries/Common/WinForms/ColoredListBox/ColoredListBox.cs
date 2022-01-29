using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class ColoredListBox : ListBox
    {
        public delegate void OnResolveItemColor(object sender, ResolveItemColorEventArgs e);
        public event OnResolveItemColor ResolveItemColor;

        [DefaultValue(typeof(Color), "51, 153, 255")]
        public Color HighlightColor { get; set; }

        public ColoredListBox()
        {
            HighlightColor = SystemColors.Highlight; //default
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true); //no flickering
            this.DoubleBuffered = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            this.Invalidate();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index >= this.Items.Count || e.Index == -1)
            {
                base.OnDrawItem(e);
                return;
            }
            object obj = this.Items[e.Index];
            ResolveItemColorEventArgs args = new ResolveItemColorEventArgs(e.Index, obj, this.BackColor, HighlightColor);
            if (ResolveItemColor != null)
                ResolveItemColor(this, args);
            
            if (e.Index == this.SelectedIndex)
                e.Graphics.FillRectangle(new SolidBrush(args.HighlightColor), e.Bounds);
            else
                e.Graphics.FillRectangle(new SolidBrush(args.BackgroundColor), e.Bounds);

            string text = obj.ToString();
            if (!DisplayMember.IsNullOrEmpty())
                text = (string)obj.GetType().GetProperty(DisplayMember).GetValue(obj, null);
            TextRenderer.DrawText(e.Graphics, text, this.Font, e.Bounds, this.ForeColor, TextFormatFlags.Left);
            e.DrawFocusRectangle();
        }
    }
}
