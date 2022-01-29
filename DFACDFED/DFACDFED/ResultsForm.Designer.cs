namespace DFACDFED
{
    partial class ResultsForm
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
            this.EcorrLettersLabel = new System.Windows.Forms.Label();
            this.EcorrLettersBox = new System.Windows.Forms.ListBox();
            this.PrintedLettersBox = new System.Windows.Forms.ListBox();
            this.PrintedLettersLabel = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ProcessingButton = new System.Windows.Forms.Button();
            this.TodayDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.ProcessingCompleteLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // EcorrLettersLabel
            // 
            this.EcorrLettersLabel.AutoSize = true;
            this.EcorrLettersLabel.Location = new System.Drawing.Point(15, 9);
            this.EcorrLettersLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.EcorrLettersLabel.Name = "EcorrLettersLabel";
            this.EcorrLettersLabel.Size = new System.Drawing.Size(215, 22);
            this.EcorrLettersLabel.TabIndex = 0;
            this.EcorrLettersLabel.Text = "Ecorr Letters Generated";
            // 
            // EcorrLettersBox
            // 
            this.EcorrLettersBox.FormattingEnabled = true;
            this.EcorrLettersBox.ItemHeight = 22;
            this.EcorrLettersBox.Location = new System.Drawing.Point(19, 34);
            this.EcorrLettersBox.Name = "EcorrLettersBox";
            this.EcorrLettersBox.Size = new System.Drawing.Size(348, 290);
            this.EcorrLettersBox.TabIndex = 1;
            // 
            // PrintedLettersBox
            // 
            this.PrintedLettersBox.FormattingEnabled = true;
            this.PrintedLettersBox.ItemHeight = 22;
            this.PrintedLettersBox.Location = new System.Drawing.Point(373, 34);
            this.PrintedLettersBox.Name = "PrintedLettersBox";
            this.PrintedLettersBox.Size = new System.Drawing.Size(348, 290);
            this.PrintedLettersBox.TabIndex = 2;
            // 
            // PrintedLettersLabel
            // 
            this.PrintedLettersLabel.AutoSize = true;
            this.PrintedLettersLabel.Location = new System.Drawing.Point(369, 9);
            this.PrintedLettersLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PrintedLettersLabel.Name = "PrintedLettersLabel";
            this.PrintedLettersLabel.Size = new System.Drawing.Size(229, 22);
            this.PrintedLettersLabel.TabIndex = 3;
            this.PrintedLettersLabel.Text = "Printed Letters Generated";
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(19, 330);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(162, 42);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ProcessingButton
            // 
            this.ProcessingButton.Location = new System.Drawing.Point(508, 330);
            this.ProcessingButton.Name = "ProcessingButton";
            this.ProcessingButton.Size = new System.Drawing.Size(213, 42);
            this.ProcessingButton.TabIndex = 5;
            this.ProcessingButton.Text = "Begin Processing";
            this.ProcessingButton.UseVisualStyleBackColor = true;
            this.ProcessingButton.Click += new System.EventHandler(this.ProcessingButton_Click);
            // 
            // TodayDate
            // 
            this.TodayDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.TodayDate.Location = new System.Drawing.Point(345, 335);
            this.TodayDate.Name = "TodayDate";
            this.TodayDate.Size = new System.Drawing.Size(157, 29);
            this.TodayDate.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(278, 340);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 22);
            this.label3.TabIndex = 7;
            this.label3.Text = "Today";
            // 
            // ProcessingCompleteLabel
            // 
            this.ProcessingCompleteLabel.ForeColor = System.Drawing.Color.Green;
            this.ProcessingCompleteLabel.Location = new System.Drawing.Point(508, 330);
            this.ProcessingCompleteLabel.Name = "ProcessingCompleteLabel";
            this.ProcessingCompleteLabel.Size = new System.Drawing.Size(213, 42);
            this.ProcessingCompleteLabel.TabIndex = 8;
            this.ProcessingCompleteLabel.Text = "Processing Complete";
            this.ProcessingCompleteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProcessingCompleteLabel.Visible = false;
            // 
            // ResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 381);
            this.Controls.Add(this.ProcessingCompleteLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TodayDate);
            this.Controls.Add(this.ProcessingButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.PrintedLettersBox);
            this.Controls.Add(this.EcorrLettersBox);
            this.Controls.Add(this.EcorrLettersLabel);
            this.Controls.Add(this.PrintedLettersLabel);
            this.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MaximizeBox = false;
            this.Name = "ResultsForm";
            this.Text = "DFACDFED Status";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EcorrLettersLabel;
        private System.Windows.Forms.ListBox EcorrLettersBox;
        private System.Windows.Forms.ListBox PrintedLettersBox;
        private System.Windows.Forms.Label PrintedLettersLabel;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button ProcessingButton;
        private System.Windows.Forms.DateTimePicker TodayDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ProcessingCompleteLabel;
    }
}