namespace Uheaa.Common.DataAccess
{
    partial class CreateShortcutForm
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
            this.DevButton = new System.Windows.Forms.Button();
            this.TestButton = new System.Windows.Forms.Button();
            this.QAButton = new System.Windows.Forms.Button();
            this.LiveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(277, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "This application must be started with an argument of dev, test, qa, or live.";
            // 
            // DevButton
            // 
            this.DevButton.Location = new System.Drawing.Point(16, 56);
            this.DevButton.Name = "DevButton";
            this.DevButton.Size = new System.Drawing.Size(274, 50);
            this.DevButton.TabIndex = 1;
            this.DevButton.Text = "Create Dev Shortcut";
            this.DevButton.UseVisualStyleBackColor = true;
            this.DevButton.Click += new System.EventHandler(this.DevButton_Click);
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(16, 112);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(274, 50);
            this.TestButton.TabIndex = 2;
            this.TestButton.Text = "Create Test Shortcut";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // QAButton
            // 
            this.QAButton.Location = new System.Drawing.Point(16, 168);
            this.QAButton.Name = "QAButton";
            this.QAButton.Size = new System.Drawing.Size(274, 50);
            this.QAButton.TabIndex = 3;
            this.QAButton.Text = "Create QA Shortcut";
            this.QAButton.UseVisualStyleBackColor = true;
            this.QAButton.Click += new System.EventHandler(this.QAButton_Click);
            // 
            // LiveButton
            // 
            this.LiveButton.Location = new System.Drawing.Point(17, 224);
            this.LiveButton.Name = "LiveButton";
            this.LiveButton.Size = new System.Drawing.Size(274, 50);
            this.LiveButton.TabIndex = 4;
            this.LiveButton.Text = "Create Live Shortcut";
            this.LiveButton.UseVisualStyleBackColor = true;
            this.LiveButton.Click += new System.EventHandler(this.LiveButton_Click);
            // 
            // CreateShortcutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 284);
            this.Controls.Add(this.LiveButton);
            this.Controls.Add(this.QAButton);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.DevButton);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateShortcutForm";
            this.Text = "{0} - Create Shortcut?";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DevButton;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.Button QAButton;
        private System.Windows.Forms.Button LiveButton;
    }
}