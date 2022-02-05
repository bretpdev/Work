namespace SpecialEmailCampaignFed
{
	partial class TestAcctNum
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
			this.components = new System.ComponentModel.Container();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtAcctNumField = new System.Windows.Forms.TextBox();
			this.lblTestAcct = new System.Windows.Forms.Label();
			this.borrowerDetailsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.borrowerDetailsBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(12, 76);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(65, 25);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(78, 76);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(65, 25);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtAcctNumField
			// 
			this.txtAcctNumField.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borrowerDetailsBindingSource, "AccountNumber", true));
			this.txtAcctNumField.Location = new System.Drawing.Point(12, 38);
			this.txtAcctNumField.Name = "txtAcctNumField";
			this.txtAcctNumField.Size = new System.Drawing.Size(131, 20);
			this.txtAcctNumField.TabIndex = 2;
			// 
			// lblTestAcct
			// 
			this.lblTestAcct.AutoSize = true;
			this.lblTestAcct.Location = new System.Drawing.Point(9, 0);
			this.lblTestAcct.MaximumSize = new System.Drawing.Size(150, 0);
			this.lblTestAcct.Name = "lblTestAcct";
			this.lblTestAcct.Size = new System.Drawing.Size(140, 26);
			this.lblTestAcct.TabIndex = 3;
			this.lblTestAcct.Text = "Please enter a test account number for processing.";
			// 
			// borrowerDetailsBindingSource
			// 
			this.borrowerDetailsBindingSource.DataSource = typeof(SpecialEmailCampaignFed.BorrowerDetails);
			// 
			// TestAcctNum
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(154, 112);
			this.Controls.Add(this.lblTestAcct);
			this.Controls.Add(this.txtAcctNumField);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Name = "TestAcctNum";
			((System.ComponentModel.ISupportInitialize)(this.borrowerDetailsBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtAcctNumField;
		private System.Windows.Forms.Label lblTestAcct;
		private System.Windows.Forms.BindingSource borrowerDetailsBindingSource;
	}
}