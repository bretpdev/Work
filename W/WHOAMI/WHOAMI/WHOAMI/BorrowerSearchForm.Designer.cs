namespace WHOAMI
{
    partial class BorrowerSearchForm
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
            this.BorrowerResults = new Uheaa.Common.WinForms.BorrowerResultsControl();
            this.borrowerSearchControl1 = new Uheaa.Common.WinForms.BorrowerSearchControl();
            this.SuspendLayout();
            // 
            // BorrowerResults
            // 
            this.BorrowerResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerResults.ChooseBorrowerOnSelection = false;
            this.BorrowerResults.EmailsColumnVisible = true;
            this.BorrowerResults.Location = new System.Drawing.Point(12, 248);
            this.BorrowerResults.Name = "BorrowerResults";
            this.BorrowerResults.PhoneColumnsVisible = true;
            this.BorrowerResults.Size = new System.Drawing.Size(1239, 277);
            this.BorrowerResults.TabIndex = 1;
            this.BorrowerResults.OnSelectionChanged += new Uheaa.Common.WinForms.BorrowerResultsControl.SelectedBorrowerDelegate(this.BorrowerResults_OnSelectionChanged);
            this.BorrowerResults.OnBorrowerChosen += new Uheaa.Common.WinForms.BorrowerResultsControl.SelectedBorrowerDelegate(this.BorrowerResults_OnBorrowerChosen);
            // 
            // borrowerSearchControl1
            // 
            this.borrowerSearchControl1.LDA = null;
            this.borrowerSearchControl1.Location = new System.Drawing.Point(430, 12);
            this.borrowerSearchControl1.Name = "borrowerSearchControl1";
            this.borrowerSearchControl1.OnelinkEnabled = true;
            this.borrowerSearchControl1.Size = new System.Drawing.Size(370, 230);
            this.borrowerSearchControl1.TabIndex = 4;
            this.borrowerSearchControl1.UheaaEnabled = true;
            this.borrowerSearchControl1.OnSearchResultsRetrieved += new Uheaa.Common.WinForms.BorrowerSearchControl.SearchResultsRetrieved(this.BorrowerSearch_OnSearchResultsRetrieved);
            // 
            // BorrowerSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 534);
            this.Controls.Add(this.borrowerSearchControl1);
            this.Controls.Add(this.BorrowerResults);
            this.MinimumSize = new System.Drawing.Size(1277, 573);
            this.Name = "BorrowerSearchForm";
            this.Text = "Borrower Search";
            this.ResumeLayout(false);

        }

        #endregion

        private Uheaa.Common.WinForms.BorrowerResultsControl BorrowerResults;
        private Uheaa.Common.WinForms.BorrowerSearchControl borrowerSearchControl1;
    }
}