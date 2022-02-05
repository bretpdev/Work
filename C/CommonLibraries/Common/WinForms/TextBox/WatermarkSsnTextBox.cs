using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public class WatermarkSsnTextBox : SsnTextBox
    {
        private string watermark;
        public string Watermark
        {
            get { return watermark; }
            set
            {
                watermark = value;
                SyncUI();
            }
        }
        public static Color WatermarkColor = Color.DarkGray;
        private Color cachedForeColor;
        [DefaultValue(typeof(Color), "0xFF0000")]
        public new Color ForeColor
        {
            get { return cachedForeColor; }
            set
            {
                cachedForeColor = value;
                base.ForeColor = value;
            }
        }
        private string text;
        public new string Text
        {
            get { return text ?? ""; }
            set
            {
                text = value;
                SyncUI();
            }
        }
        public void SyncUI()
        {
            if (!this.Focused && text == watermark)
            {
                base.Text = watermark;
                text = null;
            }

            if (text.IsNullOrEmpty() && !this.Focused)
            {
                base.Text = Watermark;
                base.ForeColor = WatermarkColor;
                Italicize();
            }
            else if (this.Focused && text == watermark)
            {
                base.ForeColor = WatermarkColor;
                Italicize();
            }
            else
            {
                base.Text = text;
                base.ForeColor = cachedForeColor;
                Italicize(false);
            }
        }
        private void Italicize(bool italic = true)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime) //designer gets REALLY screwy if you change the font during initialization
                this.Font = new Font((this.Parent ?? this).Font, italic ? FontStyle.Italic : FontStyle.Regular);
        }
        public WatermarkSsnTextBox()
        {
            this.TextChanged += WatermarkTextBox_TextChanged;
            this.GotFocus += WatermarkTextBox_GotFocus;
            this.LostFocus += WatermarkTextBox_LostFocus;
            cachedForeColor = base.ForeColor;
        }

        void WatermarkTextBox_GotFocus(object sender, EventArgs e)
        {
            if (text.IsNullOrEmpty())
            {
                codeTriggered = true;
                base.Text = null;
                codeTriggered = false;
            }
        }

        bool codeTriggered = false;
        void WatermarkTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!codeTriggered)
            {
                Text = base.Text;
                SyncUI();
            }
        }

        void WatermarkTextBox_LostFocus(object sender, EventArgs e)
        {
            SyncUI();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!codeTriggered)
                base.OnTextChanged(e);
        }
    }
}
