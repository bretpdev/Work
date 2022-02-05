namespace Monitor
{
    partial class ThresholdsEditorForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.MaxIncreaseBox = new System.Windows.Forms.NumericUpDown();
            this.MaxForceBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.MaxPreNoteBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.MaxIncreaseDbBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MaxForceDbBox = new System.Windows.Forms.TextBox();
            this.MaxPreNoteDbBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.MaxIncreaseBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxForceBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxPreNoteBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Max Increase";
            // 
            // MaxIncreaseBox
            // 
            this.MaxIncreaseBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxIncreaseBox.DecimalPlaces = 2;
            this.MaxIncreaseBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.MaxIncreaseBox.Location = new System.Drawing.Point(16, 59);
            this.MaxIncreaseBox.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.MaxIncreaseBox.Name = "MaxIncreaseBox";
            this.MaxIncreaseBox.Size = new System.Drawing.Size(198, 26);
            this.MaxIncreaseBox.TabIndex = 4;
            // 
            // MaxForceBox
            // 
            this.MaxForceBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxForceBox.Location = new System.Drawing.Point(16, 109);
            this.MaxForceBox.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.MaxForceBox.Name = "MaxForceBox";
            this.MaxForceBox.Size = new System.Drawing.Size(198, 26);
            this.MaxForceBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Max Force";
            // 
            // MaxPreNoteBox
            // 
            this.MaxPreNoteBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxPreNoteBox.Location = new System.Drawing.Point(16, 159);
            this.MaxPreNoteBox.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.MaxPreNoteBox.Name = "MaxPreNoteBox";
            this.MaxPreNoteBox.Size = new System.Drawing.Size(198, 26);
            this.MaxPreNoteBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Max Pre-Note";
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderLabel.Location = new System.Drawing.Point(12, 9);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(162, 24);
            this.HeaderLabel.TabIndex = 9;
            this.HeaderLabel.Text = "Monitor Settings";
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SaveButton.Location = new System.Drawing.Point(129, 201);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(191, 33);
            this.SaveButton.TabIndex = 10;
            this.SaveButton.Text = "Save and Continue";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(12, 201);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(111, 33);
            this.CancelButton.TabIndex = 11;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // MaxIncreaseDbBox
            // 
            this.MaxIncreaseDbBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxIncreaseDbBox.Location = new System.Drawing.Point(220, 59);
            this.MaxIncreaseDbBox.Name = "MaxIncreaseDbBox";
            this.MaxIncreaseDbBox.ReadOnly = true;
            this.MaxIncreaseDbBox.Size = new System.Drawing.Size(100, 26);
            this.MaxIncreaseDbBox.TabIndex = 12;
            this.MaxIncreaseDbBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(210, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "Current Values";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MaxForceDbBox
            // 
            this.MaxForceDbBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxForceDbBox.Location = new System.Drawing.Point(220, 108);
            this.MaxForceDbBox.Name = "MaxForceDbBox";
            this.MaxForceDbBox.ReadOnly = true;
            this.MaxForceDbBox.Size = new System.Drawing.Size(100, 26);
            this.MaxForceDbBox.TabIndex = 14;
            this.MaxForceDbBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MaxPreNoteDbBox
            // 
            this.MaxPreNoteDbBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxPreNoteDbBox.Location = new System.Drawing.Point(220, 158);
            this.MaxPreNoteDbBox.Name = "MaxPreNoteDbBox";
            this.MaxPreNoteDbBox.ReadOnly = true;
            this.MaxPreNoteDbBox.Size = new System.Drawing.Size(100, 26);
            this.MaxPreNoteDbBox.TabIndex = 15;
            this.MaxPreNoteDbBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ThresholdsEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 246);
            this.Controls.Add(this.MaxPreNoteDbBox);
            this.Controls.Add(this.MaxForceDbBox);
            this.Controls.Add(this.MaxIncreaseDbBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.HeaderLabel);
            this.Controls.Add(this.MaxPreNoteBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MaxForceBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MaxIncreaseBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(348, 284);
            this.Name = "ThresholdsEditorForm";
            this.Text = "Monitor - Adjust Thresholds";
            ((System.ComponentModel.ISupportInitialize)(this.MaxIncreaseBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxForceBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxPreNoteBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown MaxIncreaseBox;
        private System.Windows.Forms.NumericUpDown MaxForceBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown MaxPreNoteBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.TextBox MaxIncreaseDbBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox MaxForceDbBox;
        private System.Windows.Forms.TextBox MaxPreNoteDbBox;
    }
}