using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uheaa.Common.WinForms
{
    public class YesNoComboBox : ComboBox
    {
        string yes = "Yes";
        string no = "No";
        public YesNoComboBox()
        {
            base.Items.Add("");
            base.Items.Add(yes);
            base.Items.Add(no);
            DropDownStyle = ComboBoxStyle.DropDownList;
            this.Items = new ObjectCollection(this);
        }

        public new bool? SelectedValue
        {
            get
            {
                if (SelectedIndex <= 0)
                    return null;
                return (string)SelectedItem == yes;
            }
            set
            {
                var changed = SelectedValue != value;
                if (changed)
                {
                    if (value == null)
                        SelectedIndex = 0;
                    else if (value.Value)
                        SelectedItem = yes;
                    else
                        SelectedItem = no;
                    OnSelectedItemChanged(new EventArgs());
                }
            }
        }

        [Browsable(false)] //users should not be able to manipulate the list of items
        public new ObjectCollection Items { get; set; }
    }
}
