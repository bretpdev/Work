using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;

namespace TRDPRTYRES
{
    public partial class PossibleMatchRefSelection : Form
    {
        public References Rdata { get; set; }
        public List<References> RefData { get; set; }

        public PossibleMatchRefSelection(List<References> refData)
        {
            InitializeComponent();
            RefData = refData;
            foreach (References item in refData)
                References.Items.Add(item.ReferenceId).SubItems.Add(item.ReferenceName);
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            if (References.SelectedIndices.Count > 0)
            {
                Rdata = RefData[References.SelectedIndices[0]];
                DialogResult = DialogResult.OK;
            }
            else
                Dialog.Warning.Ok("You must select a existing reference or select new reference.  Please try again.", "Error");
        }
    }
}