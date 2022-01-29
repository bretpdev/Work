using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace DPLTRS.MissingRAPInfo
{
    public partial class MissingRapForm : Form
    {
        public MissingRapForm()
        {
            InitializeComponent();
        }

        public RapInfo GetInfo()
        {
            RapInfo ri = new RapInfo();
            ri.AccountIdentifier = AccountBox.Text;
            ri.DueDate = DueDateBox.Text;
            ri.Item1 = Item1Box.Text;
            ri.Item2 = Item2Box.Text;
            ri.Item3 = Item3Box.Text;
            ri.Item4 = Item4Box.Text;
            ri.Item5 = Item5Box.Text;
            ri.Item6 = Item6Box.Text;
            ri.Item7 = Item7Box.Text;
            ri.Item8 = Item8Box.Text;
            return ri;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            //HACK Re factor the shit out of this when you have some time...
            if (Item1Box.Text.AreEqualAndNotEmpty(Item2Box.Text, Item3Box.Text, Item4Box.Text, Item5Box.Text, Item6Box.Text, Item7Box.Text, Item8Box.Text))
            {
                Dialog.Error.Ok("Item 1 is a duplicate of another item listed on the form.  Please review and try again");
                return;
            }
            else if (Item2Box.Text.AreEqualAndNotEmpty(Item1Box.Text, Item3Box.Text, Item4Box.Text, Item5Box.Text, Item6Box.Text, Item7Box.Text, Item8Box.Text))
            {
                Dialog.Error.Ok("Item 2 is a duplicate of another item listed on the form.  Please review and try again");
                return;
            }
            else if (Item3Box.Text.AreEqualAndNotEmpty(Item1Box.Text, Item2Box.Text, Item4Box.Text, Item5Box.Text, Item6Box.Text, Item7Box.Text, Item8Box.Text))
            {
                Dialog.Error.Ok("Item 3 is a duplicate of another item listed on the form.  Please review and try again");
                return;
            }
            else if (Item4Box.Text.AreEqualAndNotEmpty(Item1Box.Text, Item3Box.Text, Item2Box.Text, Item5Box.Text, Item6Box.Text, Item7Box.Text, Item8Box.Text))
            {
                Dialog.Error.Ok("Item 4 is a duplicate of another item listed on the form.  Please review and try again");
                return;
            }
            else if (Item5Box.Text.AreEqualAndNotEmpty(Item1Box.Text, Item3Box.Text, Item4Box.Text, Item2Box.Text, Item6Box.Text, Item7Box.Text, Item8Box.Text))
            {
                Dialog.Error.Ok("Item 5 is a duplicate of another item listed on the form.  Please review and try again");
                return;
            }
            else if (Item6Box.Text.AreEqualAndNotEmpty(Item1Box.Text, Item3Box.Text, Item4Box.Text, Item5Box.Text, Item2Box.Text, Item7Box.Text, Item8Box.Text))
            {
                Dialog.Error.Ok("Item 6 is a duplicate of another item listed on the form.  Please review and try again");
                return;
            }
            else if (Item7Box.Text.AreEqualAndNotEmpty(Item1Box.Text, Item3Box.Text, Item4Box.Text, Item5Box.Text, Item6Box.Text, Item2Box.Text, Item8Box.Text))
            {
                Dialog.Error.Ok("Item 7 is a duplicate of another item listed on the form.  Please review and try again");
                return;
            }
            else if (Item8Box.Text.AreEqualAndNotEmpty(Item1Box.Text, Item3Box.Text, Item4Box.Text, Item5Box.Text, Item6Box.Text, Item7Box.Text, Item2Box.Text))
            {
                Dialog.Error.Ok("Item 8 is a duplicate of another item listed on the form.  Please review and try again");
                return;
            }
            else
                DialogResult = DialogResult.OK;
        }
    }

    public class RapInfo
    {
        public string AccountIdentifier { get; set; }
        public string DueDate { get; set; }
        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public string Item3 { get; set; }
        public string Item4 { get; set; }
        public string Item5 { get; set; }
        public string Item6 { get; set; }
        public string Item7 { get; set; }
        public string Item8 { get; set; }
    }
}
