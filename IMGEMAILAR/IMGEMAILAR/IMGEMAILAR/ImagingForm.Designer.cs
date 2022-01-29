namespace IMGEMAILAR
{
    partial class ImagingForm
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
            this.BorrowerGroup = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AccountIdentifierBox = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.AccountIdentifierButton = new System.Windows.Forms.Button();
            this.BorrowerSearch = new Uheaa.Common.WinForms.DynamicBorrowerSearchControl();
            this.BorrowerResults = new Uheaa.Common.WinForms.BorrowerResultsControl();
            this.LetterGroup = new System.Windows.Forms.GroupBox();
            this.LetterBox = new System.Windows.Forms.ComboBox();
            this.ImageButton = new System.Windows.Forms.Button();
            this.EmailPasteGroup = new System.Windows.Forms.GroupBox();
            this.AttachmentsButton = new System.Windows.Forms.Button();
            this.CopyButton = new System.Windows.Forms.Button();
            this.PreviewBrowser = new System.Windows.Forms.WebBrowser();
            this.ResetButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ActivityCommentBox = new System.Windows.Forms.TextBox();
            this.TempTimer = new System.Windows.Forms.Timer(this.components);
            this.BorrowerGroup.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.LetterGroup.SuspendLayout();
            this.EmailPasteGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BorrowerGroup
            // 
            this.BorrowerGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.BorrowerGroup.Controls.Add(this.groupBox2);
            this.BorrowerGroup.Controls.Add(this.BorrowerSearch);
            this.BorrowerGroup.Controls.Add(this.BorrowerResults);
            this.BorrowerGroup.Location = new System.Drawing.Point(12, 12);
            this.BorrowerGroup.Name = "BorrowerGroup";
            this.BorrowerGroup.Size = new System.Drawing.Size(555, 376);
            this.BorrowerGroup.TabIndex = 0;
            this.BorrowerGroup.TabStop = false;
            this.BorrowerGroup.Text = "Select a Borrower";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AccountIdentifierBox);
            this.groupBox2.Controls.Add(this.AccountIdentifierButton);
            this.groupBox2.Location = new System.Drawing.Point(7, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(464, 58);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SSN or Account Number";
            // 
            // AccountIdentifierBox
            // 
            this.AccountIdentifierBox.AccountNumber = null;
            this.AccountIdentifierBox.AllowedSpecialCharacters = "";
            this.AccountIdentifierBox.Location = new System.Drawing.Point(6, 22);
            this.AccountIdentifierBox.MaxLength = 10;
            this.AccountIdentifierBox.Name = "AccountIdentifierBox";
            this.AccountIdentifierBox.Size = new System.Drawing.Size(246, 26);
            this.AccountIdentifierBox.Ssn = null;
            this.AccountIdentifierBox.TabIndex = 0;
            this.AccountIdentifierBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.AccountIdentifierBox_PreviewKeyDown);
            // 
            // AccountIdentifierButton
            // 
            this.AccountIdentifierButton.Location = new System.Drawing.Point(258, 20);
            this.AccountIdentifierButton.Name = "AccountIdentifierButton";
            this.AccountIdentifierButton.Size = new System.Drawing.Size(200, 30);
            this.AccountIdentifierButton.TabIndex = 1;
            this.AccountIdentifierButton.Text = "Load Borrower";
            this.AccountIdentifierButton.UseVisualStyleBackColor = true;
            this.AccountIdentifierButton.Click += new System.EventHandler(this.AccountIdentifierButton_Click);
            // 
            // BorrowerSearch
            // 
            this.BorrowerSearch.AddressEnabled = false;
            this.BorrowerSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerSearch.CityEnabled = false;
            this.BorrowerSearch.DOBEnabled = false;
            this.BorrowerSearch.EmailWidth = 3.1D;
            this.BorrowerSearch.FirstNameWidth = 1.525D;
            this.BorrowerSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BorrowerSearch.LastNameWidth = 1.525D;
            this.BorrowerSearch.LDA = null;
            this.BorrowerSearch.Location = new System.Drawing.Point(7, 84);
            this.BorrowerSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BorrowerSearch.MiddleInitialEnabled = false;
            this.BorrowerSearch.Name = "BorrowerSearch";
            this.BorrowerSearch.OnelinkEnabled = true;
            this.BorrowerSearch.PhoneEnabled = false;
            this.BorrowerSearch.Size = new System.Drawing.Size(541, 124);
            this.BorrowerSearch.StateEnabled = false;
            this.BorrowerSearch.TabIndex = 0;
            this.BorrowerSearch.UheaaEnabled = true;
            this.BorrowerSearch.ZipEnabled = false;
            this.BorrowerSearch.OnSearchResultsRetrieved += new Uheaa.Common.WinForms.DynamicBorrowerSearchControl.SearchResultsRetrieved(this.BorrowerSearch_OnSearchResultsRetrieved);
            // 
            // BorrowerResults
            // 
            this.BorrowerResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerResults.ChooseBorrowerOnSelection = true;
            this.BorrowerResults.EmailsColumnVisible = false;
            this.BorrowerResults.Location = new System.Drawing.Point(7, 207);
            this.BorrowerResults.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BorrowerResults.Name = "BorrowerResults";
            this.BorrowerResults.PhoneColumnsVisible = false;
            this.BorrowerResults.Size = new System.Drawing.Size(541, 161);
            this.BorrowerResults.TabIndex = 1;
            this.BorrowerResults.OnBorrowerChosen += new Uheaa.Common.WinForms.BorrowerResultsControl.SelectedBorrowerDelegate(this.BorrowerResults_OnBorrowerChosen);
            // 
            // LetterGroup
            // 
            this.LetterGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LetterGroup.Controls.Add(this.LetterBox);
            this.LetterGroup.Location = new System.Drawing.Point(12, 394);
            this.LetterGroup.Name = "LetterGroup";
            this.LetterGroup.Size = new System.Drawing.Size(548, 70);
            this.LetterGroup.TabIndex = 1;
            this.LetterGroup.TabStop = false;
            this.LetterGroup.Text = "Select a Letter";
            // 
            // LetterBox
            // 
            this.LetterBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LetterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LetterBox.Enabled = false;
            this.LetterBox.FormattingEnabled = true;
            this.LetterBox.Location = new System.Drawing.Point(22, 25);
            this.LetterBox.Name = "LetterBox";
            this.LetterBox.Size = new System.Drawing.Size(519, 28);
            this.LetterBox.TabIndex = 0;
            this.LetterBox.SelectedIndexChanged += new System.EventHandler(this.LetterBox_SelectedIndexChanged);
            // 
            // ImageButton
            // 
            this.ImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageButton.Location = new System.Drawing.Point(311, 473);
            this.ImageButton.Name = "ImageButton";
            this.ImageButton.Size = new System.Drawing.Size(709, 55);
            this.ImageButton.TabIndex = 4;
            this.ImageButton.Text = "Image this Email under Borrower {0}, Letter {1}";
            this.ImageButton.UseVisualStyleBackColor = true;
            this.ImageButton.Click += new System.EventHandler(this.ImageButton_Click);
            // 
            // EmailPasteGroup
            // 
            this.EmailPasteGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EmailPasteGroup.Controls.Add(this.AttachmentsButton);
            this.EmailPasteGroup.Controls.Add(this.CopyButton);
            this.EmailPasteGroup.Controls.Add(this.PreviewBrowser);
            this.EmailPasteGroup.Location = new System.Drawing.Point(573, 12);
            this.EmailPasteGroup.Name = "EmailPasteGroup";
            this.EmailPasteGroup.Size = new System.Drawing.Size(447, 326);
            this.EmailPasteGroup.TabIndex = 2;
            this.EmailPasteGroup.TabStop = false;
            this.EmailPasteGroup.Text = "Paste Contents Here";
            // 
            // AttachmentsButton
            // 
            this.AttachmentsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AttachmentsButton.Location = new System.Drawing.Point(238, 25);
            this.AttachmentsButton.Name = "AttachmentsButton";
            this.AttachmentsButton.Size = new System.Drawing.Size(203, 38);
            this.AttachmentsButton.TabIndex = 2;
            this.AttachmentsButton.Text = "Add Attachments";
            this.AttachmentsButton.UseVisualStyleBackColor = true;
            this.AttachmentsButton.Click += new System.EventHandler(this.AttachmentsButton_Click);
            // 
            // CopyButton
            // 
            this.CopyButton.Location = new System.Drawing.Point(6, 26);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(226, 38);
            this.CopyButton.TabIndex = 0;
            this.CopyButton.Text = "Paste Copied Content";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // PreviewBrowser
            // 
            this.PreviewBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PreviewBrowser.Location = new System.Drawing.Point(3, 84);
            this.PreviewBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.PreviewBrowser.Name = "PreviewBrowser";
            this.PreviewBrowser.Size = new System.Drawing.Size(441, 239);
            this.PreviewBrowser.TabIndex = 1;
            this.PreviewBrowser.Url = new System.Uri("", System.UriKind.Relative);
            this.PreviewBrowser.Visible = false;
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResetButton.Location = new System.Drawing.Point(12, 473);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(293, 55);
            this.ResetButton.TabIndex = 5;
            this.ResetButton.Text = "Cancel and Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ActivityCommentBox);
            this.groupBox1.Location = new System.Drawing.Point(573, 344);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 120);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Activity Comment";
            // 
            // ActivityCommentBox
            // 
            this.ActivityCommentBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActivityCommentBox.Location = new System.Drawing.Point(3, 22);
            this.ActivityCommentBox.MaxLength = 120;
            this.ActivityCommentBox.Multiline = true;
            this.ActivityCommentBox.Name = "ActivityCommentBox";
            this.ActivityCommentBox.Size = new System.Drawing.Size(441, 95);
            this.ActivityCommentBox.TabIndex = 0;
            // 
            // TempTimer
            // 
            this.TempTimer.Enabled = true;
            this.TempTimer.Interval = 3000;
            this.TempTimer.Tick += new System.EventHandler(this.TempTimer_Tick);
            // 
            // ImagingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 540);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.EmailPasteGroup);
            this.Controls.Add(this.ImageButton);
            this.Controls.Add(this.LetterGroup);
            this.Controls.Add(this.BorrowerGroup);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1048, 579);
            this.Name = "ImagingForm";
            this.Text = "Imaging Archive";
            this.BorrowerGroup.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.LetterGroup.ResumeLayout(false);
            this.EmailPasteGroup.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox BorrowerGroup;
        private System.Windows.Forms.GroupBox LetterGroup;
        private System.Windows.Forms.Button ImageButton;
        private Uheaa.Common.WinForms.BorrowerResultsControl BorrowerResults;
        private System.Windows.Forms.ComboBox LetterBox;
        private System.Windows.Forms.GroupBox EmailPasteGroup;
        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.WebBrowser PreviewBrowser;
        private Uheaa.Common.WinForms.DynamicBorrowerSearchControl BorrowerSearch;
        private System.Windows.Forms.Button AttachmentsButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ActivityCommentBox;
        private System.Windows.Forms.Timer TempTimer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button AccountIdentifierButton;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox AccountIdentifierBox;
    }
}