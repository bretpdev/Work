namespace CommentAuditTracker
{
    partial class AgentControl
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
            this.ManageUtIdsButton = new System.Windows.Forms.Button();
            this.FullNameBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.AuditPercentageBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ActiveInactiveButton = new CommentAuditTracker.ActiveInactiveCycleButton();
            ((System.ComponentModel.ISupportInitialize)(this.AuditPercentageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ManageUtIdsButton
            // 
            this.ManageUtIdsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ManageUtIdsButton.Location = new System.Drawing.Point(420, 3);
            this.ManageUtIdsButton.Name = "ManageUtIdsButton";
            this.ManageUtIdsButton.Size = new System.Drawing.Size(156, 26);
            this.ManageUtIdsButton.TabIndex = 3;
            this.ManageUtIdsButton.Text = "Manage UT IDs";
            this.ManageUtIdsButton.UseVisualStyleBackColor = true;
            this.ManageUtIdsButton.Click += new System.EventHandler(this.ManageUtIdsButton_Click);
            // 
            // FullNameBox
            // 
            this.FullNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FullNameBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic);
            this.FullNameBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FullNameBox.Location = new System.Drawing.Point(3, 3);
            this.FullNameBox.Name = "FullNameBox";
            this.FullNameBox.Size = new System.Drawing.Size(216, 26);
            this.FullNameBox.TabIndex = 0;
            this.FullNameBox.Text = "Full Name";
            this.FullNameBox.Watermark = "Full Name";
            this.FullNameBox.TextChanged += new System.EventHandler(this.FullNameBox_TextChanged);
            // 
            // AuditPercentageBox
            // 
            this.AuditPercentageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AuditPercentageBox.DecimalPlaces = 2;
            this.AuditPercentageBox.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.AuditPercentageBox.Location = new System.Drawing.Point(225, 3);
            this.AuditPercentageBox.Name = "AuditPercentageBox";
            this.AuditPercentageBox.Size = new System.Drawing.Size(61, 26);
            this.AuditPercentageBox.TabIndex = 1;
            this.AuditPercentageBox.Value = new decimal(new int[] {
            75,
            0,
            0,
            65536});
            this.AuditPercentageBox.ValueChanged += new System.EventHandler(this.AuditPercentageBox_ValueChanged);
            this.AuditPercentageBox.Leave += new System.EventHandler(this.AuditPercentageBox_Leave);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(286, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "%";
            // 
            // ActiveInactiveButton
            // 
            this.ActiveInactiveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveInactiveButton.Font = new System.Drawing.Font("Arial", 12F);
            this.ActiveInactiveButton.Location = new System.Drawing.Point(317, 3);
            this.ActiveInactiveButton.Name = "ActiveInactiveButton";
            this.ActiveInactiveButton.SelectedIndex = 1;
            this.ActiveInactiveButton.SelectedValue = CommentAuditTracker.ActiveInactive.Active;
            this.ActiveInactiveButton.Size = new System.Drawing.Size(97, 26);
            this.ActiveInactiveButton.TabIndex = 2;
            this.ActiveInactiveButton.UseVisualStyleBackColor = true;
            this.ActiveInactiveButton.Cycle += new Uheaa.Common.WinForms.CycleButton<CommentAuditTracker.ActiveInactive>.OnCycle(this.ActiveInactiveButton_Cycle);
            // 
            // AgentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AuditPercentageBox);
            this.Controls.Add(this.ManageUtIdsButton);
            this.Controls.Add(this.ActiveInactiveButton);
            this.Controls.Add(this.FullNameBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AgentControl";
            this.Size = new System.Drawing.Size(579, 34);
            ((System.ComponentModel.ISupportInitialize)(this.AuditPercentageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.WatermarkTextBox FullNameBox;
        private ActiveInactiveCycleButton ActiveInactiveButton;
        private System.Windows.Forms.Button ManageUtIdsButton;
        private System.Windows.Forms.NumericUpDown AuditPercentageBox;
        private System.Windows.Forms.Label label1;
    }
}
