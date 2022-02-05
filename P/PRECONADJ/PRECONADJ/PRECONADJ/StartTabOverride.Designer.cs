namespace PRECONADJ
{
    partial class StartTabOverride
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
            this.startButton = new System.Windows.Forms.Button();
            this.startAtCheckBox = new System.Windows.Forms.CheckBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.startAtUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.startAtUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.startButton.Location = new System.Drawing.Point(260, 141);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 30);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            // 
            // startAtCheckBox
            // 
            this.startAtCheckBox.AutoSize = true;
            this.startAtCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.startAtCheckBox.Location = new System.Drawing.Point(12, 12);
            this.startAtCheckBox.Name = "startAtCheckBox";
            this.startAtCheckBox.Size = new System.Drawing.Size(282, 22);
            this.startAtCheckBox.TabIndex = 1;
            this.startAtCheckBox.Text = "Start at a specified tab(loan sequence).";
            this.startAtCheckBox.UseVisualStyleBackColor = true;
            this.startAtCheckBox.CheckedChanged += new System.EventHandler(this.startAtCheckBox_CheckedChanged);
            // 
            // infoLabel
            // 
            this.infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.infoLabel.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.infoLabel.Location = new System.Drawing.Point(12, 71);
            this.infoLabel.MinimumSize = new System.Drawing.Size(318, 67);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(318, 67);
            this.infoLabel.TabIndex = 3;
            this.infoLabel.Text = "*If you would like to process the whole worksheet press start without checking th" +
    "e box";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // startAtUpDown
            // 
            this.startAtUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startAtUpDown.Location = new System.Drawing.Point(15, 40);
            this.startAtUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.startAtUpDown.Name = "startAtUpDown";
            this.startAtUpDown.Size = new System.Drawing.Size(73, 26);
            this.startAtUpDown.TabIndex = 4;
            this.startAtUpDown.ValueChanged += new System.EventHandler(this.startAtUpDown_ValueChanged);
            // 
            // StartTabOverride
            // 
            this.AcceptButton = this.startButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 176);
            this.Controls.Add(this.startAtUpDown);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.startAtCheckBox);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StartTabOverride";
            this.Text = "PRECONADJ";
            ((System.ComponentModel.ISupportInitialize)(this.startAtUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.CheckBox startAtCheckBox;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.NumericUpDown startAtUpDown;
    }
}