namespace MD
{
    partial class ExistingQuestionsTab
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
            this.AnswerBox = new RTF.RTFEditor();
            this.AssignedPortfoliosLabel = new System.Windows.Forms.Label();
            this.AssignedGroupLabel = new System.Windows.Forms.Label();
            this.AssignedGroupsList = new System.Windows.Forms.ListBox();
            this.AssignedPortfoliosList = new System.Windows.Forms.CheckedListBox();
            this.QuestionBox = new System.Windows.Forms.TextBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.LastEditedLabel = new System.Windows.Forms.Label();
            this.AnswerLabel = new System.Windows.Forms.Label();
            this.QuestionLabel = new System.Windows.Forms.Label();
            this.QuestionFilterBox = new MD.QuestionFilter();
            this.SuspendLayout();
            // 
            // AnswerBox
            // 
            this.AnswerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnswerBox.DefaultBackgroundColor = System.Drawing.Color.White;
            this.AnswerBox.DefaultFontColor = System.Drawing.Color.Black;
            this.AnswerBox.DefaultFontFamily = "Arial";
            this.AnswerBox.DefaultFontSize = 8;
            this.AnswerBox.DefaultWordWrap = true;
            this.AnswerBox.DocumentRtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Arial;}}\r\n\\" +
    "viewkind4\\uc1\\pard\\fs17\\par\r\n}\r\n";
            this.AnswerBox.DocumentText = "";
            this.AnswerBox.Location = new System.Drawing.Point(249, 318);
            this.AnswerBox.Margin = new System.Windows.Forms.Padding(4);
            this.AnswerBox.Name = "AnswerBox";
            this.AnswerBox.ReadOnly = false;
            this.AnswerBox.Size = new System.Drawing.Size(796, 324);
            this.AnswerBox.TabIndex = 32;
            this.AnswerBox.ToolStripVisible = true;
            // 
            // AssignedPortfoliosLabel
            // 
            this.AssignedPortfoliosLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AssignedPortfoliosLabel.AutoSize = true;
            this.AssignedPortfoliosLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssignedPortfoliosLabel.Location = new System.Drawing.Point(6, 489);
            this.AssignedPortfoliosLabel.Name = "AssignedPortfoliosLabel";
            this.AssignedPortfoliosLabel.Size = new System.Drawing.Size(145, 20);
            this.AssignedPortfoliosLabel.TabIndex = 31;
            this.AssignedPortfoliosLabel.Text = "Assigned Portfolios";
            // 
            // AssignedGroupLabel
            // 
            this.AssignedGroupLabel.AutoSize = true;
            this.AssignedGroupLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssignedGroupLabel.Location = new System.Drawing.Point(6, 201);
            this.AssignedGroupLabel.Name = "AssignedGroupLabel";
            this.AssignedGroupLabel.Size = new System.Drawing.Size(124, 20);
            this.AssignedGroupLabel.TabIndex = 30;
            this.AssignedGroupLabel.Text = "Assigned Group";
            // 
            // AssignedGroupsList
            // 
            this.AssignedGroupsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.AssignedGroupsList.FormattingEnabled = true;
            this.AssignedGroupsList.ItemHeight = 16;
            this.AssignedGroupsList.Location = new System.Drawing.Point(8, 224);
            this.AssignedGroupsList.Name = "AssignedGroupsList";
            this.AssignedGroupsList.Size = new System.Drawing.Size(235, 260);
            this.AssignedGroupsList.TabIndex = 29;
            this.AssignedGroupsList.SelectedIndexChanged += new System.EventHandler(this.AssignedGroupsList_SelectedIndexChanged);
            // 
            // AssignedPortfoliosList
            // 
            this.AssignedPortfoliosList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AssignedPortfoliosList.CheckOnClick = true;
            this.AssignedPortfoliosList.FormattingEnabled = true;
            this.AssignedPortfoliosList.Location = new System.Drawing.Point(8, 512);
            this.AssignedPortfoliosList.Name = "AssignedPortfoliosList";
            this.AssignedPortfoliosList.Size = new System.Drawing.Size(234, 130);
            this.AssignedPortfoliosList.TabIndex = 28;
            this.AssignedPortfoliosList.SelectedIndexChanged += new System.EventHandler(this.AssignedPortfoliosList_SelectedIndexChanged);
            // 
            // QuestionBox
            // 
            this.QuestionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuestionBox.Location = new System.Drawing.Point(249, 224);
            this.QuestionBox.Multiline = true;
            this.QuestionBox.Name = "QuestionBox";
            this.QuestionBox.Size = new System.Drawing.Size(803, 67);
            this.QuestionBox.TabIndex = 27;
            this.QuestionBox.TextChanged += new System.EventHandler(this.QuestionBox_TextChanged);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteButton.Location = new System.Drawing.Point(681, 651);
            this.DeleteButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(94, 43);
            this.DeleteButton.TabIndex = 26;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // NewButton
            // 
            this.NewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewButton.Location = new System.Drawing.Point(10, 649);
            this.NewButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(132, 43);
            this.NewButton.TabIndex = 25;
            this.NewButton.Text = "New Question";
            this.NewButton.UseVisualStyleBackColor = true;
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.Location = new System.Drawing.Point(781, 651);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(132, 43);
            this.CancelButton.TabIndex = 24;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(919, 651);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(132, 43);
            this.SaveButton.TabIndex = 23;
            this.SaveButton.Text = "Save Changes";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // LastEditedLabel
            // 
            this.LastEditedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LastEditedLabel.AutoSize = true;
            this.LastEditedLabel.Location = new System.Drawing.Point(252, 662);
            this.LastEditedLabel.Name = "LastEditedLabel";
            this.LastEditedLabel.Size = new System.Drawing.Size(285, 17);
            this.LastEditedLabel.TabIndex = 22;
            this.LastEditedLabel.Text = "Last edited on 1/1/2013 by Uheaa\\TestUser";
            // 
            // AnswerLabel
            // 
            this.AnswerLabel.AutoSize = true;
            this.AnswerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnswerLabel.Location = new System.Drawing.Point(245, 294);
            this.AnswerLabel.Name = "AnswerLabel";
            this.AnswerLabel.Size = new System.Drawing.Size(62, 20);
            this.AnswerLabel.TabIndex = 21;
            this.AnswerLabel.Text = "Answer";
            // 
            // QuestionLabel
            // 
            this.QuestionLabel.AutoSize = true;
            this.QuestionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestionLabel.Location = new System.Drawing.Point(245, 201);
            this.QuestionLabel.Name = "QuestionLabel";
            this.QuestionLabel.Size = new System.Drawing.Size(73, 20);
            this.QuestionLabel.TabIndex = 20;
            this.QuestionLabel.Text = "Question";
            // 
            // QuestionFilterBox
            // 
            this.QuestionFilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuestionFilterBox.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestionFilterBox.Location = new System.Drawing.Point(10, 4);
            this.QuestionFilterBox.Margin = new System.Windows.Forms.Padding(4);
            this.QuestionFilterBox.Name = "QuestionFilterBox";
            this.QuestionFilterBox.Size = new System.Drawing.Size(1041, 193);
            this.QuestionFilterBox.TabIndex = 33;
            this.QuestionFilterBox.QuestionSelected += new MD.QuestionFilter.OnQuestionSelected(this.QuestionFilterBox_QuestionSelected);
            // 
            // ExistingQuestionsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.QuestionFilterBox);
            this.Controls.Add(this.AnswerBox);
            this.Controls.Add(this.AssignedPortfoliosLabel);
            this.Controls.Add(this.AssignedGroupLabel);
            this.Controls.Add(this.AssignedGroupsList);
            this.Controls.Add(this.AssignedPortfoliosList);
            this.Controls.Add(this.QuestionBox);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LastEditedLabel);
            this.Controls.Add(this.AnswerLabel);
            this.Controls.Add(this.QuestionLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ExistingQuestionsTab";
            this.Size = new System.Drawing.Size(1061, 704);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RTF.RTFEditor AnswerBox;
        private System.Windows.Forms.Label AssignedPortfoliosLabel;
        private System.Windows.Forms.Label AssignedGroupLabel;
        private System.Windows.Forms.ListBox AssignedGroupsList;
        private System.Windows.Forms.CheckedListBox AssignedPortfoliosList;
        private System.Windows.Forms.TextBox QuestionBox;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label LastEditedLabel;
        private System.Windows.Forms.Label AnswerLabel;
        private System.Windows.Forms.Label QuestionLabel;
        private QuestionFilter QuestionFilterBox;
    }
}
