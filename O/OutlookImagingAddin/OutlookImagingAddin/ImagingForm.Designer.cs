namespace OutlookImagingAddin
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
            this.BorrowerGroup = new System.Windows.Forms.GroupBox();
            this.LetterGroup = new System.Windows.Forms.GroupBox();
            this.LetterBox = new System.Windows.Forms.ComboBox();
            this.ImageButton = new System.Windows.Forms.Button();
            this.BorrowerSearch = new Uheaa.Common.WinForms.SimpleBorrowerSearchControl();
            this.BorrowerResults = new Uheaa.Common.WinForms.BorrowerResultsControl();
            this.BorrowerGroup.SuspendLayout();
            this.LetterGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // BorrowerGroup
            // 
            this.BorrowerGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerGroup.Controls.Add(this.BorrowerSearch);
            this.BorrowerGroup.Controls.Add(this.BorrowerResults);
            this.BorrowerGroup.Location = new System.Drawing.Point(12, 12);
            this.BorrowerGroup.Name = "BorrowerGroup";
            this.BorrowerGroup.Size = new System.Drawing.Size(614, 246);
            this.BorrowerGroup.TabIndex = 0;
            this.BorrowerGroup.TabStop = false;
            this.BorrowerGroup.Text = "Select a Borrower";
            // 
            // LetterGroup
            // 
            this.LetterGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LetterGroup.Controls.Add(this.LetterBox);
            this.LetterGroup.Location = new System.Drawing.Point(12, 264);
            this.LetterGroup.Name = "LetterGroup";
            this.LetterGroup.Size = new System.Drawing.Size(614, 70);
            this.LetterGroup.TabIndex = 1;
            this.LetterGroup.TabStop = false;
            this.LetterGroup.Text = "Select a Letter";
            // 
            // LetterBox
            // 
            this.LetterBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LetterBox.Enabled = false;
            this.LetterBox.FormattingEnabled = true;
            this.LetterBox.Location = new System.Drawing.Point(22, 25);
            this.LetterBox.Name = "LetterBox";
            this.LetterBox.Size = new System.Drawing.Size(572, 28);
            this.LetterBox.TabIndex = 0;
            this.LetterBox.SelectedIndexChanged += new System.EventHandler(this.LetterBox_SelectedIndexChanged);
            // 
            // ImageButton
            // 
            this.ImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageButton.Location = new System.Drawing.Point(12, 370);
            this.ImageButton.Name = "ImageButton";
            this.ImageButton.Size = new System.Drawing.Size(614, 55);
            this.ImageButton.TabIndex = 2;
            this.ImageButton.Text = "Image this Email under Borrower {0}, Letter {1}";
            this.ImageButton.UseVisualStyleBackColor = true;
            this.ImageButton.Click += new System.EventHandler(this.ImageButton_Click);
            // 
            // BorrowerSearch
            // 
            this.BorrowerSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BorrowerSearch.LDA = null;
            this.BorrowerSearch.Location = new System.Drawing.Point(22, 27);
            this.BorrowerSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BorrowerSearch.Name = "BorrowerSearch";
            this.BorrowerSearch.Size = new System.Drawing.Size(585, 33);
            this.BorrowerSearch.TabIndex = 2;
            this.BorrowerSearch.OnSearchResultsRetrieved += new Uheaa.Common.WinForms.SimpleBorrowerSearchControl.SearchResultsRetrieved(this.BorrowerSearch_OnSearchResultsRetrieved);
            // 
            // BorrowerResults
            // 
            this.BorrowerResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerResults.ChooseBorrowerOnSelection = true;
            this.BorrowerResults.EmailsColumnVisible = false;
            this.BorrowerResults.Location = new System.Drawing.Point(7, 70);
            this.BorrowerResults.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BorrowerResults.Name = "BorrowerResults";
            this.BorrowerResults.PhoneColumnsVisible = false;
            this.BorrowerResults.Size = new System.Drawing.Size(600, 168);
            this.BorrowerResults.TabIndex = 1;
            this.BorrowerResults.OnBorrowerChosen += new Uheaa.Common.WinForms.BorrowerResultsControl.SelectedBorrowerDelegate(this.BorrowerResults_OnBorrowerChosen);
            // 
            // ImagingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 440);
            this.Controls.Add(this.ImageButton);
            this.Controls.Add(this.LetterGroup);
            this.Controls.Add(this.BorrowerGroup);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImagingForm";
            this.Text = " Image this Email";
            this.BorrowerGroup.ResumeLayout(false);
            this.LetterGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox BorrowerGroup;
        private System.Windows.Forms.GroupBox LetterGroup;
        private System.Windows.Forms.Button ImageButton;
        private Uheaa.Common.WinForms.BorrowerResultsControl BorrowerResults;
        private Uheaa.Common.WinForms.SimpleBorrowerSearchControl BorrowerSearch;
        private System.Windows.Forms.ComboBox LetterBox;
    }
}