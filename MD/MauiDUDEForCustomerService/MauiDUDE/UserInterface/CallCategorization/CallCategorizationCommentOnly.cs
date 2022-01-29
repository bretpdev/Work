using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class CallCategorizationCommentOnly : UserControl
    {
        private CallCategorizationEntry entry { get; set; }

        public CallCategorizationCommentOnly()
        {
            InitializeComponent();
        }

        public void SetEntry(CallCategorizationEntry entry)
        {
            textBoxComments.Text = entry.Comments;
            this.entry = entry;
        }

        public CallCategorizationEntry GetEntry()
        {
            entry.Comments = textBoxComments.Text;
            return entry;
        }
    }
}
