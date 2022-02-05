namespace CMPLNTRACK
{
    partial class ComplaintForm
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
            this.AccountNumberBox = new Uheaa.Common.WinForms.AccountNumberTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BorrowerNameBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ComplaintPartyBox = new System.Windows.Forms.ComboBox();
            this.ComplaintTypesBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ComplaintDescriptionBox = new System.Windows.Forms.TextBox();
            this.HistoryBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ComplaintUpdateBox = new System.Windows.Forms.TextBox();
            this.SubmitUpdateButton = new System.Windows.Forms.Button();
            this.UpdateResolvesComplaintCheck = new System.Windows.Forms.CheckBox();
            this.SaveChangesButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.FlagsList = new System.Windows.Forms.CheckedListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.DateReceivedBox = new System.Windows.Forms.DateTimePicker();
            this.ControlMailNumberBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.DaysToRespondBox = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.NeedHelpNumberBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.ComplaintGroupBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ReportedByBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.AutoResolveBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.DaysToRespondBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account Number";
            // 
            // AccountNumberBox
            // 
            this.AccountNumberBox.AllowedSpecialCharacters = "";
            this.AccountNumberBox.Location = new System.Drawing.Point(12, 29);
            this.AccountNumberBox.MaxLength = 10;
            this.AccountNumberBox.Name = "AccountNumberBox";
            this.AccountNumberBox.Size = new System.Drawing.Size(311, 23);
            this.AccountNumberBox.Ssn = null;
            this.AccountNumberBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Borrower/Party Name";
            // 
            // BorrowerNameBox
            // 
            this.BorrowerNameBox.Location = new System.Drawing.Point(12, 75);
            this.BorrowerNameBox.MaxLength = 50;
            this.BorrowerNameBox.Name = "BorrowerNameBox";
            this.BorrowerNameBox.Size = new System.Drawing.Size(311, 23);
            this.BorrowerNameBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Complaint Party";
            // 
            // ComplaintPartyBox
            // 
            this.ComplaintPartyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComplaintPartyBox.FormattingEnabled = true;
            this.ComplaintPartyBox.Location = new System.Drawing.Point(12, 170);
            this.ComplaintPartyBox.Name = "ComplaintPartyBox";
            this.ComplaintPartyBox.Size = new System.Drawing.Size(311, 24);
            this.ComplaintPartyBox.TabIndex = 5;
            // 
            // ComplaintTypesBox
            // 
            this.ComplaintTypesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComplaintTypesBox.FormattingEnabled = true;
            this.ComplaintTypesBox.Location = new System.Drawing.Point(12, 217);
            this.ComplaintTypesBox.Name = "ComplaintTypesBox";
            this.ComplaintTypesBox.Size = new System.Drawing.Size(311, 24);
            this.ComplaintTypesBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Complaint Type";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 528);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(183, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Short Complaint Description";
            // 
            // ComplaintDescriptionBox
            // 
            this.ComplaintDescriptionBox.Location = new System.Drawing.Point(12, 548);
            this.ComplaintDescriptionBox.MaxLength = 4000;
            this.ComplaintDescriptionBox.Multiline = true;
            this.ComplaintDescriptionBox.Name = "ComplaintDescriptionBox";
            this.ComplaintDescriptionBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ComplaintDescriptionBox.Size = new System.Drawing.Size(311, 101);
            this.ComplaintDescriptionBox.TabIndex = 9;
            // 
            // HistoryBox
            // 
            this.HistoryBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HistoryBox.Location = new System.Drawing.Point(329, 168);
            this.HistoryBox.Multiline = true;
            this.HistoryBox.Name = "HistoryBox";
            this.HistoryBox.ReadOnly = true;
            this.HistoryBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.HistoryBox.Size = new System.Drawing.Size(607, 567);
            this.HistoryBox.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(326, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Update Complaint";
            // 
            // ComplaintUpdateBox
            // 
            this.ComplaintUpdateBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ComplaintUpdateBox.Enabled = false;
            this.ComplaintUpdateBox.Location = new System.Drawing.Point(329, 29);
            this.ComplaintUpdateBox.MaxLength = 4000;
            this.ComplaintUpdateBox.Multiline = true;
            this.ComplaintUpdateBox.Name = "ComplaintUpdateBox";
            this.ComplaintUpdateBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ComplaintUpdateBox.Size = new System.Drawing.Size(607, 86);
            this.ComplaintUpdateBox.TabIndex = 12;
            // 
            // SubmitUpdateButton
            // 
            this.SubmitUpdateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SubmitUpdateButton.Enabled = false;
            this.SubmitUpdateButton.Location = new System.Drawing.Point(803, 121);
            this.SubmitUpdateButton.Name = "SubmitUpdateButton";
            this.SubmitUpdateButton.Size = new System.Drawing.Size(133, 24);
            this.SubmitUpdateButton.TabIndex = 13;
            this.SubmitUpdateButton.Text = "Submit Update";
            this.SubmitUpdateButton.UseVisualStyleBackColor = true;
            this.SubmitUpdateButton.Click += new System.EventHandler(this.SubmitUpdateButton_Click);
            // 
            // UpdateResolvesComplaintCheck
            // 
            this.UpdateResolvesComplaintCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpdateResolvesComplaintCheck.AutoSize = true;
            this.UpdateResolvesComplaintCheck.Enabled = false;
            this.UpdateResolvesComplaintCheck.Location = new System.Drawing.Point(546, 124);
            this.UpdateResolvesComplaintCheck.Name = "UpdateResolvesComplaintCheck";
            this.UpdateResolvesComplaintCheck.Size = new System.Drawing.Size(251, 21);
            this.UpdateResolvesComplaintCheck.TabIndex = 14;
            this.UpdateResolvesComplaintCheck.Text = "This update resolves the complaint.";
            this.UpdateResolvesComplaintCheck.UseVisualStyleBackColor = true;
            // 
            // SaveChangesButton
            // 
            this.SaveChangesButton.Location = new System.Drawing.Point(190, 711);
            this.SaveChangesButton.Name = "SaveChangesButton";
            this.SaveChangesButton.Size = new System.Drawing.Size(133, 24);
            this.SaveChangesButton.TabIndex = 15;
            this.SaveChangesButton.Text = "Save Changes";
            this.SaveChangesButton.UseVisualStyleBackColor = true;
            this.SaveChangesButton.Click += new System.EventHandler(this.SaveChangesButton_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(326, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 17);
            this.label7.TabIndex = 17;
            this.label7.Text = "History";
            // 
            // FlagsList
            // 
            this.FlagsList.CheckOnClick = true;
            this.FlagsList.FormattingEnabled = true;
            this.FlagsList.Location = new System.Drawing.Point(12, 340);
            this.FlagsList.Name = "FlagsList";
            this.FlagsList.Size = new System.Drawing.Size(311, 76);
            this.FlagsList.TabIndex = 18;
            this.FlagsList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FlagsList_ItemCheck);
            this.FlagsList.SelectedIndexChanged += new System.EventHandler(this.FlagsList_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 290);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 17);
            this.label8.TabIndex = 19;
            this.label8.Text = "Flags";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 101);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 17);
            this.label9.TabIndex = 20;
            this.label9.Text = "Date Received";
            // 
            // DateReceivedBox
            // 
            this.DateReceivedBox.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateReceivedBox.Location = new System.Drawing.Point(12, 124);
            this.DateReceivedBox.Name = "DateReceivedBox";
            this.DateReceivedBox.Size = new System.Drawing.Size(311, 23);
            this.DateReceivedBox.TabIndex = 21;
            // 
            // ControlMailNumberBox
            // 
            this.ControlMailNumberBox.Enabled = false;
            this.ControlMailNumberBox.Location = new System.Drawing.Point(12, 439);
            this.ControlMailNumberBox.MaxLength = 50;
            this.ControlMailNumberBox.Name = "ControlMailNumberBox";
            this.ControlMailNumberBox.Size = new System.Drawing.Size(311, 23);
            this.ControlMailNumberBox.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 419);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(136, 17);
            this.label10.TabIndex = 22;
            this.label10.Text = "Control Mail Number";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 465);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(122, 17);
            this.label11.TabIndex = 24;
            this.label11.Text = "Days To Respond";
            // 
            // DaysToRespondBox
            // 
            this.DaysToRespondBox.Enabled = false;
            this.DaysToRespondBox.Location = new System.Drawing.Point(15, 485);
            this.DaysToRespondBox.Name = "DaysToRespondBox";
            this.DaysToRespondBox.Size = new System.Drawing.Size(308, 23);
            this.DaysToRespondBox.TabIndex = 25;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 291);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(218, 17);
            this.label12.TabIndex = 26;
            this.label12.Text = "Need Help Ticket # (if applicable)";
            // 
            // NeedHelpNumberBox
            // 
            this.NeedHelpNumberBox.AllowAllCharacters = false;
            this.NeedHelpNumberBox.AllowAlphaCharacters = false;
            this.NeedHelpNumberBox.AllowedAdditionalCharacters = "";
            this.NeedHelpNumberBox.AllowNumericCharacters = true;
            this.NeedHelpNumberBox.InvalidColor = System.Drawing.Color.LightPink;
            this.NeedHelpNumberBox.Location = new System.Drawing.Point(12, 311);
            this.NeedHelpNumberBox.Mask = "";
            this.NeedHelpNumberBox.Name = "NeedHelpNumberBox";
            this.NeedHelpNumberBox.Size = new System.Drawing.Size(311, 23);
            this.NeedHelpNumberBox.TabIndex = 27;
            this.NeedHelpNumberBox.ValidationMessage = null;
            this.NeedHelpNumberBox.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // ComplaintGroupBox
            // 
            this.ComplaintGroupBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComplaintGroupBox.FormattingEnabled = true;
            this.ComplaintGroupBox.Location = new System.Drawing.Point(12, 264);
            this.ComplaintGroupBox.Name = "ComplaintGroupBox";
            this.ComplaintGroupBox.Size = new System.Drawing.Size(311, 24);
            this.ComplaintGroupBox.TabIndex = 29;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 244);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 17);
            this.label13.TabIndex = 28;
            this.label13.Text = "Complaint Group";
            // 
            // ReportedByBox
            // 
            this.ReportedByBox.Location = new System.Drawing.Point(12, 672);
            this.ReportedByBox.MaxLength = 50;
            this.ReportedByBox.Name = "ReportedByBox";
            this.ReportedByBox.ReadOnly = true;
            this.ReportedByBox.Size = new System.Drawing.Size(311, 23);
            this.ReportedByBox.TabIndex = 31;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 652);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(87, 17);
            this.label14.TabIndex = 30;
            this.label14.Text = "Reported By";
            // 
            // AutoResolveBox
            // 
            this.AutoResolveBox.AutoSize = true;
            this.AutoResolveBox.Location = new System.Drawing.Point(12, 714);
            this.AutoResolveBox.Name = "AutoResolveBox";
            this.AutoResolveBox.Size = new System.Drawing.Size(166, 21);
            this.AutoResolveBox.TabIndex = 32;
            this.AutoResolveBox.Text = "Complaint is Resolved";
            this.AutoResolveBox.UseVisualStyleBackColor = true;
            this.AutoResolveBox.Visible = false;
            // 
            // ComplaintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 747);
            this.Controls.Add(this.AutoResolveBox);
            this.Controls.Add(this.ReportedByBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.ComplaintGroupBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.NeedHelpNumberBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.DaysToRespondBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ControlMailNumberBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.DateReceivedBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.FlagsList);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SaveChangesButton);
            this.Controls.Add(this.UpdateResolvesComplaintCheck);
            this.Controls.Add(this.SubmitUpdateButton);
            this.Controls.Add(this.ComplaintUpdateBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.HistoryBox);
            this.Controls.Add(this.ComplaintDescriptionBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ComplaintTypesBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ComplaintPartyBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BorrowerNameBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AccountNumberBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(964, 786);
            this.Name = "ComplaintForm";
            this.Text = "Complaint History";
            ((System.ComponentModel.ISupportInitialize)(this.DaysToRespondBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.AccountNumberTextBox AccountNumberBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox BorrowerNameBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ComplaintPartyBox;
        private System.Windows.Forms.ComboBox ComplaintTypesBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ComplaintDescriptionBox;
        private System.Windows.Forms.TextBox HistoryBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ComplaintUpdateBox;
        private System.Windows.Forms.Button SubmitUpdateButton;
        private System.Windows.Forms.CheckBox UpdateResolvesComplaintCheck;
        private System.Windows.Forms.Button SaveChangesButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckedListBox FlagsList;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker DateReceivedBox;
        private System.Windows.Forms.TextBox ControlMailNumberBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown DaysToRespondBox;
        private System.Windows.Forms.Label label12;
        private Uheaa.Common.WinForms.OmniTextBox NeedHelpNumberBox;
        private System.Windows.Forms.ComboBox ComplaintGroupBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox ReportedByBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox AutoResolveBox;

    }
}