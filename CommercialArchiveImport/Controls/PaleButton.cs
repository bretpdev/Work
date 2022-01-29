using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommercialArchiveImport
{
    public class PaleButton : Button
    {
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                if (this.DesignMode)
                {
                    base.BackgroundImage = value;
                    return;
                }
                Bitmap b = new Bitmap(value.Width, value.Height);
                Graphics g= Graphics.FromImage(b);
                ColorMatrix cm = new ColorMatrix();
                cm.Matrix33 = 0.4f;
                ImageAttributes ia = new ImageAttributes();
                ia.SetColorMatrix(cm);

                g.DrawImage(value, new Rectangle(0, 0, b.Width, b.Height), 0, 0, value.Width, value.Height, GraphicsUnit.Pixel, ia);
                base.BackgroundImage = b;
            }
        }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }
    }
}
