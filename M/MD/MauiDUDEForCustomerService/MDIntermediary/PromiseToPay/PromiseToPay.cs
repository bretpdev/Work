using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace MDIntermediary.PromiseToPay
{
    public partial class PromiseToPay : Form
    {

        public int DaysToExclude;

        public PromiseToPay()
        {
            InitializeComponent();
            AddSelectionBoxEntries();
        }

        public bool ValidateInput()
        {
            if(SelectionBox.SelectedIndex < 0 || !GetSelectionBoxEntries().Contains(SelectionBox.Items[SelectionBox.SelectedIndex].ToString()))
            {
                return false;
            }
            else
            {
                int days;
                bool success = int.TryParse(SelectionBox.Items[SelectionBox.SelectedIndex].ToString().Substring(0, 2).TrimEnd(), out days);
                if(!success)
                {
                    return false;
                }
                DaysToExclude = days;
            }
            return true;
        }

        public void AddSelectionBoxEntries()
        {
            foreach(string str in GetSelectionBoxEntries())
            {
                SelectionBox.Items.Add(str);
            }
        }

        public List<string> GetSelectionBoxEntries()
        {
            List<string> options = new List<string>();
            options.Add("1 Day");
            options.Add("2 Days");
            options.Add("3 Days");
            options.Add("4 Days");
            options.Add("5 Days");
            options.Add("6 Days");
            options.Add("7 Days");
            options.Add("8 Days");
            options.Add("9 Days");
            options.Add("10 Days");
            options.Add("11 Days");
            options.Add("12 Days");
            options.Add("13 Days");
            options.Add("14 Days");
            return options;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if(ValidateInput())
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                Dialog.Error.Ok("Please make a selection before continuing.");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
