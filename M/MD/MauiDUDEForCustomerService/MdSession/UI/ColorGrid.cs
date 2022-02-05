using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MdSession
{
    public class ColorGrid : TableLayoutPanel
    {
        public ColorGrid()
        {
            int size = (int)Math.Sqrt(ColorCodes.Codes.Count);
            float percent = 100f / size;
            for (int row = 0; row < size; row++)
            {
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20));
                for (int col = 0; col < size; col++)
                {
                    this.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
                    int i = row * size + col;
                    var code = ColorCodes.Codes[i];
                    Label color = new Label();
                    color.Font = new System.Drawing.Font("Arial", 6);
                    color.Height = color.Width = 20;
                    color.BackColor = code.Color;
                    color.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    color.Margin = Padding.Empty;
                    color.Click += (o, ea) =>
                    {
                        if (ColorSelected != null)
                            ColorSelected(this, code);
                    };
                    this.Controls.Add(color, col, row);
                }
            }
            this.Width = size * 20;
        }

        public delegate void ColorSelectedEventHandler(object sender, ColorCode code);
        public event ColorSelectedEventHandler ColorSelected;
    }
}
