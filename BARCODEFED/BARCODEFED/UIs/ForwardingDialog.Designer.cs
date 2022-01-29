namespace BARCODEFED
{
	partial class ForwardingDialog
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
            this.lblStreet1 = new System.Windows.Forms.Label();
            this.lblStreet2 = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblZip = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.cmbStates = new System.Windows.Forms.ComboBox();
            this.forwardingInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRules = new System.Windows.Forms.Button();
            this.txtAddress1 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.txtAddress2 = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.txtZip = new Uheaa.Common.WinForms.NumericTextBox();
            this.txtCountry = new Uheaa.Common.WinForms.AlphaNumericTextBox();
            this.txtCity = new Uheaa.Common.WinForms.AlphaTextBox();
            this.AddressVerifiedlbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.forwardingInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStreet1
            // 
            this.lblStreet1.AutoSize = true;
            this.lblStreet1.Location = new System.Drawing.Point(21, 40);
            this.lblStreet1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStreet1.Name = "lblStreet1";
            this.lblStreet1.Size = new System.Drawing.Size(66, 20);
            this.lblStreet1.TabIndex = 40;
            this.lblStreet1.Text = "Street 1";
            // 
            // lblStreet2
            // 
            this.lblStreet2.AutoSize = true;
            this.lblStreet2.Location = new System.Drawing.Point(21, 77);
            this.lblStreet2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStreet2.Name = "lblStreet2";
            this.lblStreet2.Size = new System.Drawing.Size(66, 20);
            this.lblStreet2.TabIndex = 41;
            this.lblStreet2.Text = "Street 2";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(52, 114);
            this.lblCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(35, 20);
            this.lblCity.TabIndex = 42;
            this.lblCity.Text = "City";
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Location = new System.Drawing.Point(56, 188);
            this.lblZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(31, 20);
            this.lblZip.TabIndex = 44;
            this.lblZip.Text = "Zip";
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.Location = new System.Drawing.Point(23, 225);
            this.lblCountry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(64, 20);
            this.lblCountry.TabIndex = 45;
            this.lblCountry.Text = "Country";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(39, 151);
            this.lblState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(48, 20);
            this.lblState.TabIndex = 43;
            this.lblState.Text = "State";
            // 
            // cmbStates
            // 
            this.cmbStates.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "State", true));
            this.cmbStates.FormattingEnabled = true;
            this.cmbStates.Location = new System.Drawing.Point(96, 148);
            this.cmbStates.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbStates.Name = "cmbStates";
            this.cmbStates.Size = new System.Drawing.Size(67, 28);
            this.cmbStates.TabIndex = 4;
            // 
            // forwardingInfoBindingSource
            // 
            this.forwardingInfoBindingSource.DataSource = typeof(BARCODEFED.ForwardingInfo);
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnContinue.Location = new System.Drawing.Point(21, 274);
            this.btnContinue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(150, 46);
            this.btnContinue.TabIndex = 7;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(195, 274);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 46);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRules
            // 
            this.btnRules.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRules.Location = new System.Drawing.Point(369, 274);
            this.btnRules.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRules.Name = "btnRules";
            this.btnRules.Size = new System.Drawing.Size(150, 46);
            this.btnRules.TabIndex = 9;
            this.btnRules.Text = "Rules";
            this.btnRules.UseVisualStyleBackColor = true;
            this.btnRules.Click += new System.EventHandler(this.btnRules_Click);
            // 
            // txtAddress1
            // 
            this.txtAddress1.AllowedSpecialCharacters = " ";
            this.txtAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "Address1", true));
            this.txtAddress1.Location = new System.Drawing.Point(94, 37);
            this.txtAddress1.MaxLength = 30;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.Size = new System.Drawing.Size(425, 26);
            this.txtAddress1.TabIndex = 1;
            // 
            // txtAddress2
            // 
            this.txtAddress2.AllowedSpecialCharacters = " ";
            this.txtAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "Address2", true));
            this.txtAddress2.Location = new System.Drawing.Point(96, 74);
            this.txtAddress2.MaxLength = 30;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(421, 26);
            this.txtAddress2.TabIndex = 2;
            // 
            // txtZip
            // 
            this.txtZip.AllowedSpecialCharacters = "";
            this.txtZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "Zip", true));
            this.txtZip.Location = new System.Drawing.Point(96, 187);
            this.txtZip.MaxLength = 9;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(100, 26);
            this.txtZip.TabIndex = 5;
            // 
            // txtCountry
            // 
            this.txtCountry.AllowedSpecialCharacters = " ";
            this.txtCountry.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "Country", true));
            this.txtCountry.Location = new System.Drawing.Point(96, 224);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(373, 26);
            this.txtCountry.TabIndex = 6;
            // 
            // txtCity
            // 
            this.txtCity.AllowedSpecialCharacters = " ";
            this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.forwardingInfoBindingSource, "City", true));
            this.txtCity.Location = new System.Drawing.Point(96, 111);
            this.txtCity.MaxLength = 20;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(249, 26);
            this.txtCity.TabIndex = 3;
            // 
            // AddressVerifiedlbl
            // 
            this.AddressVerifiedlbl.AutoSize = true;
            this.AddressVerifiedlbl.Location = new System.Drawing.Point(342, 14);
            this.AddressVerifiedlbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.AddressVerifiedlbl.Name = "AddressVerifiedlbl";
            this.AddressVerifiedlbl.Size = new System.Drawing.Size(0, 20);
            this.AddressVerifiedlbl.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(130, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 20);
            this.label2.TabIndex = 31;
            this.label2.Text = "Address Last Verified Date:";
            // 
            // ForwardingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 337);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AddressVerifiedlbl);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.txtCountry);
            this.Controls.Add(this.txtZip);
            this.Controls.Add(this.txtAddress2);
            this.Controls.Add(this.txtAddress1);
            this.Controls.Add(this.btnRules);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.cmbStates);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lblCountry);
            this.Controls.Add(this.lblZip);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.lblStreet2);
            this.Controls.Add(this.lblStreet1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(554, 375);
            this.Name = "ForwardingDialog";
            this.Text = "Forwarding Address";
            ((System.ComponentModel.ISupportInitialize)(this.forwardingInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label lblStreet1;
		private System.Windows.Forms.Label lblStreet2;
		private System.Windows.Forms.Label lblCity;
		private System.Windows.Forms.Label lblZip;
		private System.Windows.Forms.Label lblCountry;
		private System.Windows.Forms.Label lblState;
		private System.Windows.Forms.ComboBox cmbStates;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource forwardingInfoBindingSource;
		private System.Windows.Forms.Button btnRules;
        private Uheaa.Common.WinForms.AlphaNumericTextBox txtAddress1;
        private Uheaa.Common.WinForms.AlphaNumericTextBox txtAddress2;
        private Uheaa.Common.WinForms.NumericTextBox txtZip;
        private Uheaa.Common.WinForms.AlphaNumericTextBox txtCountry;
        private Uheaa.Common.WinForms.AlphaTextBox txtCity;
        private System.Windows.Forms.Label AddressVerifiedlbl;
        private System.Windows.Forms.Label label2;
    }
}