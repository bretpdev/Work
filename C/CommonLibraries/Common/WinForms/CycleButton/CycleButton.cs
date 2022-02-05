using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    [DefaultEvent("Cycle")]
    public class CycleButton<T> : Button
    {
        public CycleOptionCollection<T> Options { get; internal set; }
        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = ValidateIndex(value) ?? 0;
                SetText();
            }
        }
        public T SelectedValue
        {
            get { return ValidIndex(selectedIndex) ? Options[selectedIndex].Value : default(T); }
            set
            {
                if (value == null)
                    selectedIndex = 0;
                else
                    selectedIndex = Options.Select((o, i) => new { Option = o.Value, Index = i }).Where(o => o.Option.Equals(value)).SingleOrDefault().Index;
                SetText();
            }
        }
        public string SelectedLabel
        {
            get
            {
                if (SelectedOption == null) return null;
                return SelectedOption.Label;
            }
        }
        protected CycleOption<T> SelectedOption
        {
            get
            {
                if (ValidIndex(selectedIndex))
                    return Options[selectedIndex];
                return null;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] //don't show in designer or create entry in InitializeComponent
        public new string Text { get { return base.Text; } }


        public delegate void OnCycle(object sender);
        public event OnCycle Cycle;
        public CycleButton()
            : base()
        {
            Options = new CycleOptionCollection<T>();
            Options.CollectionChanged += (o, ea) => NormalizeWidth();
            Click += CycleButton_Click;
            FontChanged += CycleButton_FontChanged;
            SetText();
        }

        protected void SetText()
        {
            base.Text = SelectedLabel;
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                if (SelectedOption != null)
                {
                    FontStyle fs = SelectedOption.BoldLabel ? FontStyle.Bold : FontStyle.Regular;
                    base.Font = new Font(Font, fs);
                    ForeColor = SelectedOption.LabelColor == Color.Transparent ? Color.Black : SelectedOption.LabelColor;
                }
        }

        void CycleButton_Click(object sender, EventArgs e)
        {
            SelectedIndex++;
            if (Cycle != null) Cycle(this);
        }
        protected bool ValidIndex(int index)
        {
            return ValidateIndex(index) == index;
        }
        protected int? ValidateIndex(int index)
        {
            if (Options.Count == 0) return null;
            while (index >= Options.Count) index -= Options.Count;
            while (index < 0) index += Options.Count;
            return index;
        }

        void CycleButton_FontChanged(object sender, EventArgs e)
        {
            NormalizeWidth();
        }

        protected void NormalizeWidth()
        {
            if (this.AutoSize)
            {
                int longestWidth = 0;
                foreach (CycleOption<T> option in Options)
                    longestWidth = Math.Max(longestWidth, TextRenderer.MeasureText(option.Label, this.Font).Width);
                this.Width = longestWidth + 20;
            }
        }
    }
}
