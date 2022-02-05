namespace MD
{
    partial class FaqViewerForm
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
            this.Tabs = new System.Windows.Forms.TabControl();
            this.AllQuestionsTab = new System.Windows.Forms.TabPage();
            this.QuestionAnswerPanel = new System.Windows.Forms.TableLayoutPanel();
            this.QuestionLabel = new System.Windows.Forms.Label();
            this.AnswerBox = new System.Windows.Forms.RichTextBox();
            this.LastEditedLabel = new System.Windows.Forms.Label();
            this.RelatedPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SearchQuestionsBox = new MD.QuestionFilter();
            this.MyQuestionsTab = new System.Windows.Forms.TabPage();
            this.PendingQuestionsControl = new MD.PendingQuestions();
            this.Tabs.SuspendLayout();
            this.AllQuestionsTab.SuspendLayout();
            this.QuestionAnswerPanel.SuspendLayout();
            this.RelatedPanel.SuspendLayout();
            this.MyQuestionsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.AllQuestionsTab);
            this.Tabs.Controls.Add(this.MyQuestionsTab);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(1077, 697);
            this.Tabs.TabIndex = 24;
            // 
            // AllQuestionsTab
            // 
            this.AllQuestionsTab.BackColor = System.Drawing.SystemColors.Control;
            this.AllQuestionsTab.Controls.Add(this.QuestionAnswerPanel);
            this.AllQuestionsTab.Controls.Add(this.LastEditedLabel);
            this.AllQuestionsTab.Controls.Add(this.RelatedPanel);
            this.AllQuestionsTab.Controls.Add(this.SearchQuestionsBox);
            this.AllQuestionsTab.Location = new System.Drawing.Point(4, 25);
            this.AllQuestionsTab.Name = "AllQuestionsTab";
            this.AllQuestionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.AllQuestionsTab.Size = new System.Drawing.Size(1069, 668);
            this.AllQuestionsTab.TabIndex = 0;
            this.AllQuestionsTab.Text = "All Questions";
            // 
            // QuestionAnswerPanel
            // 
            this.QuestionAnswerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuestionAnswerPanel.ColumnCount = 1;
            this.QuestionAnswerPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.QuestionAnswerPanel.Controls.Add(this.QuestionLabel, 0, 0);
            this.QuestionAnswerPanel.Controls.Add(this.AnswerBox, 0, 1);
            this.QuestionAnswerPanel.Location = new System.Drawing.Point(6, 228);
            this.QuestionAnswerPanel.Name = "QuestionAnswerPanel";
            this.QuestionAnswerPanel.RowCount = 2;
            this.QuestionAnswerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.QuestionAnswerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.QuestionAnswerPanel.Size = new System.Drawing.Size(1057, 385);
            this.QuestionAnswerPanel.TabIndex = 27;
            // 
            // QuestionLabel
            // 
            this.QuestionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuestionLabel.Font = new System.Drawing.Font("Arial", 15.7F);
            this.QuestionLabel.Location = new System.Drawing.Point(3, 0);
            this.QuestionLabel.Name = "QuestionLabel";
            this.QuestionLabel.Size = new System.Drawing.Size(1051, 60);
            this.QuestionLabel.TabIndex = 24;
            this.QuestionLabel.Text = "Does a forbearance or deferment on my underlying loans automatically transfer to " +
    "my new consolidation loan?  And this is another sentence that should wrap.";
            this.QuestionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // AnswerBox
            // 
            this.AnswerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnswerBox.BackColor = System.Drawing.Color.White;
            this.AnswerBox.Location = new System.Drawing.Point(3, 63);
            this.AnswerBox.Name = "AnswerBox";
            this.AnswerBox.ReadOnly = true;
            this.AnswerBox.Size = new System.Drawing.Size(1051, 339);
            this.AnswerBox.TabIndex = 22;
            this.AnswerBox.Text = "";
            // 
            // LastEditedLabel
            // 
            this.LastEditedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LastEditedLabel.AutoSize = true;
            this.LastEditedLabel.Location = new System.Drawing.Point(9, 616);
            this.LastEditedLabel.Name = "LastEditedLabel";
            this.LastEditedLabel.Size = new System.Drawing.Size(245, 16);
            this.LastEditedLabel.TabIndex = 26;
            this.LastEditedLabel.Text = "Last edited on 1/1/2012 by Username";
            // 
            // RelatedPanel
            // 
            this.RelatedPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RelatedPanel.Controls.Add(this.label1);
            this.RelatedPanel.Location = new System.Drawing.Point(6, 635);
            this.RelatedPanel.Name = "RelatedPanel";
            this.RelatedPanel.Size = new System.Drawing.Size(1057, 22);
            this.RelatedPanel.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Related: ";
            // 
            // SearchQuestionsBox
            // 
            this.SearchQuestionsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchQuestionsBox.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchQuestionsBox.Location = new System.Drawing.Point(6, 7);
            this.SearchQuestionsBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SearchQuestionsBox.Name = "SearchQuestionsBox";
            this.SearchQuestionsBox.Size = new System.Drawing.Size(1057, 217);
            this.SearchQuestionsBox.TabIndex = 23;
            this.SearchQuestionsBox.QuestionSelected += new MD.QuestionFilter.OnQuestionSelected(this.SearchQuestionsBox_QuestionSelected);
            // 
            // MyQuestionsTab
            // 
            this.MyQuestionsTab.BackColor = System.Drawing.SystemColors.Control;
            this.MyQuestionsTab.Controls.Add(this.PendingQuestionsControl);
            this.MyQuestionsTab.Location = new System.Drawing.Point(4, 25);
            this.MyQuestionsTab.Name = "MyQuestionsTab";
            this.MyQuestionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.MyQuestionsTab.Size = new System.Drawing.Size(1069, 668);
            this.MyQuestionsTab.TabIndex = 1;
            this.MyQuestionsTab.Text = "My Questions";
            // 
            // PendingQuestionsControl
            // 
            this.PendingQuestionsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PendingQuestionsControl.Location = new System.Drawing.Point(3, 3);
            this.PendingQuestionsControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PendingQuestionsControl.Name = "PendingQuestionsControl";
            this.PendingQuestionsControl.Size = new System.Drawing.Size(1063, 662);
            this.PendingQuestionsControl.TabIndex = 0;
            // 
            // FaqViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 697);
            this.Controls.Add(this.Tabs);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(1093, 736);
            this.Name = "FaqViewerForm";
            this.Text = "FAQ";
            this.Tabs.ResumeLayout(false);
            this.AllQuestionsTab.ResumeLayout(false);
            this.AllQuestionsTab.PerformLayout();
            this.QuestionAnswerPanel.ResumeLayout(false);
            this.RelatedPanel.ResumeLayout(false);
            this.RelatedPanel.PerformLayout();
            this.MyQuestionsTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox AnswerBox;
        private QuestionFilter SearchQuestionsBox;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage AllQuestionsTab;
        private System.Windows.Forms.TabPage MyQuestionsTab;
        private System.Windows.Forms.Label QuestionLabel;
        private System.Windows.Forms.FlowLayoutPanel RelatedPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LastEditedLabel;
        private PendingQuestions PendingQuestionsControl;
        private System.Windows.Forms.TableLayoutPanel QuestionAnswerPanel;
    }
}