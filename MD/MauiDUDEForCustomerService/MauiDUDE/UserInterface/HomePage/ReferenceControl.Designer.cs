namespace MauiDUDE
{
    partial class ReferenceControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.referenceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.labelRelationship = new System.Windows.Forms.Label();
            this.labelThirdPartAuth = new System.Windows.Forms.Label();
            this.labelLastContact = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonViewHistory = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.referenceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.referenceBindingSource, "FullName", true));
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Full Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // referenceBindingSource
            // 
            this.referenceBindingSource.DataSource = typeof(MDIntermediary.Reference);
            // 
            // labelRelationship
            // 
            this.labelRelationship.BackColor = System.Drawing.Color.LightGray;
            this.labelRelationship.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.referenceBindingSource, "RelationshipToBorrower", true));
            this.labelRelationship.Location = new System.Drawing.Point(285, 1);
            this.labelRelationship.Name = "labelRelationship";
            this.labelRelationship.Size = new System.Drawing.Size(73, 31);
            this.labelRelationship.TabIndex = 1;
            this.labelRelationship.Text = "Relationship";
            this.labelRelationship.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelThirdPartAuth
            // 
            this.labelThirdPartAuth.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.referenceBindingSource, "AuthorizedThirdPartyIndicator", true));
            this.labelThirdPartAuth.Location = new System.Drawing.Point(359, 1);
            this.labelThirdPartAuth.Name = "labelThirdPartAuth";
            this.labelThirdPartAuth.Size = new System.Drawing.Size(82, 31);
            this.labelThirdPartAuth.TabIndex = 2;
            this.labelThirdPartAuth.Text = "3rd Party Auth.";
            this.labelThirdPartAuth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelLastContact
            // 
            this.labelLastContact.BackColor = System.Drawing.Color.LightGray;
            this.labelLastContact.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.referenceBindingSource, "LastContact", true));
            this.labelLastContact.Location = new System.Drawing.Point(447, 1);
            this.labelLastContact.Name = "labelLastContact";
            this.labelLastContact.Size = new System.Drawing.Size(73, 31);
            this.labelLastContact.TabIndex = 3;
            this.labelLastContact.Text = "Last Contact";
            this.labelLastContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.referenceBindingSource, "LastAttempt", true));
            this.label5.Location = new System.Drawing.Point(526, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 31);
            this.label5.TabIndex = 4;
            this.label5.Text = "Last Attempt";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonViewHistory
            // 
            this.buttonViewHistory.Location = new System.Drawing.Point(614, 4);
            this.buttonViewHistory.Name = "buttonViewHistory";
            this.buttonViewHistory.Size = new System.Drawing.Size(124, 23);
            this.buttonViewHistory.TabIndex = 5;
            this.buttonViewHistory.Text = "View History Below";
            this.buttonViewHistory.UseVisualStyleBackColor = true;
            this.buttonViewHistory.Visible = false;
            this.buttonViewHistory.Click += new System.EventHandler(this.buttonViewHistory_Click);
            // 
            // ReferenceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonViewHistory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelLastContact);
            this.Controls.Add(this.labelThirdPartAuth);
            this.Controls.Add(this.labelRelationship);
            this.Controls.Add(this.label1);
            this.Name = "ReferenceControl";
            this.Size = new System.Drawing.Size(741, 32);
            ((System.ComponentModel.ISupportInitialize)(this.referenceBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelRelationship;
        private System.Windows.Forms.Label labelThirdPartAuth;
        private System.Windows.Forms.Label labelLastContact;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonViewHistory;
        private System.Windows.Forms.BindingSource referenceBindingSource;
    }
}
