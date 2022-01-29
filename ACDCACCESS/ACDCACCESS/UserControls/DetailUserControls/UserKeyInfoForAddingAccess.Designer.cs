namespace ACDCAccess
{
    partial class UserKeyInfoForAddingAccess
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
			this.lblDescription = new System.Windows.Forms.Label();
			this.keyBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.chkAdd = new System.Windows.Forms.CheckBox();
			this.lblAdd = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.keyBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.keyBindingSource, "Description", true));
			this.lblDescription.Location = new System.Drawing.Point(555, 1);
			this.lblDescription.MaximumSize = new System.Drawing.Size(283, 2);
			this.lblDescription.MinimumSize = new System.Drawing.Size(283, 18);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(283, 18);
			this.lblDescription.TabIndex = 9;
			this.lblDescription.Text = "Description";
			this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// keyBindingSource
			// 
			this.keyBindingSource.DataSource = typeof(ACDCAccess.Key);
			// 
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.keyBindingSource, "Type", true));
			this.label4.Location = new System.Drawing.Point(439, 1);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(111, 18);
			this.label4.TabIndex = 8;
			this.label4.Text = "Type";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.keyBindingSource, "Application", true));
			this.label2.Location = new System.Drawing.Point(323, 1);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 18);
			this.label2.TabIndex = 6;
			this.label2.Text = "Application";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.keyBindingSource, "Name", true));
			this.label1.Location = new System.Drawing.Point(121, 1);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(196, 18);
			this.label1.TabIndex = 5;
			this.label1.Text = "Key";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkAdd
			// 
			this.chkAdd.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
			this.chkAdd.Location = new System.Drawing.Point(4, 1);
			this.chkAdd.Name = "chkAdd";
			this.chkAdd.Size = new System.Drawing.Size(16, 16);
			this.chkAdd.TabIndex = 10;
			this.chkAdd.UseVisualStyleBackColor = true;
			// 
			// lblAdd
			// 
			this.lblAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblAdd.Location = new System.Drawing.Point(4, 1);
			this.lblAdd.Name = "lblAdd";
			this.lblAdd.Size = new System.Drawing.Size(111, 18);
			this.lblAdd.TabIndex = 11;
			this.lblAdd.Text = "Add";
			this.lblAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// UserKeyInfoForAddingAccess
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.lblAdd);
			this.Controls.Add(this.chkAdd);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.MaximumSize = new System.Drawing.Size(840, 0);
			this.MinimumSize = new System.Drawing.Size(840, 20);
			this.Name = "UserKeyInfoForAddingAccess";
			this.Size = new System.Drawing.Size(840, 20);
			((System.ComponentModel.ISupportInitialize)(this.keyBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label lblDescription;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkAdd;
        protected System.Windows.Forms.Label lblAdd;
        private System.Windows.Forms.BindingSource keyBindingSource;
    }
}
