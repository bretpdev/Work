namespace ACHSETUPFD
{
	partial class MainMenu
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radMissingInfo = new System.Windows.Forms.RadioButton();
            this.radSuspend = new System.Windows.Forms.RadioButton();
            this.radRemove = new System.Windows.Forms.RadioButton();
            this.radChange = new System.Windows.Forms.RadioButton();
            this.radAdd = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.userProvidedMainMenuDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.textBox1 = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.textBox2 = new Uheaa.Common.WinForms.AlphaTextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userProvidedMainMenuDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "ACH Setup Main Menu";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 57);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "SSN/Account Num";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(78, 123);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "First Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radMissingInfo);
            this.groupBox1.Controls.Add(this.radSuspend);
            this.groupBox1.Controls.Add(this.radRemove);
            this.groupBox1.Controls.Add(this.radChange);
            this.groupBox1.Controls.Add(this.radAdd);
            this.groupBox1.Location = new System.Drawing.Point(22, 184);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(204, 218);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // radMissingInfo
            // 
            this.radMissingInfo.AutoSize = true;
            this.radMissingInfo.Location = new System.Drawing.Point(9, 171);
            this.radMissingInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radMissingInfo.Name = "radMissingInfo";
            this.radMissingInfo.Size = new System.Drawing.Size(165, 24);
            this.radMissingInfo.TabIndex = 4;
            this.radMissingInfo.TabStop = true;
            this.radMissingInfo.Text = "Missing Information";
            this.radMissingInfo.UseVisualStyleBackColor = true;
            // 
            // radSuspend
            // 
            this.radSuspend.AutoSize = true;
            this.radSuspend.Location = new System.Drawing.Point(9, 135);
            this.radSuspend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radSuspend.Name = "radSuspend";
            this.radSuspend.Size = new System.Drawing.Size(91, 24);
            this.radSuspend.TabIndex = 3;
            this.radSuspend.TabStop = true;
            this.radSuspend.Text = "Suspend";
            this.radSuspend.UseVisualStyleBackColor = true;
            // 
            // radRemove
            // 
            this.radRemove.AutoSize = true;
            this.radRemove.Location = new System.Drawing.Point(9, 100);
            this.radRemove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radRemove.Name = "radRemove";
            this.radRemove.Size = new System.Drawing.Size(86, 24);
            this.radRemove.TabIndex = 2;
            this.radRemove.TabStop = true;
            this.radRemove.Text = "Remove";
            this.radRemove.UseVisualStyleBackColor = true;
            // 
            // radChange
            // 
            this.radChange.AutoSize = true;
            this.radChange.Location = new System.Drawing.Point(9, 65);
            this.radChange.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radChange.Name = "radChange";
            this.radChange.Size = new System.Drawing.Size(83, 24);
            this.radChange.TabIndex = 1;
            this.radChange.TabStop = true;
            this.radChange.Text = "Change";
            this.radChange.UseVisualStyleBackColor = true;
            // 
            // radAdd
            // 
            this.radAdd.AutoSize = true;
            this.radAdd.Location = new System.Drawing.Point(9, 29);
            this.radAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radAdd.Name = "radAdd";
            this.radAdd.Size = new System.Drawing.Size(56, 24);
            this.radAdd.TabIndex = 0;
            this.radAdd.TabStop = true;
            this.radAdd.Text = "Add";
            this.radAdd.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(38, 412);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 46);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(38, 468);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 46);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // userProvidedMainMenuDataBindingSource
            // 
            this.userProvidedMainMenuDataBindingSource.DataSource = typeof(ACHSETUPFD.UserProvidedMainMenuData);
            // 
            // textBox1
            // 
            this.textBox1.AccountNumber = null;
            this.textBox1.AllowedSpecialCharacters = "";
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userProvidedMainMenuDataBindingSource, "SSN", true));
            this.textBox1.Location = new System.Drawing.Point(45, 82);
            this.textBox1.MaxLength = 10;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(148, 26);
            this.textBox1.Ssn = null;
            this.textBox1.TabIndex = 9;
            // 
            // textBox2
            // 
            this.textBox2.AllowedSpecialCharacters = "";
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.userProvidedMainMenuDataBindingSource, "FirstName", true));
            this.textBox2.Location = new System.Drawing.Point(45, 148);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(148, 26);
            this.textBox2.TabIndex = 10;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 530);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(260, 570);
            this.MinimumSize = new System.Drawing.Size(256, 569);
            this.Name = "MainMenu";
            this.Text = "ACH Main Menu";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userProvidedMainMenuDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radSuspend;
		private System.Windows.Forms.RadioButton radRemove;
		private System.Windows.Forms.RadioButton radChange;
		private System.Windows.Forms.RadioButton radAdd;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource userProvidedMainMenuDataBindingSource;
		private System.Windows.Forms.RadioButton radMissingInfo;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox textBox1;
        private Uheaa.Common.WinForms.AlphaTextBox textBox2;
    }
}