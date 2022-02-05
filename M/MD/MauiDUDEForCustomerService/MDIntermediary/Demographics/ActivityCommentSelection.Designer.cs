namespace MDIntermediary.Demographics
{
    partial class ActivityCommentSelection
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
            this.CallStatusLabel = new System.Windows.Forms.Label();
            this.CallStatusBox = new System.Windows.Forms.ComboBox();
            this.SecondLabel = new System.Windows.Forms.Label();
            this.FirstLabel = new System.Windows.Forms.Label();
            this.SecondSelectionBox = new System.Windows.Forms.ComboBox();
            this.FirstSelectionBox = new System.Windows.Forms.ComboBox();
            this.ActivityCodeBox = new System.Windows.Forms.TextBox();
            this.ContactCodeBox = new System.Windows.Forms.TextBox();
            this.ContactInfoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CallStatusLabel
            // 
            this.CallStatusLabel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CallStatusLabel.Location = new System.Drawing.Point(5, 66);
            this.CallStatusLabel.Name = "CallStatusLabel";
            this.CallStatusLabel.Size = new System.Drawing.Size(78, 16);
            this.CallStatusLabel.TabIndex = 13;
            this.CallStatusLabel.Text = "Call Status";
            this.CallStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CallStatusLabel.Visible = false;
            // 
            // CallStatusBox
            // 
            this.CallStatusBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CallStatusBox.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CallStatusBox.Items.AddRange(new object[] {
            "",
            "No Answer",
            "Answering Machine/Service",
            "Wrong Number",
            "Phone Busy",
            "Disconnected Phone/Out of Service"});
            this.CallStatusBox.Location = new System.Drawing.Point(89, 61);
            this.CallStatusBox.Name = "CallStatusBox";
            this.CallStatusBox.Size = new System.Drawing.Size(225, 24);
            this.CallStatusBox.TabIndex = 10;
            this.CallStatusBox.Visible = false;
            this.CallStatusBox.SelectedIndexChanged += new System.EventHandler(this.CallStatusBox_SelectedIndexChanged);
            // 
            // SecondLabel
            // 
            this.SecondLabel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecondLabel.Location = new System.Drawing.Point(2, 35);
            this.SecondLabel.Name = "SecondLabel";
            this.SecondLabel.Size = new System.Drawing.Size(81, 23);
            this.SecondLabel.TabIndex = 12;
            this.SecondLabel.Text = "Recipient";
            this.SecondLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FirstLabel
            // 
            this.FirstLabel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FirstLabel.Location = new System.Drawing.Point(3, 3);
            this.FirstLabel.Name = "FirstLabel";
            this.FirstLabel.Size = new System.Drawing.Size(80, 23);
            this.FirstLabel.TabIndex = 11;
            this.FirstLabel.Text = "Call Type";
            this.FirstLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SecondSelectionBox
            // 
            this.SecondSelectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SecondSelectionBox.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecondSelectionBox.ItemHeight = 16;
            this.SecondSelectionBox.Items.AddRange(new object[] {
            "",
            "To: Attorney",
            "From: Attorney",
            "To: Borrower",
            "From: Borrower",
            "To: Comaker",
            "From: Comaker",
            "To: Credit Bureau",
            "From: Credit Bureau",
            "To: DMV",
            "From: DMV",
            "To: Employer",
            "From: Employer",
            "To: Endorser",
            "From: Endorser",
            "To: Family",
            "From: Family",
            "To: Guarantor",
            "From: Guarantor",
            "To: Lender",
            "From: Lender",
            "To: Miscellaneous",
            "From: Miscellaneous",
            "To: Post Office",
            "From: Post Office",
            "To: Prison",
            "From: Prison",
            "To: Reference",
            "From: Reference",
            "To: School",
            "From: School",
            "To: UHEAA Staff",
            "From: UHEAA Staff",
            "To 3rd Party",
            "From 3rd Party"});
            this.SecondSelectionBox.Location = new System.Drawing.Point(121, 33);
            this.SecondSelectionBox.Name = "SecondSelectionBox";
            this.SecondSelectionBox.Size = new System.Drawing.Size(327, 24);
            this.SecondSelectionBox.TabIndex = 9;
            this.SecondSelectionBox.SelectedIndexChanged += new System.EventHandler(this.SecondSelectionBox_SelectedIndexChanged);
            // 
            // FirstSelectionBox
            // 
            this.FirstSelectionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FirstSelectionBox.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FirstSelectionBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.FirstSelectionBox.ItemHeight = 16;
            this.FirstSelectionBox.Items.AddRange(new object[] {
            "",
            "Account Maintenance",
            "Court Document",
            "Claim",
            "Computer Letter",
            "Electronic Document",
            "E-mail",
            "Electronic Transmission",
            "Fax",
            "Form",
            "Letter",
            "Miscellaneous",
            "Preclaim",
            "Reference Contact Helpful (Home#)",
            "Reference Attempt (Home#)",
            "Reference Contact Not Helpful (Home#)",
            "Reference Do Not Contact (Home#)",
            "Reference Contact Helpful (Alt#)",
            "Reference Attempt (Alt#)",
            "Reference Contact Not Helpful (Alt#)",
            "Reference Do Not Contact (Alt#)",
            "Tape",
            "Telephone Contact",
            "Telephone Call",
            "Telephone Attempt",
            "Office Visit"});
            this.FirstSelectionBox.Location = new System.Drawing.Point(121, 3);
            this.FirstSelectionBox.Name = "FirstSelectionBox";
            this.FirstSelectionBox.Size = new System.Drawing.Size(327, 24);
            this.FirstSelectionBox.TabIndex = 8;
            this.FirstSelectionBox.SelectedIndexChanged += new System.EventHandler(this.FirstSelectionBox_SelectedIndexChanged);
            // 
            // ActivityCodeBox
            // 
            this.ActivityCodeBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ActivityCodeBox.Location = new System.Drawing.Point(90, 3);
            this.ActivityCodeBox.MaxLength = 2;
            this.ActivityCodeBox.Name = "ActivityCodeBox";
            this.ActivityCodeBox.Size = new System.Drawing.Size(25, 23);
            this.ActivityCodeBox.TabIndex = 14;
            this.ActivityCodeBox.TextChanged += new System.EventHandler(this.ActivityCodeBox_TextChanged);
            this.ActivityCodeBox.Leave += new System.EventHandler(this.ActivityCodeBox_Leave);
            // 
            // ContactCodeBox
            // 
            this.ContactCodeBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ContactCodeBox.Location = new System.Drawing.Point(90, 32);
            this.ContactCodeBox.MaxLength = 2;
            this.ContactCodeBox.Name = "ContactCodeBox";
            this.ContactCodeBox.Size = new System.Drawing.Size(25, 23);
            this.ContactCodeBox.TabIndex = 15;
            this.ContactCodeBox.TextChanged += new System.EventHandler(this.ContactCodeBox_TextChanged);
            this.ContactCodeBox.Leave += new System.EventHandler(this.ContactCodeBox_Leave);
            // 
            // ContactInfoButton
            // 
            this.ContactInfoButton.Location = new System.Drawing.Point(320, 61);
            this.ContactInfoButton.Name = "ContactInfoButton";
            this.ContactInfoButton.Size = new System.Drawing.Size(128, 24);
            this.ContactInfoButton.TabIndex = 16;
            this.ContactInfoButton.Text = "Recipient Info...";
            this.ContactInfoButton.UseVisualStyleBackColor = true;
            this.ContactInfoButton.Visible = false;
            this.ContactInfoButton.Click += new System.EventHandler(this.ContactInfoButton_Click);
            // 
            // ActivityCommentSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ContactInfoButton);
            this.Controls.Add(this.ContactCodeBox);
            this.Controls.Add(this.ActivityCodeBox);
            this.Controls.Add(this.CallStatusLabel);
            this.Controls.Add(this.CallStatusBox);
            this.Controls.Add(this.SecondLabel);
            this.Controls.Add(this.FirstLabel);
            this.Controls.Add(this.SecondSelectionBox);
            this.Controls.Add(this.FirstSelectionBox);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ActivityCommentSelection";
            this.Size = new System.Drawing.Size(478, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected internal System.Windows.Forms.Label CallStatusLabel;
        protected internal System.Windows.Forms.ComboBox CallStatusBox;
        protected internal System.Windows.Forms.Label SecondLabel;
        protected internal System.Windows.Forms.Label FirstLabel;
        protected internal System.Windows.Forms.ComboBox SecondSelectionBox;
        protected internal System.Windows.Forms.ComboBox FirstSelectionBox;
        private System.Windows.Forms.TextBox ActivityCodeBox;
        private System.Windows.Forms.TextBox ContactCodeBox;
        private System.Windows.Forms.Button ContactInfoButton;
    }
}
