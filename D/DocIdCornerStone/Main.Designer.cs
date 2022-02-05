namespace DocIdCornerStone
{
	partial class Main
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
			this.txtPersonId = new System.Windows.Forms.TextBox();
			this.userInputBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btnProcess = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbDescription = new System.Windows.Forms.ComboBox();
			this.cmbSource = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.userInputBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// txtPersonId
			// 
			this.txtPersonId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userInputBindingSource, "AccountId", true));
			this.txtPersonId.Location = new System.Drawing.Point(165, 66);
			this.txtPersonId.MaxLength = 10;
			this.txtPersonId.Name = "txtPersonId";
			this.txtPersonId.Size = new System.Drawing.Size(115, 20);
			this.txtPersonId.TabIndex = 0;
			// 
			// userInputBindingSource
			// 
			this.userInputBindingSource.DataSource = typeof(DocIdCornerStone.UserInput);
			// 
			// btnProcess
			// 
			this.btnProcess.Location = new System.Drawing.Point(103, 101);
			this.btnProcess.Name = "btnProcess";
			this.btnProcess.Size = new System.Drawing.Size(100, 30);
			this.btnProcess.TabIndex = 1;
			this.btnProcess.Text = "Process";
			this.btnProcess.UseVisualStyleBackColor = true;
			this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(218, 101);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 30);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 69);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(147, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "SSN/AccountNumber/Ref ID";
			// 
			// cmbDescription
			// 
			this.cmbDescription.FormattingEnabled = true;
			this.cmbDescription.Location = new System.Drawing.Point(117, 12);
			this.cmbDescription.Name = "cmbDescription";
			this.cmbDescription.Size = new System.Drawing.Size(288, 21);
			this.cmbDescription.TabIndex = 4;
			// 
			// cmbSource
			// 
			this.cmbSource.FormattingEnabled = true;
			this.cmbSource.Location = new System.Drawing.Point(165, 39);
			this.cmbSource.Name = "cmbSource";
			this.cmbSource.Size = new System.Drawing.Size(115, 21);
			this.cmbSource.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(99, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Doc ID/Description";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Source";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(420, 143);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbSource);
			this.Controls.Add(this.cmbDescription);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnProcess);
			this.Controls.Add(this.txtPersonId);
			this.Name = "Main";
			this.Text = "CornerStone Doc ID";
			((System.ComponentModel.ISupportInitialize)(this.userInputBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtPersonId;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbDescription;
		private System.Windows.Forms.ComboBox cmbSource;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.BindingSource userInputBindingSource;
	}
}

