namespace MD
{
    partial class TrainingModulesForm
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
            this.LaunchButton = new System.Windows.Forms.Button();
            this.ModulesList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // LaunchButton
            // 
            this.LaunchButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LaunchButton.Font = new System.Drawing.Font("Arial", 14F);
            this.LaunchButton.Location = new System.Drawing.Point(92, 326);
            this.LaunchButton.Name = "LaunchButton";
            this.LaunchButton.Size = new System.Drawing.Size(145, 47);
            this.LaunchButton.TabIndex = 1;
            this.LaunchButton.Text = "Launch";
            this.LaunchButton.UseVisualStyleBackColor = true;
            this.LaunchButton.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // ModulesList
            // 
            this.ModulesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModulesList.FormattingEnabled = true;
            this.ModulesList.ItemHeight = 16;
            this.ModulesList.Location = new System.Drawing.Point(12, 12);
            this.ModulesList.Name = "ModulesList";
            this.ModulesList.Size = new System.Drawing.Size(305, 308);
            this.ModulesList.TabIndex = 0;
            this.ModulesList.SelectedIndexChanged += new System.EventHandler(this.ModulesList_SelectedIndexChanged);
            this.ModulesList.DoubleClick += new System.EventHandler(this.ModulesList_DoubleClick);
            // 
            // TrainingModulesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 376);
            this.Controls.Add(this.LaunchButton);
            this.Controls.Add(this.ModulesList);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(347, 415);
            this.Name = "TrainingModulesForm";
            this.Text = "Training Modules";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ModulesList;
        private System.Windows.Forms.Button LaunchButton;
    }
}