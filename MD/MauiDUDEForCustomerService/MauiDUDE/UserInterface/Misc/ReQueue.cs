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
    public partial class ReQueue : Form
    {
        private bool FormCancelled { get; set; } = false;

        public ReQueueData Data
        {
            get
            {
                ReQueueData data = new ReQueueData();
                data.FormCancelled = FormCancelled;
                data.ReQueueDate = monthCalendar.SelectionStart;
                data.Comments = textBoxComments.Text;
                return data;
            }
        }

        public ReQueue()
        {
            InitializeComponent();
            //monthCalendar.SelectionStart = DateTime.Today;
            //monthCalendar.SelectionEnd = DateTime.Today;
        }

        private void buttonReQueue_Click(object sender, EventArgs e)
        {
            FormCancelled = false;
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            FormCancelled = true;
            textBoxComments.Clear();
            DialogResult = DialogResult.Cancel;
        }
    }
}
