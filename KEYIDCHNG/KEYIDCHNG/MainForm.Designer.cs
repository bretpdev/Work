namespace KEYIDCHNG
{
    partial class MainForm
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
            this.CommentsBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.NewInfoGroup = new System.Windows.Forms.GroupBox();
            this.DobBox = new Uheaa.Common.WinForms.MaskedDateTextBox();
            this.LastNameBox = new Uheaa.Common.WinForms.AlphaTextBox();
            this.MiddleInitialBox = new Uheaa.Common.WinForms.AlphaTextBox();
            this.FirstNameBox = new Uheaa.Common.WinForms.AlphaTextBox();
            this.RejectButton = new System.Windows.Forms.Button();
            this.ApproveButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BorrowerSearchResults = new Uheaa.Common.WinForms.BorrowerResultsControl();
            this.label2 = new System.Windows.Forms.Label();
            this.LoadBorrowerButton = new System.Windows.Forms.Button();
            this.AccountIdentifierBox = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.BorrowerSearch = new Uheaa.Common.WinForms.SimpleBorrowerSearchControl();
            this.SelectedGroup = new System.Windows.Forms.GroupBox();
            this.SelectedDobBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SelectedNameBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.NewInfoGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SelectedGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // CommentsBox
            // 
            this.CommentsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommentsBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.CommentsBox.Location = new System.Drawing.Point(15, 147);
            this.CommentsBox.Margin = new System.Windows.Forms.Padding(4);
            this.CommentsBox.MaxLength = 150;
            this.CommentsBox.Multiline = true;
            this.CommentsBox.Name = "CommentsBox";
            this.CommentsBox.Size = new System.Drawing.Size(463, 141);
            this.CommentsBox.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "First Name";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(296, 22);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 18);
            this.label7.TabIndex = 11;
            this.label7.Text = "Middle Initial";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 72);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 18);
            this.label8.TabIndex = 12;
            this.label8.Text = "Last Name";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(297, 75);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 18);
            this.label9.TabIndex = 13;
            this.label9.Text = "DOB";
            // 
            // NewInfoGroup
            // 
            this.NewInfoGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewInfoGroup.Controls.Add(this.DobBox);
            this.NewInfoGroup.Controls.Add(this.LastNameBox);
            this.NewInfoGroup.Controls.Add(this.MiddleInitialBox);
            this.NewInfoGroup.Controls.Add(this.FirstNameBox);
            this.NewInfoGroup.Controls.Add(this.RejectButton);
            this.NewInfoGroup.Controls.Add(this.ApproveButton);
            this.NewInfoGroup.Controls.Add(this.label3);
            this.NewInfoGroup.Controls.Add(this.CommentsBox);
            this.NewInfoGroup.Controls.Add(this.label9);
            this.NewInfoGroup.Controls.Add(this.label8);
            this.NewInfoGroup.Controls.Add(this.label7);
            this.NewInfoGroup.Controls.Add(this.label6);
            this.NewInfoGroup.Enabled = false;
            this.NewInfoGroup.Location = new System.Drawing.Point(477, 104);
            this.NewInfoGroup.Margin = new System.Windows.Forms.Padding(4);
            this.NewInfoGroup.Name = "NewInfoGroup";
            this.NewInfoGroup.Padding = new System.Windows.Forms.Padding(4);
            this.NewInfoGroup.Size = new System.Drawing.Size(487, 348);
            this.NewInfoGroup.TabIndex = 2;
            this.NewInfoGroup.TabStop = false;
            this.NewInfoGroup.Text = "New Key Identifier Info";
            // 
            // DobBox
            // 
            this.DobBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.DobBox.Location = new System.Drawing.Point(300, 94);
            this.DobBox.Mask = "00/00/0000";
            this.DobBox.Name = "DobBox";
            this.DobBox.Size = new System.Drawing.Size(178, 26);
            this.DobBox.TabIndex = 3;
            // 
            // LastNameBox
            // 
            this.LastNameBox.AllowedSpecialCharacters = " ";
            this.LastNameBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LastNameBox.Location = new System.Drawing.Point(15, 94);
            this.LastNameBox.MaxLength = 23;
            this.LastNameBox.Name = "LastNameBox";
            this.LastNameBox.Size = new System.Drawing.Size(277, 26);
            this.LastNameBox.TabIndex = 2;
            // 
            // MiddleInitialBox
            // 
            this.MiddleInitialBox.AllowedSpecialCharacters = " ";
            this.MiddleInitialBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.MiddleInitialBox.Location = new System.Drawing.Point(299, 44);
            this.MiddleInitialBox.MaxLength = 1;
            this.MiddleInitialBox.Name = "MiddleInitialBox";
            this.MiddleInitialBox.Size = new System.Drawing.Size(41, 26);
            this.MiddleInitialBox.TabIndex = 1;
            // 
            // FirstNameBox
            // 
            this.FirstNameBox.AllowedSpecialCharacters = " ";
            this.FirstNameBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.FirstNameBox.Location = new System.Drawing.Point(15, 44);
            this.FirstNameBox.MaxLength = 12;
            this.FirstNameBox.Name = "FirstNameBox";
            this.FirstNameBox.Size = new System.Drawing.Size(277, 26);
            this.FirstNameBox.TabIndex = 0;
            // 
            // RejectButton
            // 
            this.RejectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RejectButton.Location = new System.Drawing.Point(346, 294);
            this.RejectButton.Name = "RejectButton";
            this.RejectButton.Size = new System.Drawing.Size(132, 38);
            this.RejectButton.TabIndex = 6;
            this.RejectButton.Text = "Reject";
            this.RejectButton.UseVisualStyleBackColor = true;
            this.RejectButton.Click += new System.EventHandler(this.RejectButton_Click);
            // 
            // ApproveButton
            // 
            this.ApproveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ApproveButton.Location = new System.Drawing.Point(15, 294);
            this.ApproveButton.Name = "ApproveButton";
            this.ApproveButton.Size = new System.Drawing.Size(132, 38);
            this.ApproveButton.TabIndex = 5;
            this.ApproveButton.Text = "Approve";
            this.ApproveButton.UseVisualStyleBackColor = true;
            this.ApproveButton.Click += new System.EventHandler(this.ApproveButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 124);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 18);
            this.label3.TabIndex = 14;
            this.label3.Text = "Comments";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 18);
            this.label1.TabIndex = 21;
            this.label1.Text = "SSN or Account Number";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.BorrowerSearchResults);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.LoadBorrowerButton);
            this.groupBox1.Controls.Add(this.AccountIdentifierBox);
            this.groupBox1.Controls.Add(this.BorrowerSearch);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(18, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 442);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // BorrowerSearchResults
            // 
            this.BorrowerSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerSearchResults.ChooseBorrowerOnSelection = false;
            this.BorrowerSearchResults.EmailsColumnVisible = false;
            this.BorrowerSearchResults.Location = new System.Drawing.Point(11, 126);
            this.BorrowerSearchResults.Margin = new System.Windows.Forms.Padding(4);
            this.BorrowerSearchResults.Name = "BorrowerSearchResults";
            this.BorrowerSearchResults.PhoneColumnsVisible = false;
            this.BorrowerSearchResults.Size = new System.Drawing.Size(421, 300);
            this.BorrowerSearchResults.TabIndex = 3;
            this.BorrowerSearchResults.OnBorrowerChosen += new Uheaa.Common.WinForms.BorrowerResultsControl.SelectedBorrowerDelegate(this.BorrowerSearchResults_OnBorrowerChosen);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 18);
            this.label2.TabIndex = 23;
            this.label2.Text = "- OR -";
            // 
            // LoadBorrowerButton
            // 
            this.LoadBorrowerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadBorrowerButton.Location = new System.Drawing.Point(220, 38);
            this.LoadBorrowerButton.Name = "LoadBorrowerButton";
            this.LoadBorrowerButton.Size = new System.Drawing.Size(212, 26);
            this.LoadBorrowerButton.TabIndex = 1;
            this.LoadBorrowerButton.Text = "Load Borrower";
            this.LoadBorrowerButton.UseVisualStyleBackColor = true;
            this.LoadBorrowerButton.Click += new System.EventHandler(this.LoadBorrowerButton_Click);
            // 
            // AccountIdentifierBox
            // 
            this.AccountIdentifierBox.AccountNumber = null;
            this.AccountIdentifierBox.AllowedSpecialCharacters = "";
            this.AccountIdentifierBox.Location = new System.Drawing.Point(7, 38);
            this.AccountIdentifierBox.MaxLength = 10;
            this.AccountIdentifierBox.Name = "AccountIdentifierBox";
            this.AccountIdentifierBox.Size = new System.Drawing.Size(205, 26);
            this.AccountIdentifierBox.Ssn = null;
            this.AccountIdentifierBox.TabIndex = 0;
            this.AccountIdentifierBox.TextChanged += new System.EventHandler(this.AccountIdentifierBox_TextChanged);
            this.AccountIdentifierBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.AccountIdentifierBox_PreviewKeyDown);
            // 
            // BorrowerSearch
            // 
            this.BorrowerSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerSearch.LDA = null;
            this.BorrowerSearch.Location = new System.Drawing.Point(8, 85);
            this.BorrowerSearch.Margin = new System.Windows.Forms.Padding(4);
            this.BorrowerSearch.Name = "BorrowerSearch";
            this.BorrowerSearch.Size = new System.Drawing.Size(436, 33);
            this.BorrowerSearch.TabIndex = 2;
            this.BorrowerSearch.OnSearchResultsRetrieved += new Uheaa.Common.WinForms.SimpleBorrowerSearchControl.SearchResultsRetrieved(this.BorrowerSearch_OnSearchResultsRetrieved);
            // 
            // SelectedGroup
            // 
            this.SelectedGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedGroup.Controls.Add(this.SelectedDobBox);
            this.SelectedGroup.Controls.Add(this.label5);
            this.SelectedGroup.Controls.Add(this.SelectedNameBox);
            this.SelectedGroup.Controls.Add(this.label4);
            this.SelectedGroup.Location = new System.Drawing.Point(477, 11);
            this.SelectedGroup.Name = "SelectedGroup";
            this.SelectedGroup.Size = new System.Drawing.Size(487, 82);
            this.SelectedGroup.TabIndex = 1;
            this.SelectedGroup.TabStop = false;
            this.SelectedGroup.Text = "Selected Borrower";
            // 
            // SelectedDobBox
            // 
            this.SelectedDobBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedDobBox.Location = new System.Drawing.Point(299, 42);
            this.SelectedDobBox.Margin = new System.Windows.Forms.Padding(4);
            this.SelectedDobBox.Name = "SelectedDobBox";
            this.SelectedDobBox.ReadOnly = true;
            this.SelectedDobBox.Size = new System.Drawing.Size(180, 26);
            this.SelectedDobBox.TabIndex = 1;
            this.SelectedDobBox.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(295, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 18);
            this.label5.TabIndex = 14;
            this.label5.Text = "DOB";
            // 
            // SelectedNameBox
            // 
            this.SelectedNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedNameBox.Location = new System.Drawing.Point(12, 42);
            this.SelectedNameBox.Margin = new System.Windows.Forms.Padding(4);
            this.SelectedNameBox.Name = "SelectedNameBox";
            this.SelectedNameBox.ReadOnly = true;
            this.SelectedNameBox.Size = new System.Drawing.Size(277, 26);
            this.SelectedNameBox.TabIndex = 0;
            this.SelectedNameBox.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "Full Name";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 463);
            this.Controls.Add(this.SelectedGroup);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.NewInfoGroup);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Key Identifier Change";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.NewInfoGroup.ResumeLayout(false);
            this.NewInfoGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.SelectedGroup.ResumeLayout(false);
            this.SelectedGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox CommentsBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox NewInfoGroup;
        private System.Windows.Forms.Label label3;
        private Uheaa.Common.WinForms.SimpleBorrowerSearchControl BorrowerSearch;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox AccountIdentifierBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Uheaa.Common.WinForms.BorrowerResultsControl BorrowerSearchResults;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button LoadBorrowerButton;
        private System.Windows.Forms.Button RejectButton;
        private System.Windows.Forms.Button ApproveButton;
        private System.Windows.Forms.GroupBox SelectedGroup;
        private System.Windows.Forms.TextBox SelectedDobBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SelectedNameBox;
        private System.Windows.Forms.Label label4;
        private Uheaa.Common.WinForms.AlphaTextBox FirstNameBox;
        private Uheaa.Common.WinForms.AlphaTextBox LastNameBox;
        private Uheaa.Common.WinForms.AlphaTextBox MiddleInitialBox;
        private Uheaa.Common.WinForms.MaskedDateTextBox DobBox;
    }
}