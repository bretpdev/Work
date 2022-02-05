using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.WinForms;

namespace MDIntermediary
{
    class PhoneBox : NumericMaskedTextBox
    {
        public bool IsForeign
        {
            get { return base.Mask == foreignMask; }
            set
            {
                if (value)
                    base.Mask = foreignMask;
                else
                    base.Mask = domesticMask;
            }
        }
        //0 = 0-9
        //9 = 0-9, or space
        string domesticMask = "000-000-0000x9999";
        string foreignMask = "000-00000-00000000000";
        public PhoneBox()
        {
            base.Mask = domesticMask;
            this.Enter += (o, ea) => this.BeginInvoke(new Action(() => this.SelectionStart = 0));
            string oldValue = "";
            this.TextChanged += (o, ea) =>
            {
                string newValue = this.Text;
                oldValue = newValue;
            };
        }
        [ReadOnly(true)]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string Mask { get { return base.Mask; } }

        private string Trim(string s)
        {
            return s.Replace("-", "").Replace("x", "").Replace("_", "").Replace(" ", "");
        }
        public string PhoneNumber
        {
            get
            {
                if (IsForeign)
                    return "";
                    //return Text.Replace("-", "").Trim();
                string number = Trim(Text.Split('x')[0]);
                return number;
            }
            set
            {
                if (!IsForeign || !string.IsNullOrWhiteSpace(value))
                {
                    IsForeign = false;
                    string val = value.PadRight(9, ' ');
                    val = val.Insert(3, "-").Insert(7 + 1, "-");
                    this.Text = val + "x" + Extension;
                }
            }
        }
        public string Extension
        {
            get
            {
                if (IsForeign)
                    return "";
                var split = Text.Split('x');
                if (split.Count() > 1)
                    return Trim(split[1]);
                return "";
            }
            set
            {
                if (!IsForeign || !string.IsNullOrWhiteSpace(value))
                {
                    IsForeign = false;
                    this.Text = PhoneNumber + "x" + value;
                }
            }
        }

        public string ForeignCountry
        {
            get
            {
                if (!IsForeign)
                    return "";
                return this.Text.Substring(0, 3);
            }
            set
            {
                if (IsForeign || !string.IsNullOrWhiteSpace(value))
                {
                    this.IsForeign = true;
                    string text = this.Text;
                    this.Text = (value ?? "").PadRight(3) + text.Substring(3);
                }
            }
        }

        public string ForeignCity
        {
            get
            {
                if (Mask != foreignMask)
                    return "";
                return this.Text.Substring(4, 5);
            }
            set
            {
                if (IsForeign || !string.IsNullOrWhiteSpace(value))
                {
                    this.IsForeign = true;
                    string text = this.Text;
                    this.Text = text.Substring(0, 4) + (value ?? "").PadRight(5) + text.Substring(9);
                }
            }
        }

        public string ForeignLocal
        {
            get
            {
                if (Mask != foreignMask)
                    return "";
                return this.Text.Substring(10);
            }
            set
            {
                if (IsForeign || !string.IsNullOrWhiteSpace(value))
                {
                    this.IsForeign = true;
                    this.Text = this.Text.Substring(0, 10) + value;
                }
            }
        }
    }
}
