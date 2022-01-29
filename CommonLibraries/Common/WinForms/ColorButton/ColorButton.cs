using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    /// <summary>
    /// This is a WIP class.
    /// </summary>
    public class ColorButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);

            BoundsManager bounds = new BoundsManager(this.Size, 5, 2);
            //foreach (RoundedPoint rp in bounds.PointList)
            //{
            //    pevent.Graphics.FillRectangle(Brushes.White, new Rectangle(rp.Point, new Size(1, 1)));
            //}
            //return;
            //pevent.Graphics.FillPath(Brushes.LightGray, bounds.TopHalfPath);
            pevent.Graphics.FillPath(Brushes.DarkGray, bounds.BottomHalfPath);
            //pevent.Graphics.DrawPath(Pens.Gray, bounds.BorderPath);
        }


    }
}
