using System;
using System.Windows.Forms;

namespace MassAssignRangeAssignment
{
    public partial class Queues : UserControl
    {
        public QueueNames Queue { get; set; }
        public RangeAssignment RA { get; set; }
        public bool Loaded { get; set; }

        public Queues(QueueNames queue, RangeAssignment ra)
        {
            InitializeComponent();
            Queue = queue;
            QueueNameBox.Text = queue.QueueName;
            FutureDatedBox.Checked = queue.FutureDated;
            RA = ra;
            Loaded = true;
        }

        private void QueueNameBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                if (RA.DeleteQueues.Contains(this))
                    RA.DeleteQueues.Remove(this);
                else
                {
                    RA.Delete.Enabled = true;
                    RA.DeleteQueues.Add(this);
                }
            }
            if (RA != null && RA.DeleteQueues.Count == 0)
                RA.Delete.Enabled = false;
        }

        private void FutureDatedBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Loaded)
            {
                if (RA.UpdateQueues.Contains(this))
                    RA.UpdateQueues.Remove(this);
                else
                {
                    this.Queue.FutureDated = FutureDatedBox.Checked;
                    RA.Update.Enabled = true;
                    RA.UpdateQueues.Add(this);
                }
            }
            if (RA != null && RA.UpdateQueues.Count == 0)
                RA.Update.Enabled = false;
        }
    }
}