using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIntermediary.Calls
{
    public partial class CallCategorizationControl : UserControl
    {
        public CallCategorizationControl()
        {
            InitializeComponent();
        }

        CallsHelper helper;
        public void LoadHelper(CallsHelper helper)
        {
            this.helper = helper;
            CategoryBox.DataSource = helper.GetCategories();
        }

        public CallRecord GenerateCallRecord()
        {
            var reason = (ReasonBox.SelectedItem as CallReason);
            if (reason == null || reason == CallReason.NoReason)
                return null;
            return new CallRecord()
            {
                Comments = CommentsBox.Text,
                LetterId = LetterIdBox.Text,
                IsCornerstone = false,
                IsOutbound = !helper.IncomingCall,
                ReasonId = reason.ReasonId
            };
        }

        public bool RecordCall()
        {
            var record = GenerateCallRecord();
            if (record == null)
                return false;
            CallRecord.Insert(record);
            return true;
        }

        private void CategoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (helper != null)
            {
                var reasons = helper.GetReasons(CategoryBox.SelectedItem.ToString());
                ReasonBox.DataSource = reasons;
            }
            ReasonBox.Enabled = LetterIdBox.Enabled = CommentsBox.Enabled = (helper != null);
        }
    }
}
