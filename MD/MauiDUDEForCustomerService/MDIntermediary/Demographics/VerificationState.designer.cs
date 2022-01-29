namespace MDIntermediary
{
    partial class VerificationState
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
            this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.NoChangeButton = new Uheaa.Common.WinForms.CheckButton();
            this.RefusedButton = new Uheaa.Common.WinForms.CheckButton();
            this.ValidButton = new Uheaa.Common.WinForms.CheckButton();
            this.InvalidateFirstButton = new Uheaa.Common.WinForms.CheckButton();
            this.InvalidButton = new Uheaa.Common.WinForms.CheckButton();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.NoChangeButton);
            this.MainPanel.Controls.Add(this.RefusedButton);
            this.MainPanel.Controls.Add(this.ValidButton);
            this.MainPanel.Controls.Add(this.InvalidateFirstButton);
            this.MainPanel.Controls.Add(this.InvalidButton);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(661, 29);
            this.MainPanel.TabIndex = 4;
            // 
            // NoChangeButton
            // 
            this.NoChangeButton.IsChecked = true;
            this.NoChangeButton.Location = new System.Drawing.Point(3, 3);
            this.NoChangeButton.Name = "NoChangeButton";
            this.NoChangeButton.Size = new System.Drawing.Size(109, 26);
            this.NoChangeButton.TabIndex = 0;
            this.NoChangeButton.Text = "No Change";
            this.NoChangeButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.NoChangeButton.UseVisualStyleBackColor = true;
            this.NoChangeButton.Click += new System.EventHandler(this.NoChangeButton_Click);
            // 
            // RefusedButton
            // 
            this.RefusedButton.IsChecked = false;
            this.RefusedButton.Location = new System.Drawing.Point(118, 3);
            this.RefusedButton.Name = "RefusedButton";
            this.RefusedButton.Size = new System.Drawing.Size(91, 26);
            this.RefusedButton.TabIndex = 1;
            this.RefusedButton.Text = "Refused";
            this.RefusedButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RefusedButton.UseVisualStyleBackColor = true;
            this.RefusedButton.Click += new System.EventHandler(this.RefusedButton_Click);
            // 
            // ValidButton
            // 
            this.ValidButton.IsChecked = false;
            this.ValidButton.Location = new System.Drawing.Point(215, 3);
            this.ValidButton.Name = "ValidButton";
            this.ValidButton.Size = new System.Drawing.Size(67, 26);
            this.ValidButton.TabIndex = 2;
            this.ValidButton.Text = "Valid";
            this.ValidButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ValidButton.UseVisualStyleBackColor = true;
            this.ValidButton.Click += new System.EventHandler(this.ValidButton_Click);
            // 
            // InvalidateFirstButton
            // 
            this.InvalidateFirstButton.IsChecked = false;
            this.InvalidateFirstButton.Location = new System.Drawing.Point(288, 3);
            this.InvalidateFirstButton.Name = "InvalidateFirstButton";
            this.InvalidateFirstButton.Size = new System.Drawing.Size(132, 26);
            this.InvalidateFirstButton.TabIndex = 3;
            this.InvalidateFirstButton.Text = "Invalidate First";
            this.InvalidateFirstButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.InvalidateFirstButton.UseVisualStyleBackColor = true;
            // 
            // InvalidButton
            // 
            this.InvalidButton.IsChecked = false;
            this.InvalidButton.Location = new System.Drawing.Point(426, 3);
            this.InvalidButton.Name = "InvalidButton";
            this.InvalidButton.Size = new System.Drawing.Size(75, 26);
            this.InvalidButton.TabIndex = 4;
            this.InvalidButton.Text = "Invalid";
            this.InvalidButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.InvalidButton.UseVisualStyleBackColor = true;
            this.InvalidButton.Click += new System.EventHandler(this.InvalidButton_Click);
            // 
            // VerificationState
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.MainPanel);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VerificationState";
            this.Size = new System.Drawing.Size(661, 29);
            this.MainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Uheaa.Common.WinForms.CheckButton NoChangeButton;
        private Uheaa.Common.WinForms.CheckButton ValidButton;
        private Uheaa.Common.WinForms.CheckButton InvalidButton;
        private Uheaa.Common.WinForms.CheckButton RefusedButton;
        private System.Windows.Forms.FlowLayoutPanel MainPanel;
        private Uheaa.Common.WinForms.CheckButton InvalidateFirstButton;
    }
}
