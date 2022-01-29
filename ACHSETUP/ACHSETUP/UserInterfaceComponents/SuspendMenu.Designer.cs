namespace ACHSETUP
{
	partial class SuspendMenu
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radStaffRequest = new System.Windows.Forms.RadioButton();
            this.radBorrowerRequest = new System.Windows.Forms.RadioButton();
            this.txtNumberOfBills = new System.Windows.Forms.TextBox();
            this.suspendOptionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.suspendOptionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radStaffRequest);
            this.groupBox1.Controls.Add(this.radBorrowerRequest);
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(268, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Suspend Options";
            // 
            // radStaffRequest
            // 
            this.radStaffRequest.AutoSize = true;
            this.radStaffRequest.Location = new System.Drawing.Point(10, 66);
            this.radStaffRequest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radStaffRequest.Name = "radStaffRequest";
            this.radStaffRequest.Size = new System.Drawing.Size(127, 24);
            this.radStaffRequest.TabIndex = 1;
            this.radStaffRequest.TabStop = true;
            this.radStaffRequest.Text = "Staff Request";
            this.radStaffRequest.UseVisualStyleBackColor = true;
            // 
            // radBorrowerRequest
            // 
            this.radBorrowerRequest.AutoSize = true;
            this.radBorrowerRequest.Location = new System.Drawing.Point(10, 31);
            this.radBorrowerRequest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radBorrowerRequest.Name = "radBorrowerRequest";
            this.radBorrowerRequest.Size = new System.Drawing.Size(156, 24);
            this.radBorrowerRequest.TabIndex = 0;
            this.radBorrowerRequest.TabStop = true;
            this.radBorrowerRequest.Text = "Borrower Request";
            this.radBorrowerRequest.UseVisualStyleBackColor = true;
            // 
            // txtNumberOfBills
            // 
            this.txtNumberOfBills.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suspendOptionsBindingSource, "NumberOfBills", true));
            this.txtNumberOfBills.Location = new System.Drawing.Point(228, 131);
            this.txtNumberOfBills.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNumberOfBills.Name = "txtNumberOfBills";
            this.txtNumberOfBills.Size = new System.Drawing.Size(58, 26);
            this.txtNumberOfBills.TabIndex = 1;
            // 
            // suspendOptionsBindingSource
            // 
            this.suspendOptionsBindingSource.DataSource = typeof(ACHSETUP.SuspendOptions);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 135);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of Bills to Suspend";
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OK.Location = new System.Drawing.Point(76, 185);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(150, 46);
            this.OK.TabIndex = 3;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(76, 240);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(150, 46);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // SuspendMenu
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(304, 300);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNumberOfBills);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(322, 341);
            this.MinimumSize = new System.Drawing.Size(320, 339);
            this.Name = "SuspendMenu";
            this.Text = "Suspend ACH";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.suspendOptionsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radStaffRequest;
		private System.Windows.Forms.RadioButton radBorrowerRequest;
		private System.Windows.Forms.TextBox txtNumberOfBills;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.BindingSource suspendOptionsBindingSource;
	}
}