using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.WinForms;

namespace MDIntermediary
{
    class YNCheckButton : CheckButton
    {
        public YNCheckButton()
        {
            this.Click += (o, ea) =>
            {
                this.OnTextChanged(new EventArgs());
            };
        }
        public string IsCheckedYN
        {
            get { return IsChecked ? "Y" : "N"; }
            set { IsChecked = value == "Y"; }
        }
    }
}
