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

namespace MauiDUDE
{
    public partial class GenericCallCategorization : Form
    {
        private CallCategorizationEntry entry = null;

        public GenericCallCategorization()
        {
            //This call is required by the windows form deisnger
            InitializeComponent();
        }

        public GenericCallCategorization(CallCategorizationEntry.CallCategory category)
        {
            InitializeComponent();
            
            //add any initialization after the InitializeComponent() call.
            //Based off what call category enum is passed in insert entry values
            if(category == CallCategorizationEntry.CallCategory.NoUHEAAConnection)
            {
                entry = new CallCategorizationEntry() { Category = "No UHEAA Connection"};
            }
            callCategorizationCommentOnly.SetEntry(entry);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DataAccess.DA.AddCallCategorizationRecord(callCategorizationCommentOnly.GetEntry());
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
