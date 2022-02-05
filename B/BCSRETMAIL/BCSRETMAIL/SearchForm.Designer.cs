namespace BCSRETMAIL
{
    partial class SearchForm
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
            this.BorrowerSearch = new Uheaa.Common.WinForms.BorrowerSearchControl();
            this.BorrowerResults = new Uheaa.Common.WinForms.BorrowerResultsControl();
            this.SuspendLayout();
            // 
            // BorrowerSearch
            // 
            this.BorrowerSearch.Location = new System.Drawing.Point(12, 12);
            this.BorrowerSearch.Name = "BorrowerSearch";
            this.BorrowerSearch.Size = new System.Drawing.Size(370, 230);
            this.BorrowerSearch.TabIndex = 0;
            this.BorrowerSearch.OnSearchResultsRetrieved += new Uheaa.Common.WinForms.BorrowerSearchControl.SearchResultsRetrieved(this.BorrowerSearch_OnSearchResultsRetrieved);
            // 
            // BorrowerResults
            // 
            this.BorrowerResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BorrowerResults.Location = new System.Drawing.Point(388, 26);
            this.BorrowerResults.Name = "BorrowerResults";
            this.BorrowerResults.Size = new System.Drawing.Size(951, 216);
            this.BorrowerResults.TabIndex = 1;
            this.BorrowerResults.OnBorrowerChosen += new Uheaa.Common.WinForms.BorrowerResultsControl.SelectedBorrowerDelegate(this.BorrowerResults_OnBorrowerChosen);
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1351, 257);
            this.Controls.Add(this.BorrowerResults);
            this.Controls.Add(this.BorrowerSearch);
            this.MinimumSize = new System.Drawing.Size(1367, 295);
            this.Name = "SearchForm";
            this.Text = "Search";
            this.ResumeLayout(false);

        }

        #endregion

        private Uheaa.Common.WinForms.BorrowerSearchControl BorrowerSearch;
        private Uheaa.Common.WinForms.BorrowerResultsControl BorrowerResults;
    }
}