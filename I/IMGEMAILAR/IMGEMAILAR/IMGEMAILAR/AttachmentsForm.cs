using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMGEMAILAR
{
    public partial class AttachmentsForm : Form
    {
        public AttachmentsForm(BindingList<string> attachments)
        {
            InitializeComponent();
            Attachments = attachments;
            AttachmentsList.DataSource = Attachments;
        }

        private BindingList<string> Attachments;

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog() { Multiselect = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in ofd.FileNames)
                        Attachments.Add(file);
                }
            }
            AttachmentsList.SelectedIndex = -1;
        }

        private void AttachmentsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveButton.Enabled = AttachmentsList.SelectedIndex >= 0;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            Attachments.RemoveAt(AttachmentsList.SelectedIndex);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
