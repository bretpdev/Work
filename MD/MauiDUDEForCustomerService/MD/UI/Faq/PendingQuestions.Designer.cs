namespace MD
{
    partial class PendingQuestions
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.GridLabel = new System.Windows.Forms.Label();
            this.QuestionLabel = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.NotesBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.QuestionBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.MyQuestionsGrid = new System.Windows.Forms.DataGridView();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuestionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubmittedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ApproveButton = new System.Windows.Forms.Button();
            this.RejectButton = new System.Windows.Forms.Button();
            this.WithdrawButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MyQuestionsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.GridLabel);
            this.panel1.Controls.Add(this.QuestionLabel);
            this.panel1.Controls.Add(this.CancelButton);
            this.panel1.Controls.Add(this.SubmitButton);
            this.panel1.Controls.Add(this.NotesBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.QuestionBox);
            this.panel1.Controls.Add(this.MyQuestionsGrid);
            this.panel1.Controls.Add(this.ApproveButton);
            this.panel1.Controls.Add(this.RejectButton);
            this.panel1.Controls.Add(this.WithdrawButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1061, 704);
            this.panel1.TabIndex = 0;
            // 
            // GridLabel
            // 
            this.GridLabel.AutoSize = true;
            this.GridLabel.Font = new System.Drawing.Font("Arial", 14F);
            this.GridLabel.Location = new System.Drawing.Point(2, 2);
            this.GridLabel.Name = "GridLabel";
            this.GridLabel.Size = new System.Drawing.Size(125, 22);
            this.GridLabel.TabIndex = 25;
            this.GridLabel.Text = "My Questions";
            // 
            // QuestionLabel
            // 
            this.QuestionLabel.AutoSize = true;
            this.QuestionLabel.Font = new System.Drawing.Font("Arial", 14F);
            this.QuestionLabel.Location = new System.Drawing.Point(2, 180);
            this.QuestionLabel.Name = "QuestionLabel";
            this.QuestionLabel.Size = new System.Drawing.Size(86, 22);
            this.QuestionLabel.TabIndex = 24;
            this.QuestionLabel.Text = "Question";
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.Font = new System.Drawing.Font("Arial", 14F);
            this.CancelButton.Location = new System.Drawing.Point(6, 664);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(121, 39);
            this.CancelButton.TabIndex = 23;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Visible = false;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SubmitButton
            // 
            this.SubmitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SubmitButton.Font = new System.Drawing.Font("Arial", 14F);
            this.SubmitButton.Location = new System.Drawing.Point(870, 664);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(187, 39);
            this.SubmitButton.TabIndex = 22;
            this.SubmitButton.Text = "Ask New Question";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // NotesBox
            // 
            this.NotesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesBox.Location = new System.Drawing.Point(6, 262);
            this.NotesBox.Multiline = true;
            this.NotesBox.Name = "NotesBox";
            this.NotesBox.Size = new System.Drawing.Size(1051, 396);
            this.NotesBox.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 14F);
            this.label2.Location = new System.Drawing.Point(2, 237);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(217, 22);
            this.label2.TabIndex = 20;
            this.label2.Text = "Notes / Feedback / Input";
            // 
            // QuestionBox
            // 
            this.QuestionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuestionBox.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Italic);
            this.QuestionBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.QuestionBox.Location = new System.Drawing.Point(4, 205);
            this.QuestionBox.Name = "QuestionBox";
            this.QuestionBox.Size = new System.Drawing.Size(1053, 29);
            this.QuestionBox.TabIndex = 19;
            this.QuestionBox.Watermark = "";
            // 
            // MyQuestionsGrid
            // 
            this.MyQuestionsGrid.AllowUserToAddRows = false;
            this.MyQuestionsGrid.AllowUserToDeleteRows = false;
            this.MyQuestionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MyQuestionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MyQuestionsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StatusColumn,
            this.QuestionColumn,
            this.SubmittedColumn});
            this.MyQuestionsGrid.Location = new System.Drawing.Point(6, 27);
            this.MyQuestionsGrid.Name = "MyQuestionsGrid";
            this.MyQuestionsGrid.ReadOnly = true;
            this.MyQuestionsGrid.RowHeadersVisible = false;
            this.MyQuestionsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MyQuestionsGrid.Size = new System.Drawing.Size(1051, 150);
            this.MyQuestionsGrid.TabIndex = 18;
            this.MyQuestionsGrid.SelectionChanged += new System.EventHandler(this.MyQuestionsGrid_SelectionChanged);
            // 
            // StatusColumn
            // 
            this.StatusColumn.DataPropertyName = "Status";
            this.StatusColumn.HeaderText = "Status";
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.ReadOnly = true;
            this.StatusColumn.Width = 150;
            // 
            // QuestionColumn
            // 
            this.QuestionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.QuestionColumn.DataPropertyName = "Question";
            this.QuestionColumn.HeaderText = "Question";
            this.QuestionColumn.Name = "QuestionColumn";
            this.QuestionColumn.ReadOnly = true;
            // 
            // SubmittedColumn
            // 
            this.SubmittedColumn.DataPropertyName = "SubmittedOnString";
            this.SubmittedColumn.HeaderText = "Submitted";
            this.SubmittedColumn.Name = "SubmittedColumn";
            this.SubmittedColumn.ReadOnly = true;
            // 
            // ApproveButton
            // 
            this.ApproveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApproveButton.Font = new System.Drawing.Font("Arial", 14F);
            this.ApproveButton.Location = new System.Drawing.Point(771, 664);
            this.ApproveButton.Name = "ApproveButton";
            this.ApproveButton.Size = new System.Drawing.Size(286, 39);
            this.ApproveButton.TabIndex = 27;
            this.ApproveButton.Text = "Approve and Edit Question";
            this.ApproveButton.UseVisualStyleBackColor = true;
            this.ApproveButton.Visible = false;
            this.ApproveButton.Click += new System.EventHandler(this.ApproveButton_Click);
            // 
            // RejectButton
            // 
            this.RejectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RejectButton.Font = new System.Drawing.Font("Arial", 14F);
            this.RejectButton.Location = new System.Drawing.Point(6, 664);
            this.RejectButton.Name = "RejectButton";
            this.RejectButton.Size = new System.Drawing.Size(187, 39);
            this.RejectButton.TabIndex = 26;
            this.RejectButton.Text = "Reject Question";
            this.RejectButton.UseVisualStyleBackColor = true;
            this.RejectButton.Visible = false;
            this.RejectButton.Click += new System.EventHandler(this.RejectButton_Click);
            // 
            // WithdrawButton
            // 
            this.WithdrawButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WithdrawButton.Font = new System.Drawing.Font("Arial", 14F);
            this.WithdrawButton.Location = new System.Drawing.Point(6, 664);
            this.WithdrawButton.Name = "WithdrawButton";
            this.WithdrawButton.Size = new System.Drawing.Size(198, 39);
            this.WithdrawButton.TabIndex = 28;
            this.WithdrawButton.Text = "Withdraw Question";
            this.WithdrawButton.UseVisualStyleBackColor = true;
            this.WithdrawButton.Visible = false;
            this.WithdrawButton.Click += new System.EventHandler(this.WithdrawButton_Click);
            // 
            // PendingQuestions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "PendingQuestions";
            this.Size = new System.Drawing.Size(1061, 704);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MyQuestionsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label GridLabel;
        private System.Windows.Forms.Label QuestionLabel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.TextBox NotesBox;
        private System.Windows.Forms.Label label2;
        private Uheaa.Common.WinForms.WatermarkTextBox QuestionBox;
        private System.Windows.Forms.DataGridView MyQuestionsGrid;
        private System.Windows.Forms.Button ApproveButton;
        private System.Windows.Forms.Button RejectButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuestionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubmittedColumn;
        private System.Windows.Forms.Button WithdrawButton;

    }
}
