using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIntermediary.Demographics
{
    public partial class AcpRecipientForm : Form
    {
        private AcpRecipientInfo info;
        public AcpRecipientForm(AcpRecipientInfo info)
        {
            InitializeComponent();
            this.info = info;
        }

        public bool AuthorizedEnabled { get { return this.AuthorizedButton.Enabled; } set { this.AuthorizedButton.Enabled = value; } }
        public bool RelationshipEnabled { get { return this.RelationshipBox.Enabled; } set { this.RelationshipBox.Enabled = value; } }

        private void AcpRecipientForm_Shown(object sender, EventArgs e)
        {
            this.ContactPhoneBox.Text = info.ContactPhoneNumber;
            this.RecipientNameBox.Text = info.RecipientName;
            this.RelationshipBox.SelectedItem = info.Relationship;
            if (this.RelationshipBox.SelectedItem == null && info.Relationship != null)
            {
                this.RelationshipBox.Items.Add(info.Relationship);
                this.RelationshipBox.SelectedItem = info.Relationship;
            }
            this.AuthorizedButton.SelectedValue = info.Authorized;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            info.ContactPhoneNumber = this.ContactPhoneBox.Text;
            info.RecipientName = this.RecipientNameBox.Text;
            info.Relationship = (string)this.RelationshipBox.SelectedItem;
            info.Authorized = this.AuthorizedButton.SelectedValue;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// Make the buttons on this form purple.
        /// </summary>
        public void Purplify()
        {
            Color purple = Color.FromArgb(184, 174, 231);
            this.BackColor = purple;
            OkButton.BackColor = CancelButton.BackColor = purple;
        }
    }
}
