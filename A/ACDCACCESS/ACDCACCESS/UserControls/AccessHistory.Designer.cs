namespace ACDCAccess
{
    partial class AccessHistory
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
            this.label2 = new System.Windows.Forms.Label();
            this.pnlHistory = new System.Windows.Forms.FlowLayoutPanel();
            this.accessHistoryDetail1 = new ACDCAccess.AccessHistoryDetail();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbUsers = new Q.ComboBoxWithAutoCompleteExtended();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "User";
            // 
            // pnlHistory
            // 
            this.pnlHistory.AutoScroll = true;
            this.pnlHistory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlHistory.Location = new System.Drawing.Point(155, 50);
            this.pnlHistory.Name = "pnlHistory";
            this.pnlHistory.Size = new System.Drawing.Size(867, 533);
            this.pnlHistory.TabIndex = 12;
            // 
            // accessHistoryDetail1
            // 
            this.accessHistoryDetail1.AutoSize = true;
            this.accessHistoryDetail1.Location = new System.Drawing.Point(161, 30);
            this.accessHistoryDetail1.MaximumSize = new System.Drawing.Size(850, 20);
            this.accessHistoryDetail1.MinimumSize = new System.Drawing.Size(850, 20);
            this.accessHistoryDetail1.Name = "accessHistoryDetail1";
            this.accessHistoryDetail1.Size = new System.Drawing.Size(850, 20);
            this.accessHistoryDetail1.TabIndex = 13;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(491, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbUsers
            // 
            this.cmbUsers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbUsers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
            this.cmbUsers.Location = new System.Drawing.Point(155, 7);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(330, 21);
            this.cmbUsers.TabIndex = 1;
            // 
            // AccessHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbUsers);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.accessHistoryDetail1);
            this.Controls.Add(this.pnlHistory);
            this.Controls.Add(this.label2);
            this.Name = "AccessHistory";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel pnlHistory;
        private AccessHistoryDetail accessHistoryDetail1;
        private System.Windows.Forms.Button btnSearch;
        private Q.ComboBoxWithAutoCompleteExtended cmbUsers;
    }
}
