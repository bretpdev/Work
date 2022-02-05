using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDIntermediary;
using Uheaa.Common.DataAccess;

namespace MauiDUDE
{
    public partial class ReferenceControl : UserControl
    {
        public Reference Reference { get; set; }

        public ReferenceControl()
        {
            InitializeComponent();
        }

        public ReferenceControl(Reference reference)
        {
            InitializeComponent();

            referenceBindingSource.DataSource = reference;
            Reference = reference;
            buttonViewHistory.Visible = true;
        }

        private void buttonViewHistory_Click(object sender, EventArgs e)
        {
            Processing.MakeVisible();
            ((UheaaHomePage)ParentForm).activityCommentsReference.PopulateComments(ActivityCommentGatherer.DaysOrNumberOf.Days, 0, true, Reference.ReferenceId, ActivityComments.AESSystem.OneLINK, DataAccessHelper.Region.Uheaa);
            Processing.MakeInvisible();
            ParentForm.Activate();
        }
    }
}
