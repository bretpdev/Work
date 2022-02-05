namespace MD
{
    partial class QuestionFilter
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
            this.FilterGroup = new System.Windows.Forms.GroupBox();
            this.QuestionsList = new System.Windows.Forms.ListBox();
            this.SearchPortfoliosList = new System.Windows.Forms.CheckedListBox();
            this.SearchGroupsList = new System.Windows.Forms.ComboBox();
            this.SearchBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.FilterGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // FilterGroup
            // 
            this.FilterGroup.Controls.Add(this.QuestionsList);
            this.FilterGroup.Controls.Add(this.SearchPortfoliosList);
            this.FilterGroup.Controls.Add(this.SearchGroupsList);
            this.FilterGroup.Controls.Add(this.SearchBox);
            this.FilterGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterGroup.Location = new System.Drawing.Point(0, 0);
            this.FilterGroup.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.FilterGroup.Name = "FilterGroup";
            this.FilterGroup.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.FilterGroup.Size = new System.Drawing.Size(827, 228);
            this.FilterGroup.TabIndex = 21;
            this.FilterGroup.TabStop = false;
            this.FilterGroup.Text = "Find a Question";
            // 
            // QuestionsList
            // 
            this.QuestionsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuestionsList.FormattingEnabled = true;
            this.QuestionsList.IntegralHeight = false;
            this.QuestionsList.ItemHeight = 16;
            this.QuestionsList.Location = new System.Drawing.Point(269, 23);
            this.QuestionsList.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.QuestionsList.Name = "QuestionsList";
            this.QuestionsList.Size = new System.Drawing.Size(548, 195);
            this.QuestionsList.TabIndex = 18;
            this.QuestionsList.Click += new System.EventHandler(this.QuestionsList_Click);
            this.QuestionsList.SelectedIndexChanged += new System.EventHandler(this.QuestionsList_SelectedIndexChanged);
            // 
            // SearchPortfoliosList
            // 
            this.SearchPortfoliosList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SearchPortfoliosList.CheckOnClick = true;
            this.SearchPortfoliosList.FormattingEnabled = true;
            this.SearchPortfoliosList.IntegralHeight = false;
            this.SearchPortfoliosList.Location = new System.Drawing.Point(6, 87);
            this.SearchPortfoliosList.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.SearchPortfoliosList.Name = "SearchPortfoliosList";
            this.SearchPortfoliosList.Size = new System.Drawing.Size(255, 131);
            this.SearchPortfoliosList.TabIndex = 4;
            this.SearchPortfoliosList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SearchPortfoliosList_ItemCheck);
            // 
            // SearchGroupsList
            // 
            this.SearchGroupsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SearchGroupsList.FormattingEnabled = true;
            this.SearchGroupsList.Location = new System.Drawing.Point(6, 56);
            this.SearchGroupsList.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.SearchGroupsList.Name = "SearchGroupsList";
            this.SearchGroupsList.Size = new System.Drawing.Size(255, 24);
            this.SearchGroupsList.TabIndex = 3;
            this.SearchGroupsList.SelectedIndexChanged += new System.EventHandler(this.SearchGroupsList_SelectedIndexChanged);
            // 
            // SearchBox
            // 
            this.SearchBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic);
            this.SearchBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.SearchBox.Location = new System.Drawing.Point(6, 23);
            this.SearchBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(255, 23);
            this.SearchBox.TabIndex = 3;
            this.SearchBox.Text = "Search Terms";
            this.SearchBox.Watermark = "Search Terms";
            this.SearchBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchBox_KeyUp);
            // 
            // QuestionFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FilterGroup);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "QuestionFilter";
            this.Size = new System.Drawing.Size(827, 228);
            this.FilterGroup.ResumeLayout(false);
            this.FilterGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox FilterGroup;
        private System.Windows.Forms.ListBox QuestionsList;
        private System.Windows.Forms.CheckedListBox SearchPortfoliosList;
        private System.Windows.Forms.ComboBox SearchGroupsList;
        private Uheaa.Common.WinForms.WatermarkTextBox SearchBox;
    }
}
