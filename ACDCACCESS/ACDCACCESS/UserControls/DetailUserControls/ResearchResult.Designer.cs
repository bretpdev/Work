namespace ACDCAccess
{
    partial class ResearchResult
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
			this.lblKey = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.keyBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.keyBindingSource, "Description", true));
			this.lblDescription.Location = new System.Drawing.Point(439, 1);
			this.lblDescription.MaximumSize = new System.Drawing.Size(398, 2);
			this.lblDescription.MinimumSize = new System.Drawing.Size(398, 18);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(398, 18);
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
			this.label4.Location = new System.Drawing.Point(322, 1);
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
			this.label2.Location = new System.Drawing.Point(205, 1);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 18);
			this.label2.TabIndex = 6;
			this.label2.Text = "Application";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblKey
			// 
			this.lblKey.AutoSize = true;
			this.lblKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.keyBindingSource, "Name", true));
			this.lblKey.Location = new System.Drawing.Point(3, 1);
			this.lblKey.MaximumSize = new System.Drawing.Size(196, 2);
			this.lblKey.MinimumSize = new System.Drawing.Size(196, 18);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(196, 18);
			this.lblKey.TabIndex = 5;
			this.lblKey.Text = "Key";
			this.lblKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ResearchResult
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblKey);
			this.MaximumSize = new System.Drawing.Size(840, 0);
			this.MinimumSize = new System.Drawing.Size(840, 20);
			this.Name = "ResearchResult";
			this.Size = new System.Drawing.Size(840, 20);
			((System.ComponentModel.ISupportInitialize)(this.keyBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label lblDescription;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.BindingSource keyBindingSource;
    }
}
