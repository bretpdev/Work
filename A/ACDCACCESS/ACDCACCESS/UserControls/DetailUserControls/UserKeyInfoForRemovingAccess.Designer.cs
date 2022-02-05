namespace ACDCAccess
{
    partial class UserKeyInfoForRemovingAccess
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
			this.lblRemove = new System.Windows.Forms.Label();
			this.chkRemove = new System.Windows.Forms.CheckBox();
			this.lblBusinessUnit = new System.Windows.Forms.Label();
			this.userAccessKeyBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label2 = new System.Windows.Forms.Label();
			this.lblKey = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.userAccessKeyBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// lblRemove
			// 
			this.lblRemove.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblRemove.Location = new System.Drawing.Point(3, 1);
			this.lblRemove.Name = "lblRemove";
			this.lblRemove.Size = new System.Drawing.Size(111, 18);
			this.lblRemove.TabIndex = 16;
			this.lblRemove.Text = "Remove";
			this.lblRemove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkRemove
			// 
			this.chkRemove.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
			this.chkRemove.Location = new System.Drawing.Point(3, 1);
			this.chkRemove.Name = "chkRemove";
			this.chkRemove.Size = new System.Drawing.Size(16, 16);
			this.chkRemove.TabIndex = 15;
			this.chkRemove.UseVisualStyleBackColor = true;
			// 
			// lblBusinessUnit
			// 
			this.lblBusinessUnit.AutoSize = true;
			this.lblBusinessUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblBusinessUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userAccessKeyBindingSource, "BusinessUnit", true));
			this.lblBusinessUnit.Location = new System.Drawing.Point(389, 1);
			this.lblBusinessUnit.MaximumSize = new System.Drawing.Size(131, 2);
			this.lblBusinessUnit.MinimumSize = new System.Drawing.Size(131, 18);
			this.lblBusinessUnit.Name = "lblBusinessUnit";
			this.lblBusinessUnit.Size = new System.Drawing.Size(131, 18);
			this.lblBusinessUnit.TabIndex = 14;
			this.lblBusinessUnit.Text = "Business Unit";
			this.lblBusinessUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// userAccessKeyBindingSource
			// 
			this.userAccessKeyBindingSource.DataSource = typeof(ACDCAccess.UserAccessKey);
			// 
			// label2
			// 
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userAccessKeyBindingSource, "Application", true));
			this.label2.Location = new System.Drawing.Point(273, 1);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 18);
			this.label2.TabIndex = 13;
			this.label2.Text = "Application";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblKey
			// 
			this.lblKey.AutoSize = true;
			this.lblKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userAccessKeyBindingSource, "Name", true));
			this.lblKey.Location = new System.Drawing.Point(120, 1);
			this.lblKey.MaximumSize = new System.Drawing.Size(147, 2);
			this.lblKey.MinimumSize = new System.Drawing.Size(147, 18);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(147, 18);
			this.lblKey.TabIndex = 12;
			this.lblKey.Text = "Key";
			this.lblKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userAccessKeyBindingSource, "Description", true));
			this.lblDescription.Location = new System.Drawing.Point(615, 1);
			this.lblDescription.MaximumSize = new System.Drawing.Size(222, 2);
			this.lblDescription.MinimumSize = new System.Drawing.Size(222, 18);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(222, 18);
			this.lblDescription.TabIndex = 18;
			this.lblDescription.Text = "Description";
			this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userAccessKeyBindingSource, "Type", true));
			this.label3.Location = new System.Drawing.Point(526, 1);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(83, 18);
			this.label3.TabIndex = 17;
			this.label3.Text = "Type";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// UserKeyInfoForRemovingAccess
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblRemove);
			this.Controls.Add(this.chkRemove);
			this.Controls.Add(this.lblBusinessUnit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblKey);
			this.MaximumSize = new System.Drawing.Size(840, 0);
			this.MinimumSize = new System.Drawing.Size(840, 20);
			this.Name = "UserKeyInfoForRemovingAccess";
			this.Size = new System.Drawing.Size(840, 20);
			((System.ComponentModel.ISupportInitialize)(this.userAccessKeyBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label lblRemove;
        private System.Windows.Forms.CheckBox chkRemove;
        protected System.Windows.Forms.Label lblBusinessUnit;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label lblKey;
        protected System.Windows.Forms.Label lblDescription;
        protected System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource userAccessKeyBindingSource;
    }
}
