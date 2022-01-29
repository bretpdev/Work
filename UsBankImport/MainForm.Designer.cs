namespace UsBankImport
{
    partial class MainForm
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
            this.PendingZipLabel = new System.Windows.Forms.Label();
            this.PendingZipBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CompletedZipBox = new System.Windows.Forms.ListBox();
            this.LogBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.FailedZipBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BeginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PendingZipLabel
            // 
            this.PendingZipLabel.AutoSize = true;
            this.PendingZipLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PendingZipLabel.Location = new System.Drawing.Point(12, 9);
            this.PendingZipLabel.Name = "PendingZipLabel";
            this.PendingZipLabel.Size = new System.Drawing.Size(100, 18);
            this.PendingZipLabel.TabIndex = 0;
            this.PendingZipLabel.Text = "Pending Zips";
            this.PendingZipLabel.Click += new System.EventHandler(this.PendingZipLabel_Click);
            // 
            // PendingZipBox
            // 
            this.PendingZipBox.FormattingEnabled = true;
            this.PendingZipBox.IntegralHeight = false;
            this.PendingZipBox.ItemHeight = 16;
            this.PendingZipBox.Location = new System.Drawing.Point(12, 30);
            this.PendingZipBox.Name = "PendingZipBox";
            this.PendingZipBox.Size = new System.Drawing.Size(256, 148);
            this.PendingZipBox.TabIndex = 1;
            this.PendingZipBox.DoubleClick += new System.EventHandler(this.ListBox_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Completed Zips";
            // 
            // CompletedZipBox
            // 
            this.CompletedZipBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CompletedZipBox.FormattingEnabled = true;
            this.CompletedZipBox.IntegralHeight = false;
            this.CompletedZipBox.ItemHeight = 16;
            this.CompletedZipBox.Location = new System.Drawing.Point(12, 221);
            this.CompletedZipBox.Name = "CompletedZipBox";
            this.CompletedZipBox.Size = new System.Drawing.Size(256, 152);
            this.CompletedZipBox.TabIndex = 3;
            this.CompletedZipBox.DoubleClick += new System.EventHandler(this.ListBox_DoubleClick);
            // 
            // LogBox
            // 
            this.LogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogBox.FormattingEnabled = true;
            this.LogBox.IntegralHeight = false;
            this.LogBox.ItemHeight = 16;
            this.LogBox.Location = new System.Drawing.Point(286, 30);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(524, 468);
            this.LogBox.TabIndex = 5;
            this.LogBox.DoubleClick += new System.EventHandler(this.ListBox_DoubleClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(283, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Log";
            // 
            // FailedZipBox
            // 
            this.FailedZipBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FailedZipBox.FormattingEnabled = true;
            this.FailedZipBox.IntegralHeight = false;
            this.FailedZipBox.ItemHeight = 16;
            this.FailedZipBox.Location = new System.Drawing.Point(12, 414);
            this.FailedZipBox.Name = "FailedZipBox";
            this.FailedZipBox.Size = new System.Drawing.Size(256, 84);
            this.FailedZipBox.TabIndex = 7;
            this.FailedZipBox.DoubleClick += new System.EventHandler(this.ListBox_DoubleClick);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 393);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Failed Zips";
            // 
            // BeginButton
            // 
            this.BeginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BeginButton.Location = new System.Drawing.Point(658, 504);
            this.BeginButton.Name = "BeginButton";
            this.BeginButton.Size = new System.Drawing.Size(152, 40);
            this.BeginButton.TabIndex = 8;
            this.BeginButton.Text = "Begin Processing";
            this.BeginButton.UseVisualStyleBackColor = true;
            this.BeginButton.Click += new System.EventHandler(this.BeginButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 550);
            this.Controls.Add(this.BeginButton);
            this.Controls.Add(this.FailedZipBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CompletedZipBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PendingZipBox);
            this.Controls.Add(this.PendingZipLabel);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Imaging - US Bank Import";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PendingZipLabel;
        private System.Windows.Forms.ListBox PendingZipBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox CompletedZipBox;
        private System.Windows.Forms.ListBox LogBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox FailedZipBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BeginButton;
    }
}

