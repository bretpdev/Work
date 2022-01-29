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
using Uheaa.Common.DataAccess;

namespace MauiDUDE
{
    public partial class ActivityHistory : Form
    {
        private Borrower Borrower { get; set; }
        public ActivityHistory()
        {
            InitializeComponent();
        }

        public ActivityHistory(Borrower borrower)
        {
            InitializeComponent();
            Borrower = borrower;
        }

        public bool Show(int days, string title, ActivityComments.AESSystem system, DataAccessHelper.Region region)
        {
            Text = title;
            labelTitle.Text = title;
            activityComments.PopulateComments(ActivityCommentGatherer.DaysOrNumberOf.Days, days, true, Borrower.SSN, system, region);
            if(activityComments.Success)
            {
                Show();
                return true;
            }
            return false;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }
    }
}
