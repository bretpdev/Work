namespace NBLCONTPUL
{
	partial class UpdateCampaigns
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
			this.CallCampaignComboBox = new System.Windows.Forms.ComboBox();
			this.CallCampaignLbl = new System.Windows.Forms.Label();
			this.CampaignDescriptionTextBox = new System.Windows.Forms.TextBox();
			this.CampaignDescriptionLbl = new System.Windows.Forms.Label();
			this.GroupComboBox = new System.Windows.Forms.ComboBox();
			this.GroupLbl = new System.Windows.Forms.Label();
			this.ActiveCheckBox = new System.Windows.Forms.CheckBox();
			this.InvalidateCheckBox = new System.Windows.Forms.CheckBox();
			this.ContinueButton = new System.Windows.Forms.Button();
			this.CallTypeInbound = new System.Windows.Forms.RadioButton();
			this.CallTypeOutbound = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// CallCampaignComboBox
			// 
			this.CallCampaignComboBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CallCampaignComboBox.FormattingEnabled = true;
			this.CallCampaignComboBox.Location = new System.Drawing.Point(200, 51);
			this.CallCampaignComboBox.MaximumSize = new System.Drawing.Size(169, 0);
			this.CallCampaignComboBox.MaxLength = 6;
			this.CallCampaignComboBox.MinimumSize = new System.Drawing.Size(169, 0);
			this.CallCampaignComboBox.Name = "CallCampaignComboBox";
			this.CallCampaignComboBox.Size = new System.Drawing.Size(169, 27);
			this.CallCampaignComboBox.TabIndex = 1;
			this.CallCampaignComboBox.Leave += new System.EventHandler(this.CallCampaignComboBox_Leave);
			// 
			// CallCampaignLbl
			// 
			this.CallCampaignLbl.AutoSize = true;
			this.CallCampaignLbl.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CallCampaignLbl.Location = new System.Drawing.Point(28, 54);
			this.CallCampaignLbl.Name = "CallCampaignLbl";
			this.CallCampaignLbl.Size = new System.Drawing.Size(136, 19);
			this.CallCampaignLbl.TabIndex = 10;
			this.CallCampaignLbl.Text = "Call Campaign Code";
			// 
			// CampaignDescriptionTextBox
			// 
			this.CampaignDescriptionTextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CampaignDescriptionTextBox.Location = new System.Drawing.Point(200, 77);
			this.CampaignDescriptionTextBox.MaximumSize = new System.Drawing.Size(169, 26);
			this.CampaignDescriptionTextBox.MaxLength = 100;
			this.CampaignDescriptionTextBox.MinimumSize = new System.Drawing.Size(169, 26);
			this.CampaignDescriptionTextBox.Name = "CampaignDescriptionTextBox";
			this.CampaignDescriptionTextBox.Size = new System.Drawing.Size(169, 26);
			this.CampaignDescriptionTextBox.TabIndex = 2;
			// 
			// CampaignDescriptionLbl
			// 
			this.CampaignDescriptionLbl.AutoSize = true;
			this.CampaignDescriptionLbl.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CampaignDescriptionLbl.Location = new System.Drawing.Point(28, 81);
			this.CampaignDescriptionLbl.Name = "CampaignDescriptionLbl";
			this.CampaignDescriptionLbl.Size = new System.Drawing.Size(143, 19);
			this.CampaignDescriptionLbl.TabIndex = 11;
			this.CampaignDescriptionLbl.Text = "Campaign Description";
			// 
			// GroupComboBox
			// 
			this.GroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GroupComboBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GroupComboBox.FormattingEnabled = true;
			this.GroupComboBox.Location = new System.Drawing.Point(200, 24);
			this.GroupComboBox.MaximumSize = new System.Drawing.Size(169, 0);
			this.GroupComboBox.MinimumSize = new System.Drawing.Size(169, 0);
			this.GroupComboBox.Name = "GroupComboBox";
			this.GroupComboBox.Size = new System.Drawing.Size(169, 27);
			this.GroupComboBox.TabIndex = 0;
			this.GroupComboBox.SelectedIndexChanged += new System.EventHandler(this.GroupComboBox_SelectedIndexChanged);
			// 
			// GroupLbl
			// 
			this.GroupLbl.AutoSize = true;
			this.GroupLbl.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GroupLbl.Location = new System.Drawing.Point(28, 27);
			this.GroupLbl.Name = "GroupLbl";
			this.GroupLbl.Size = new System.Drawing.Size(48, 19);
			this.GroupLbl.TabIndex = 9;
			this.GroupLbl.Text = "Group";
			// 
			// ActiveCheckBox
			// 
			this.ActiveCheckBox.AutoSize = true;
			this.ActiveCheckBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ActiveCheckBox.Location = new System.Drawing.Point(32, 110);
			this.ActiveCheckBox.Name = "ActiveCheckBox";
			this.ActiveCheckBox.Size = new System.Drawing.Size(67, 23);
			this.ActiveCheckBox.TabIndex = 6;
			this.ActiveCheckBox.Text = "Active";
			this.ActiveCheckBox.UseVisualStyleBackColor = true;
			this.ActiveCheckBox.CheckedChanged += new System.EventHandler(this.ActiveCheckBox_CheckedChanged);
			// 
			// InvalidateCheckBox
			// 
			this.InvalidateCheckBox.AutoSize = true;
			this.InvalidateCheckBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.InvalidateCheckBox.Location = new System.Drawing.Point(32, 139);
			this.InvalidateCheckBox.Name = "InvalidateCheckBox";
			this.InvalidateCheckBox.Size = new System.Drawing.Size(120, 23);
			this.InvalidateCheckBox.TabIndex = 7;
			this.InvalidateCheckBox.Text = "Auto Invalidate";
			this.InvalidateCheckBox.UseVisualStyleBackColor = true;
			this.InvalidateCheckBox.CheckedChanged += new System.EventHandler(this.InvalidateCheckBox_CheckedChanged);
			// 
			// ContinueButton
			// 
			this.ContinueButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ContinueButton.Location = new System.Drawing.Point(292, 139);
			this.ContinueButton.Name = "ContinueButton";
			this.ContinueButton.Size = new System.Drawing.Size(75, 23);
			this.ContinueButton.TabIndex = 8;
			this.ContinueButton.Text = "Continue";
			this.ContinueButton.UseVisualStyleBackColor = true;
			this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
			// 
			// CallTypeInbound
			// 
			this.CallTypeInbound.AutoSize = true;
			this.CallTypeInbound.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CallTypeInbound.Location = new System.Drawing.Point(200, 109);
			this.CallTypeInbound.Name = "CallTypeInbound";
			this.CallTypeInbound.Size = new System.Drawing.Size(77, 23);
			this.CallTypeInbound.TabIndex = 4;
			this.CallTypeInbound.TabStop = true;
			this.CallTypeInbound.Text = "Inbound";
			this.CallTypeInbound.UseVisualStyleBackColor = true;
			this.CallTypeInbound.CheckedChanged += new System.EventHandler(this.CallTypeInbound_CheckedChanged);
			// 
			// CallTypeOutbound
			// 
			this.CallTypeOutbound.AutoSize = true;
			this.CallTypeOutbound.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CallTypeOutbound.Location = new System.Drawing.Point(281, 109);
			this.CallTypeOutbound.Name = "CallTypeOutbound";
			this.CallTypeOutbound.Size = new System.Drawing.Size(88, 23);
			this.CallTypeOutbound.TabIndex = 5;
			this.CallTypeOutbound.TabStop = true;
			this.CallTypeOutbound.Text = "Outbound";
			this.CallTypeOutbound.UseVisualStyleBackColor = true;
			this.CallTypeOutbound.CheckedChanged += new System.EventHandler(this.CallTypeOutbound_CheckedChanged);
			// 
			// UpdateCampaigns
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(396, 174);
			this.Controls.Add(this.CallTypeOutbound);
			this.Controls.Add(this.CallTypeInbound);
			this.Controls.Add(this.ContinueButton);
			this.Controls.Add(this.InvalidateCheckBox);
			this.Controls.Add(this.ActiveCheckBox);
			this.Controls.Add(this.GroupLbl);
			this.Controls.Add(this.GroupComboBox);
			this.Controls.Add(this.CampaignDescriptionLbl);
			this.Controls.Add(this.CampaignDescriptionTextBox);
			this.Controls.Add(this.CallCampaignLbl);
			this.Controls.Add(this.CallCampaignComboBox);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(412, 212);
			this.MinimumSize = new System.Drawing.Size(412, 212);
			this.Name = "UpdateCampaigns";
			this.Text = "UpdateCampaigns";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox CallCampaignComboBox;
		private System.Windows.Forms.Label CallCampaignLbl;
		private System.Windows.Forms.TextBox CampaignDescriptionTextBox;
		private System.Windows.Forms.Label CampaignDescriptionLbl;
		private System.Windows.Forms.ComboBox GroupComboBox;
		private System.Windows.Forms.Label GroupLbl;
		private System.Windows.Forms.CheckBox ActiveCheckBox;
		private System.Windows.Forms.CheckBox InvalidateCheckBox;
		private System.Windows.Forms.Button ContinueButton;
		private System.Windows.Forms.RadioButton CallTypeInbound;
		private System.Windows.Forms.RadioButton CallTypeOutbound;
	}
}