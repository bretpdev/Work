namespace SASM
{
    partial class SasWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.WindowNameText = new System.Windows.Forms.TextBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Window Name";
            // 
            // WindowNameText
            // 
            this.WindowNameText.Location = new System.Drawing.Point(6, 29);
            this.WindowNameText.Name = "WindowNameText";
            this.WindowNameText.Size = new System.Drawing.Size(165, 20);
            this.WindowNameText.TabIndex = 1;
            this.WindowNameText.TextChanged += new System.EventHandler(this.WindowNameText_TextChanged);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Image = global::SASM.Properties.Resources.Actions_system_shutdown_icon;
            this.DeleteButton.Location = new System.Drawing.Point(68, 82);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(38, 38);
            this.DeleteButton.TabIndex = 8;
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Image = global::SASM.Properties.Resources.Actions_dialog_close_icon;
            this.CancelButton.Location = new System.Drawing.Point(151, 6);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(20, 20);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Visible = false;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.Image = global::SASM.Properties.Resources.Actions_dialog_ok_apply_icon;
            this.OkButton.Location = new System.Drawing.Point(127, 6);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(20, 20);
            this.OkButton.TabIndex = 2;
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Visible = false;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // TitleLabel
            // 
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(3, 52);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(168, 27);
            this.TitleLabel.TabIndex = 9;
            this.TitleLabel.Text = "LEGEND LIVE";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SasWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.WindowNameText);
            this.Controls.Add(this.label1);
            this.Name = "SasWindow";
            this.Size = new System.Drawing.Size(177, 129);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SasWindow_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox WindowNameText;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Label TitleLabel;
    }
}
