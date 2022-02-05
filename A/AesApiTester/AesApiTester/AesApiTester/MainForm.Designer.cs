namespace AesApiTester
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
            this.label1 = new System.Windows.Forms.Label();
            this.UrlBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PayloadBox = new System.Windows.Forms.TextBox();
            this.SendRequestButton = new System.Windows.Forms.Button();
            this.ResponseBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL";
            // 
            // UrlBox
            // 
            this.UrlBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UrlBox.Location = new System.Drawing.Point(12, 30);
            this.UrlBox.Name = "UrlBox";
            this.UrlBox.Size = new System.Drawing.Size(1044, 26);
            this.UrlBox.TabIndex = 1;
            this.UrlBox.TextChanged += new System.EventHandler(this.UrlBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Payload";
            // 
            // PayloadBox
            // 
            this.PayloadBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PayloadBox.Location = new System.Drawing.Point(12, 80);
            this.PayloadBox.Multiline = true;
            this.PayloadBox.Name = "PayloadBox";
            this.PayloadBox.Size = new System.Drawing.Size(1044, 371);
            this.PayloadBox.TabIndex = 3;
            this.PayloadBox.TextChanged += new System.EventHandler(this.PayloadBox_TextChanged);
            // 
            // SendRequestButton
            // 
            this.SendRequestButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SendRequestButton.Location = new System.Drawing.Point(97, 457);
            this.SendRequestButton.Name = "SendRequestButton";
            this.SendRequestButton.Size = new System.Drawing.Size(896, 38);
            this.SendRequestButton.TabIndex = 4;
            this.SendRequestButton.Text = "Send Request";
            this.SendRequestButton.UseVisualStyleBackColor = true;
            this.SendRequestButton.Click += new System.EventHandler(this.SendRequestButton_Click);
            // 
            // ResponseBox
            // 
            this.ResponseBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResponseBox.Location = new System.Drawing.Point(15, 501);
            this.ResponseBox.Multiline = true;
            this.ResponseBox.Name = "ResponseBox";
            this.ResponseBox.Size = new System.Drawing.Size(1044, 359);
            this.ResponseBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 480);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Response";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 872);
            this.Controls.Add(this.ResponseBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SendRequestButton);
            this.Controls.Add(this.PayloadBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UrlBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "AES API Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UrlBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PayloadBox;
        private System.Windows.Forms.Button SendRequestButton;
        private System.Windows.Forms.TextBox ResponseBox;
        private System.Windows.Forms.Label label3;
    }
}

