namespace MD
{
    partial class SingleValueManager<T>
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
            this.ManageGroup = new System.Windows.Forms.GroupBox();
            this.QuestionsLabel = new System.Windows.Forms.Label();
            this.QuestionsList = new System.Windows.Forms.ListBox();
            this.NewButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.RenameButton = new System.Windows.Forms.Button();
            this.CollectionList = new System.Windows.Forms.ListBox();
            this.ManageGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ManageGroup
            // 
            this.ManageGroup.Controls.Add(this.QuestionsLabel);
            this.ManageGroup.Controls.Add(this.QuestionsList);
            this.ManageGroup.Controls.Add(this.NewButton);
            this.ManageGroup.Controls.Add(this.DeleteButton);
            this.ManageGroup.Controls.Add(this.RenameButton);
            this.ManageGroup.Controls.Add(this.CollectionList);
            this.ManageGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ManageGroup.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ManageGroup.Location = new System.Drawing.Point(0, 0);
            this.ManageGroup.Name = "ManageGroup";
            this.ManageGroup.Size = new System.Drawing.Size(1061, 323);
            this.ManageGroup.TabIndex = 0;
            this.ManageGroup.TabStop = false;
            this.ManageGroup.Text = "Manage {0}s";
            // 
            // QuestionsLabel
            // 
            this.QuestionsLabel.AutoSize = true;
            this.QuestionsLabel.Location = new System.Drawing.Point(341, 22);
            this.QuestionsLabel.Name = "QuestionsLabel";
            this.QuestionsLabel.Size = new System.Drawing.Size(241, 16);
            this.QuestionsLabel.TabIndex = 16;
            this.QuestionsLabel.Text = "Questions assigned to this Group (0)";
            // 
            // QuestionsList
            // 
            this.QuestionsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuestionsList.DisplayMember = "Question";
            this.QuestionsList.FormattingEnabled = true;
            this.QuestionsList.IntegralHeight = false;
            this.QuestionsList.ItemHeight = 16;
            this.QuestionsList.Location = new System.Drawing.Point(344, 41);
            this.QuestionsList.Name = "QuestionsList";
            this.QuestionsList.Size = new System.Drawing.Size(711, 273);
            this.QuestionsList.TabIndex = 15;
            this.QuestionsList.DoubleClick += new System.EventHandler(this.QuestionsList_DoubleClick);
            // 
            // NewButton
            // 
            this.NewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewButton.Location = new System.Drawing.Point(218, 283);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(120, 31);
            this.NewButton.TabIndex = 14;
            this.NewButton.Text = "New {0}";
            this.NewButton.UseVisualStyleBackColor = true;
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Location = new System.Drawing.Point(6, 283);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(65, 31);
            this.DeleteButton.TabIndex = 13;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // RenameButton
            // 
            this.RenameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RenameButton.Enabled = false;
            this.RenameButton.Location = new System.Drawing.Point(77, 283);
            this.RenameButton.Name = "RenameButton";
            this.RenameButton.Size = new System.Drawing.Size(76, 31);
            this.RenameButton.TabIndex = 11;
            this.RenameButton.Text = "Rename";
            this.RenameButton.UseVisualStyleBackColor = true;
            this.RenameButton.Click += new System.EventHandler(this.RenameButton_Click);
            // 
            // CollectionList
            // 
            this.CollectionList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CollectionList.FormattingEnabled = true;
            this.CollectionList.IntegralHeight = false;
            this.CollectionList.ItemHeight = 16;
            this.CollectionList.Location = new System.Drawing.Point(6, 22);
            this.CollectionList.Name = "CollectionList";
            this.CollectionList.Size = new System.Drawing.Size(332, 255);
            this.CollectionList.TabIndex = 0;
            this.CollectionList.SelectedIndexChanged += new System.EventHandler(this.CollectionList_SelectedIndexChanged);
            // 
            // SingleValueManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ManageGroup);
            this.Name = "SingleValueManager";
            this.Size = new System.Drawing.Size(1061, 323);
            this.ManageGroup.ResumeLayout(false);
            this.ManageGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ManageGroup;
        private System.Windows.Forms.ListBox CollectionList;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button RenameButton;
        private System.Windows.Forms.Label QuestionsLabel;
        private System.Windows.Forms.ListBox QuestionsList;
    }
}
