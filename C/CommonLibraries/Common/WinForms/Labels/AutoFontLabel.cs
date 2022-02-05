using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class AutoFontLabel : Label
    {
        public AutoFontLabel()
        {
            this.SizeChanged += AutoFontLabel_SizeChanged;
            this.Padding = Padding.Empty;
        }

        bool calculatingResize = false;
        public void CalculateResize()
        {
            if (calculatingResize) return;
            if (MaxFontSize == 0) return;
            calculatingResize = true;
            Size max = this.MaximumSize;
            Size min = this.MinimumSize;
            //prevents control from resizing in response to font change
            this.MaximumSize = this.MinimumSize = this.Size;
            //grow text until it fits the label or the maximum
            float tempFontSize = MaxFontSize;
            Font original = this.Font;
            Size textSize = Size.Empty;
            Func<Font> calcFont = () => new Font(original.FontFamily, tempFontSize);
            Action render = () => textSize = TextRenderer.MeasureText(Text, calcFont(), this.Size, TextFormatFlags.WordBreak);
            render();
            while (this.PreferredSize.Width > this.Size.Width)
            {
                tempFontSize -= 0.1f;
                this.Font = calcFont();
                if (tempFontSize <= 0)
                {
                    //couldn't fit in the label, give up and use original font
                    tempFontSize = original.Size;
                    break;
                }
                render();
            }
            base.Font = calcFont();
            //reset max and min
            this.MaximumSize = max;
            this.MinimumSize = min;
            calculatingResize = false;
        }
        void AutoFontLabel_SizeChanged(object sender, EventArgs e)
        {
            CalculateResize();
        }
        public new Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                MaxFontSize = value.Size;
                CalculateResize();
            }
        }
        public new string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                CalculateResize();
            }
        }
        /// <summary>
        /// The maximum font size in em.
        /// </summary>
        public float MaxFontSize { get; set; }

    }
}
