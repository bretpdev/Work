namespace ACDCAccess
{
    partial class Research
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtKeyWordSearch = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.pnlResults = new System.Windows.Forms.FlowLayoutPanel();
			this.label5 = new System.Windows.Forms.Label();
			this.researchResult1 = new ACDCAccess.ResearchResult();
			this.cmbApplication = new Q.ComboBoxWithAutoCompleteExtended();
			this.cmbType = new Q.ComboBoxWithAutoCompleteExtended();
			this.cmbFieldsToSearch = new Q.ComboBoxWithAutoCompleteExtended();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Application";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Type";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(54, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Key Word";
			// 
			// txtKeyWordSearch
			// 
			this.txtKeyWordSearch.Location = new System.Drawing.Point(154, 53);
			this.txtKeyWordSearch.Name = "txtKeyWordSearch";
			this.txtKeyWordSearch.Size = new System.Drawing.Size(330, 20);
			this.txtKeyWordSearch.TabIndex = 4;
			this.txtKeyWordSearch.TextChanged += new System.EventHandler(this.txtKeyWordSearch_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 79);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(83, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Fields to Search";
			// 
			// pnlResults
			// 
			this.pnlResults.AutoScroll = true;
			this.pnlResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlResults.Location = new System.Drawing.Point(154, 126);
			this.pnlResults.Name = "pnlResults";
			this.pnlResults.Size = new System.Drawing.Size(861, 457);
			this.pnlResults.TabIndex = 13;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 109);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "Results";
			// 
			// researchResult1
			// 
			this.researchResult1.AutoSize = true;
			this.researchResult1.Location = new System.Drawing.Point(159, 104);
			this.researchResult1.MaximumSize = new System.Drawing.Size(840, 0);
			this.researchResult1.MinimumSize = new System.Drawing.Size(840, 20);
			this.researchResult1.Name = "researchResult1";
			this.researchResult1.Size = new System.Drawing.Size(840, 20);
			this.researchResult1.TabIndex = 15;
			this.researchResult1.TabStop = false;
			// 
			// cmbApplication
			// 
			this.cmbApplication.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbApplication.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbApplication.FormattingEnabled = true;
			this.cmbApplication.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbApplication.Location = new System.Drawing.Point(154, 6);
			this.cmbApplication.Name = "cmbApplication";
			this.cmbApplication.Size = new System.Drawing.Size(330, 21);
			this.cmbApplication.TabIndex = 0;
			this.cmbApplication.SelectedIndexChanged += new System.EventHandler(this.cmbSystem_SelectedIndexChanged);
			// 
			// cmbType
			// 
			this.cmbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbType.FormattingEnabled = true;
			this.cmbType.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbType.Location = new System.Drawing.Point(154, 30);
			this.cmbType.Name = "cmbType";
			this.cmbType.Size = new System.Drawing.Size(330, 21);
			this.cmbType.TabIndex = 2;
			// 
			// cmbFieldsToSearch
			// 
			this.cmbFieldsToSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbFieldsToSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbFieldsToSearch.FormattingEnabled = true;
			this.cmbFieldsToSearch.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbFieldsToSearch.Items.AddRange(new object[] {
            "Key",
            "Description",
            "Key And Description"});
			this.cmbFieldsToSearch.Location = new System.Drawing.Point(154, 76);
			this.cmbFieldsToSearch.Name = "cmbFieldsToSearch";
			this.cmbFieldsToSearch.Size = new System.Drawing.Size(330, 21);
			this.cmbFieldsToSearch.TabIndex = 10;
			this.cmbFieldsToSearch.SelectedIndexChanged += new System.EventHandler(this.cmbFieldsToSearch_SelectedIndexChanged);
			// 
			// Research
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cmbFieldsToSearch);
			this.Controls.Add(this.cmbType);
			this.Controls.Add(this.cmbApplication);
			this.Controls.Add(this.researchResult1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.pnlResults);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtKeyWordSearch);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Research";
			this.Load += new System.EventHandler(this.Research_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeyWordSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel pnlResults;
        private System.Windows.Forms.Label label5;
        private ResearchResult researchResult1;
        private Q.ComboBoxWithAutoCompleteExtended cmbApplication;
        private Q.ComboBoxWithAutoCompleteExtended cmbType;
        private Q.ComboBoxWithAutoCompleteExtended cmbFieldsToSearch;
    }
}
