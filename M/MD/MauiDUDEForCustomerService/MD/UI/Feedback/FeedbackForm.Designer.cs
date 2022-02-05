namespace MD
{
    partial class FeedbackForm
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
            this.ScreenshotBox = new System.Windows.Forms.PictureBox();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.NotesBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReflectionBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenshotBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReflectionBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ScreenshotBox
            // 
            this.ScreenshotBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ScreenshotBox.Location = new System.Drawing.Point(12, 455);
            this.ScreenshotBox.Name = "ScreenshotBox";
            this.ScreenshotBox.Size = new System.Drawing.Size(145, 104);
            this.ScreenshotBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ScreenshotBox.TabIndex = 3;
            this.ScreenshotBox.TabStop = false;
            this.ScreenshotBox.Click += new System.EventHandler(this.ScreenshotBox_Click);
            // 
            // SubmitButton
            // 
            this.SubmitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SubmitButton.Font = new System.Drawing.Font("Arial", 14F);
            this.SubmitButton.Location = new System.Drawing.Point(525, 482);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(241, 48);
            this.SubmitButton.TabIndex = 2;
            this.SubmitButton.Text = "Submit Request";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // NotesBox
            // 
            this.NotesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesBox.Location = new System.Drawing.Point(12, 38);
            this.NotesBox.MaxLength = 4000;
            this.NotesBox.Multiline = true;
            this.NotesBox.Name = "NotesBox";
            this.NotesBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.NotesBox.Size = new System.Drawing.Size(754, 411);
            this.NotesBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F);
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Details, Notes, or Ideas here:";
            // 
            // ReflectionBox
            // 
            this.ReflectionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ReflectionBox.Location = new System.Drawing.Point(163, 455);
            this.ReflectionBox.Name = "ReflectionBox";
            this.ReflectionBox.Size = new System.Drawing.Size(145, 104);
            this.ReflectionBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ReflectionBox.TabIndex = 4;
            this.ReflectionBox.TabStop = false;
            this.ReflectionBox.Click += new System.EventHandler(this.ReflectionBox_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8F);
            this.label2.Location = new System.Drawing.Point(314, 543);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Click Screenshots to add Annotations";
            // 
            // FeedbackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 571);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ReflectionBox);
            this.Controls.Add(this.ScreenshotBox);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.NotesBox);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(794, 610);
            this.Name = "FeedbackForm";
            this.Text = "Feature Request";
            ((System.ComponentModel.ISupportInitialize)(this.ScreenshotBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReflectionBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NotesBox;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.PictureBox ScreenshotBox;
        private System.Windows.Forms.PictureBox ReflectionBox;
        private System.Windows.Forms.Label label2;
    }
}