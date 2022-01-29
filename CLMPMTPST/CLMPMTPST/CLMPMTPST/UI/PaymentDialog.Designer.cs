namespace CLMPMTPST
{
	partial class PaymentDialog
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtAmount = new System.Windows.Forms.TextBox();
			this.totalBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblWire = new System.Windows.Forms.Label();
			this.radWire = new System.Windows.Forms.RadioButton();
			this.radCash = new System.Windows.Forms.RadioButton();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.totalBindingSource)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(256, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter the total amount of the payments being posted.";
			// 
			// txtAmount
			// 
			this.txtAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.totalBindingSource, "Amount", true));
			this.txtAmount.Location = new System.Drawing.Point(74, 38);
			this.txtAmount.Name = "txtAmount";
			this.txtAmount.Size = new System.Drawing.Size(135, 20);
			this.txtAmount.TabIndex = 1;
			// 
			// totalBindingSource
			// 
			this.totalBindingSource.DataSource = typeof(CLMPMTPST.PaymentTotal);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblWire);
			this.groupBox1.Controls.Add(this.radWire);
			this.groupBox1.Controls.Add(this.radCash);
			this.groupBox1.Location = new System.Drawing.Point(16, 74);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(253, 47);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Payment Type";
			// 
			// lblWire
			// 
			this.lblWire.AutoSize = true;
			this.lblWire.Font = new System.Drawing.Font("Wingdings", 18F);
			this.lblWire.Location = new System.Drawing.Point(209, 16);
			this.lblWire.Name = "lblWire";
			this.lblWire.Size = new System.Drawing.Size(38, 26);
			this.lblWire.TabIndex = 2;
			this.lblWire.Text = "h";
			// 
			// radWire
			// 
			this.radWire.AutoSize = true;
			this.radWire.Location = new System.Drawing.Point(146, 19);
			this.radWire.Name = "radWire";
			this.radWire.Size = new System.Drawing.Size(47, 17);
			this.radWire.TabIndex = 1;
			this.radWire.TabStop = true;
			this.radWire.Text = "Wire";
			this.radWire.UseVisualStyleBackColor = true;
			this.radWire.CheckedChanged += new System.EventHandler(this.Wire_CheckedChanged);
			// 
			// radCash
			// 
			this.radCash.AutoSize = true;
			this.radCash.Location = new System.Drawing.Point(62, 19);
			this.radCash.Name = "radCash";
			this.radCash.Size = new System.Drawing.Size(49, 17);
			this.radCash.TabIndex = 0;
			this.radCash.TabStop = true;
			this.radCash.Text = "Cash";
			this.radCash.UseVisualStyleBackColor = true;
			this.radCash.CheckedChanged += new System.EventHandler(this.Cash_CheckedChanged);
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(27, 136);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(100, 30);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(156, 136);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 30);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// PaymentDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(283, 178);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtAmount);
			this.Controls.Add(this.label1);
			this.Name = "PaymentDialog";
			this.Text = "Total Claim Payments";
			((System.ComponentModel.ISupportInitialize)(this.totalBindingSource)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtAmount;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radWire;
		private System.Windows.Forms.RadioButton radCash;
		private System.Windows.Forms.Label lblWire;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource totalBindingSource;
	}
}