namespace Monitor
{
    partial class LaunchForm
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
            this.components = new System.ComponentModel.Container();
            this.AdjustSettingsButton = new System.Windows.Forms.Button();
            this.BeginProcessingButton = new System.Windows.Forms.Button();
            this.AutoLaunchTimerLabel = new System.Windows.Forms.Label();
            this.AutoLaunchTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // AdjustSettingsButton
            // 
            this.AdjustSettingsButton.Location = new System.Drawing.Point(13, 13);
            this.AdjustSettingsButton.Margin = new System.Windows.Forms.Padding(4);
            this.AdjustSettingsButton.Name = "AdjustSettingsButton";
            this.AdjustSettingsButton.Size = new System.Drawing.Size(328, 56);
            this.AdjustSettingsButton.TabIndex = 0;
            this.AdjustSettingsButton.Text = "Adjust Settings";
            this.AdjustSettingsButton.UseVisualStyleBackColor = true;
            this.AdjustSettingsButton.Click += new System.EventHandler(this.AdjustSettingsButton_Click);
            // 
            // BeginProcessingButton
            // 
            this.BeginProcessingButton.Location = new System.Drawing.Point(13, 77);
            this.BeginProcessingButton.Margin = new System.Windows.Forms.Padding(4);
            this.BeginProcessingButton.Name = "BeginProcessingButton";
            this.BeginProcessingButton.Size = new System.Drawing.Size(328, 56);
            this.BeginProcessingButton.TabIndex = 1;
            this.BeginProcessingButton.Text = "Begin Processing";
            this.BeginProcessingButton.UseVisualStyleBackColor = true;
            this.BeginProcessingButton.Click += new System.EventHandler(this.BeginProcessingButton_Click);
            // 
            // AutoLaunchTimerLabel
            // 
            this.AutoLaunchTimerLabel.Location = new System.Drawing.Point(13, 137);
            this.AutoLaunchTimerLabel.Name = "AutoLaunchTimerLabel";
            this.AutoLaunchTimerLabel.Size = new System.Drawing.Size(328, 55);
            this.AutoLaunchTimerLabel.TabIndex = 3;
            this.AutoLaunchTimerLabel.Text = "If no selection is made, procesing will begin in {0} seconds.";
            // 
            // AutoLaunchTimer
            // 
            this.AutoLaunchTimer.Enabled = true;
            this.AutoLaunchTimer.Tick += new System.EventHandler(this.AutoLaunchTimer_Tick);
            // 
            // LaunchForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(356, 181);
            this.Controls.Add(this.AutoLaunchTimerLabel);
            this.Controls.Add(this.BeginProcessingButton);
            this.Controls.Add(this.AdjustSettingsButton);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LaunchForm";
            this.Text = "Monitor - Select Region";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AdjustSettingsButton;
        private System.Windows.Forms.Button BeginProcessingButton;
        private System.Windows.Forms.Label AutoLaunchTimerLabel;
        private System.Windows.Forms.Timer AutoLaunchTimer;
    }
}