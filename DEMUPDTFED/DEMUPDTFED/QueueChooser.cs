using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DEMUPDTFED
{
    public partial class QueueChooser : Form
    {
        public List<string> SelectedQueues { get; set; }

        public QueueChooser(IEnumerable<string> queues)
        {
            InitializeComponent();
            SelectedQueues = new List<string>();
            Queues.DataSource = queues;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            foreach (var row in Queues.CheckedItems)
            {
                SelectedQueues.Add(row.ToString());
            }

            DialogResult = DialogResult.OK;
        }
    }
}