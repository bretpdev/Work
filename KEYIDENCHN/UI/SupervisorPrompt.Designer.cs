namespace KEYIDENCHN
{
    partial class SupervisorPrompt
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
            this.NormalUserButton = new System.Windows.Forms.Button();
            this.AdminButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NormalUserButton
            // 
            this.NormalUserButton.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NormalUserButton.Location = new System.Drawing.Point(12, 12);
            this.NormalUserButton.Name = "NormalUserButton";
            this.NormalUserButton.Size = new System.Drawing.Size(171, 78);
            this.NormalUserButton.TabIndex = 0;
            this.NormalUserButton.Text = "Make Changes Request";
            this.NormalUserButton.UseVisualStyleBackColor = true;
            this.NormalUserButton.Click += new System.EventHandler(this.NormalUserButton_Click);
            // 
            // AdminButton
            // 
            this.AdminButton.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdminButton.Location = new System.Drawing.Point(189, 12);
            this.AdminButton.Name = "AdminButton";
            this.AdminButton.Size = new System.Drawing.Size(171, 78);
            this.AdminButton.TabIndex = 1;
            this.AdminButton.Text = "Supervisor Authorization";
            this.AdminButton.UseVisualStyleBackColor = true;
            this.AdminButton.Click += new System.EventHandler(this.AdminButton_Click);
            // 
            // SupervisorPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 104);
            this.Controls.Add(this.AdminButton);
            this.Controls.Add(this.NormalUserButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SupervisorPrompt";
            this.Text = "Mode Selection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NormalUserButton;
        private System.Windows.Forms.Button AdminButton;
    }
}