namespace I1I2CHREV
{
    partial class IntroForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IntroForm));
            this.CancelButton = new System.Windows.Forms.Button();
            this.ProcessingButton = new System.Windows.Forms.Button();
            this.formatLabel1 = new Uheaa.Common.WinForms.FormatLabel();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(12, 192);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(92, 38);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ProcessingButton
            // 
            this.ProcessingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessingButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ProcessingButton.Location = new System.Drawing.Point(208, 192);
            this.ProcessingButton.Name = "ProcessingButton";
            this.ProcessingButton.Size = new System.Drawing.Size(188, 38);
            this.ProcessingButton.TabIndex = 2;
            this.ProcessingButton.Text = "Begin Processing";
            this.ProcessingButton.UseVisualStyleBackColor = true;
            // 
            // formatLabel1
            // 
            this.formatLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formatLabel1.Location = new System.Drawing.Point(12, 12);
            this.formatLabel1.Name = "formatLabel1";
            this.formatLabel1.Size = new System.Drawing.Size(384, 174);
            this.formatLabel1.SourceMarkup = resources.GetString("formatLabel1.SourceMarkup");
            this.formatLabel1.TabIndex = 3;
            // 
            // IntroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 245);
            this.Controls.Add(this.formatLabel1);
            this.Controls.Add(this.ProcessingButton);
            this.Controls.Add(this.CancelButton);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(424, 283);
            this.Name = "IntroForm";
            this.Text = "I1/I2 Clearing House Review";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button ProcessingButton;
        private Uheaa.Common.WinForms.FormatLabel formatLabel1;
    }
}