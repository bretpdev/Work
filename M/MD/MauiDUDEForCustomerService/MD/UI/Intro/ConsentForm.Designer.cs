namespace MD
{
    partial class ConsentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsentForm));
            this.WarningLabel = new System.Windows.Forms.Label();
            this.PF10Button = new System.Windows.Forms.Button();
            this.PF3Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // WarningLabel
            // 
            this.WarningLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.WarningLabel.Location = new System.Drawing.Point(0, 0);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Padding = new System.Windows.Forms.Padding(20, 10, 20, 0);
            this.WarningLabel.Size = new System.Drawing.Size(563, 252);
            this.WarningLabel.TabIndex = 2;
            this.WarningLabel.Text = resources.GetString("WarningLabel.Text");
            // 
            // PF10Button
            // 
            this.PF10Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PF10Button.Location = new System.Drawing.Point(490, 265);
            this.PF10Button.Name = "PF10Button";
            this.PF10Button.Size = new System.Drawing.Size(61, 32);
            this.PF10Button.TabIndex = 0;
            this.PF10Button.Text = "PF10";
            this.PF10Button.UseVisualStyleBackColor = true;
            this.PF10Button.Click += new System.EventHandler(this.PF10Button_Click);
            this.PF10Button.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_KeyUp);
            // 
            // PF3Button
            // 
            this.PF3Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PF3Button.Location = new System.Drawing.Point(12, 265);
            this.PF3Button.Name = "PF3Button";
            this.PF3Button.Size = new System.Drawing.Size(61, 32);
            this.PF3Button.TabIndex = 1;
            this.PF3Button.Text = "PF3";
            this.PF3Button.UseVisualStyleBackColor = true;
            this.PF3Button.Click += new System.EventHandler(this.PF3Button_Click);
            this.PF3Button.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_KeyUp);
            // 
            // ConsentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 309);
            this.Controls.Add(this.PF10Button);
            this.Controls.Add(this.PF3Button);
            this.Controls.Add(this.WarningLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(579, 348);
            this.Name = "ConsentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Consent";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GreetingsForm_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button PF3Button;
        private System.Windows.Forms.Button PF10Button;
        private System.Windows.Forms.Label WarningLabel;
    }
}