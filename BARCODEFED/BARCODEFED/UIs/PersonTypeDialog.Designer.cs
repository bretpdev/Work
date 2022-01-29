namespace BARCODEFED
{
	partial class PersonTypeDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.radBorrower = new System.Windows.Forms.RadioButton();
            this.radEndorser = new System.Windows.Forms.RadioButton();
            this.radReference = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 77);
            this.label1.TabIndex = 0;
            this.label1.Text = "Is the return mail for a borrower, endorser, or reference?";
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnContinue.Location = new System.Drawing.Point(18, 238);
            this.btnContinue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(112, 35);
            this.btnContinue.TabIndex = 3;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // radBorrower
            // 
            this.radBorrower.AutoSize = true;
            this.radBorrower.Checked = true;
            this.radBorrower.Location = new System.Drawing.Point(26, 95);
            this.radBorrower.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radBorrower.Name = "radBorrower";
            this.radBorrower.Size = new System.Drawing.Size(91, 24);
            this.radBorrower.TabIndex = 0;
            this.radBorrower.TabStop = true;
            this.radBorrower.Text = "Borrower";
            this.radBorrower.UseVisualStyleBackColor = true;
            // 
            // radEndorser
            // 
            this.radEndorser.AutoSize = true;
            this.radEndorser.Location = new System.Drawing.Point(26, 131);
            this.radEndorser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radEndorser.Name = "radEndorser";
            this.radEndorser.Size = new System.Drawing.Size(92, 24);
            this.radEndorser.TabIndex = 1;
            this.radEndorser.Text = "Endorser";
            this.radEndorser.UseVisualStyleBackColor = true;
            // 
            // radReference
            // 
            this.radReference.AutoSize = true;
            this.radReference.Location = new System.Drawing.Point(26, 166);
            this.radReference.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radReference.Name = "radReference";
            this.radReference.Size = new System.Drawing.Size(102, 24);
            this.radReference.TabIndex = 2;
            this.radReference.Text = "Reference";
            this.radReference.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(148, 238);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PersonTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 292);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.radReference);
            this.Controls.Add(this.radEndorser);
            this.Controls.Add(this.radBorrower);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(295, 330);
            this.Name = "PersonTypeDialog";
            this.Text = "Person Type";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.RadioButton radBorrower;
		private System.Windows.Forms.RadioButton radEndorser;
		private System.Windows.Forms.RadioButton radReference;
		private System.Windows.Forms.Button btnCancel;
	}
}