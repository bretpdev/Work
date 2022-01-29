namespace MDIntermediary.Calls
{
    partial class CallCategorizationControl
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
            this.CommentsBox = new System.Windows.Forms.TextBox();
            this.LetterIdBox = new System.Windows.Forms.TextBox();
            this.Label38 = new System.Windows.Forms.Label();
            this.Label39 = new System.Windows.Forms.Label();
            this.ReasonBox = new System.Windows.Forms.ComboBox();
            this.Label40 = new System.Windows.Forms.Label();
            this.Label41 = new System.Windows.Forms.Label();
            this.CategoryBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // CommentsBox
            // 
            this.CommentsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CommentsBox.Enabled = false;
            this.CommentsBox.Location = new System.Drawing.Point(86, 68);
            this.CommentsBox.MaxLength = 30;
            this.CommentsBox.Multiline = true;
            this.CommentsBox.Name = "CommentsBox";
            this.CommentsBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CommentsBox.Size = new System.Drawing.Size(237, 59);
            this.CommentsBox.TabIndex = 23;
            // 
            // LetterIdBox
            // 
            this.LetterIdBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LetterIdBox.Enabled = false;
            this.LetterIdBox.Location = new System.Drawing.Point(86, 47);
            this.LetterIdBox.MaxLength = 10;
            this.LetterIdBox.Name = "LetterIdBox";
            this.LetterIdBox.Size = new System.Drawing.Size(237, 20);
            this.LetterIdBox.TabIndex = 22;
            // 
            // Label38
            // 
            this.Label38.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label38.Location = new System.Drawing.Point(2, 71);
            this.Label38.Name = "Label38";
            this.Label38.Size = new System.Drawing.Size(64, 16);
            this.Label38.TabIndex = 21;
            this.Label38.Text = "Comments:";
            // 
            // Label39
            // 
            this.Label39.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label39.Location = new System.Drawing.Point(2, 50);
            this.Label39.Name = "Label39";
            this.Label39.Size = new System.Drawing.Size(64, 16);
            this.Label39.TabIndex = 20;
            this.Label39.Text = "Letter ID:";
            // 
            // ReasonBox
            // 
            this.ReasonBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ReasonBox.DisplayMember = "ReasonText";
            this.ReasonBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReasonBox.Enabled = false;
            this.ReasonBox.Location = new System.Drawing.Point(86, 25);
            this.ReasonBox.Name = "ReasonBox";
            this.ReasonBox.Size = new System.Drawing.Size(237, 21);
            this.ReasonBox.TabIndex = 19;
            // 
            // Label40
            // 
            this.Label40.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label40.Location = new System.Drawing.Point(2, 28);
            this.Label40.Name = "Label40";
            this.Label40.Size = new System.Drawing.Size(64, 16);
            this.Label40.TabIndex = 18;
            this.Label40.Text = "Reason:";
            // 
            // Label41
            // 
            this.Label41.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Label41.Location = new System.Drawing.Point(2, 7);
            this.Label41.Name = "Label41";
            this.Label41.Size = new System.Drawing.Size(64, 16);
            this.Label41.TabIndex = 17;
            this.Label41.Text = "Category:";
            // 
            // CategoryBox
            // 
            this.CategoryBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CategoryBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CategoryBox.Location = new System.Drawing.Point(86, 3);
            this.CategoryBox.Name = "CategoryBox";
            this.CategoryBox.Size = new System.Drawing.Size(237, 21);
            this.CategoryBox.TabIndex = 16;
            this.CategoryBox.SelectedIndexChanged += new System.EventHandler(this.CategoryBox_SelectedIndexChanged);
            // 
            // CallCategorizationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CommentsBox);
            this.Controls.Add(this.LetterIdBox);
            this.Controls.Add(this.Label38);
            this.Controls.Add(this.Label39);
            this.Controls.Add(this.ReasonBox);
            this.Controls.Add(this.Label40);
            this.Controls.Add(this.Label41);
            this.Controls.Add(this.CategoryBox);
            this.Name = "CallCategorizationControl";
            this.Size = new System.Drawing.Size(325, 130);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox CommentsBox;
        internal System.Windows.Forms.TextBox LetterIdBox;
        internal System.Windows.Forms.Label Label38;
        internal System.Windows.Forms.Label Label39;
        internal System.Windows.Forms.ComboBox ReasonBox;
        internal System.Windows.Forms.Label Label40;
        internal System.Windows.Forms.Label Label41;
        internal System.Windows.Forms.ComboBox CategoryBox;


    }
}
