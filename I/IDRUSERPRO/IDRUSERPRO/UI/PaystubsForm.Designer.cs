namespace IDRUSERPRO
{
    partial class PaystubsForm
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
            this.EmployersBox = new System.Windows.Forms.ListBox();
            this.EmployerGroup = new System.Windows.Forms.GroupBox();
            this.FrequencyLabel = new System.Windows.Forms.Label();
            this.PayFrequency = new System.Windows.Forms.ComboBox();
            this.RemoveEmployerButton = new System.Windows.Forms.Button();
            this.EmployerNameLabel = new System.Windows.Forms.Label();
            this.EmployerNameBox = new System.Windows.Forms.TextBox();
            this.AddEmployerButton = new System.Windows.Forms.Button();
            this.PaystubsGroup = new System.Windows.Forms.GroupBox();
            this.PaystubsPanel = new System.Windows.Forms.Panel();
            this.AddPaystubButton = new System.Windows.Forms.Button();
            this.TotalsLabel = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.EmployerGroup.SuspendLayout();
            this.PaystubsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Employers";
            // 
            // EmployersBox
            // 
            this.EmployersBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.EmployersBox.FormattingEnabled = true;
            this.EmployersBox.IntegralHeight = false;
            this.EmployersBox.ItemHeight = 20;
            this.EmployersBox.Location = new System.Drawing.Point(12, 36);
            this.EmployersBox.Name = "EmployersBox";
            this.EmployersBox.Size = new System.Drawing.Size(217, 370);
            this.EmployersBox.TabIndex = 0;
            this.EmployersBox.SelectedIndexChanged += new System.EventHandler(this.EmployersBox_SelectedIndexChanged);
            // 
            // EmployerGroup
            // 
            this.EmployerGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EmployerGroup.Controls.Add(this.FrequencyLabel);
            this.EmployerGroup.Controls.Add(this.PayFrequency);
            this.EmployerGroup.Controls.Add(this.RemoveEmployerButton);
            this.EmployerGroup.Controls.Add(this.EmployerNameLabel);
            this.EmployerGroup.Controls.Add(this.EmployerNameBox);
            this.EmployerGroup.Location = new System.Drawing.Point(235, 12);
            this.EmployerGroup.Name = "EmployerGroup";
            this.EmployerGroup.Size = new System.Drawing.Size(747, 85);
            this.EmployerGroup.TabIndex = 2;
            this.EmployerGroup.TabStop = false;
            this.EmployerGroup.Text = "Employer";
            this.EmployerGroup.Leave += new System.EventHandler(this.EmployerGroup_Leave);
            // 
            // FrequencyLabel
            // 
            this.FrequencyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FrequencyLabel.AutoSize = true;
            this.FrequencyLabel.Location = new System.Drawing.Point(507, 22);
            this.FrequencyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FrequencyLabel.Name = "FrequencyLabel";
            this.FrequencyLabel.Size = new System.Drawing.Size(84, 20);
            this.FrequencyLabel.TabIndex = 13;
            this.FrequencyLabel.Text = "Frequency";
            // 
            // PayFrequency
            // 
            this.PayFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PayFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PayFrequency.FormattingEnabled = true;
            this.PayFrequency.Items.AddRange(new object[] {
            "Bi-Weekly",
            "Semi-Monthly",
            "Weekly",
            "Monthly",
            "Yearly"});
            this.PayFrequency.Location = new System.Drawing.Point(511, 43);
            this.PayFrequency.Name = "PayFrequency";
            this.PayFrequency.Size = new System.Drawing.Size(115, 28);
            this.PayFrequency.TabIndex = 1;
            this.PayFrequency.SelectedIndexChanged += new System.EventHandler(this.PayFrequency_SelectedIndexChanged);
            // 
            // RemoveEmployerButton
            // 
            this.RemoveEmployerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveEmployerButton.Location = new System.Drawing.Point(632, 22);
            this.RemoveEmployerButton.Name = "RemoveEmployerButton";
            this.RemoveEmployerButton.Size = new System.Drawing.Size(109, 49);
            this.RemoveEmployerButton.TabIndex = 2;
            this.RemoveEmployerButton.Text = "Remove Employer";
            this.RemoveEmployerButton.UseVisualStyleBackColor = true;
            this.RemoveEmployerButton.Click += new System.EventHandler(this.RemoveEmployerButton_Click);
            // 
            // EmployerNameLabel
            // 
            this.EmployerNameLabel.AutoSize = true;
            this.EmployerNameLabel.Location = new System.Drawing.Point(6, 22);
            this.EmployerNameLabel.Name = "EmployerNameLabel";
            this.EmployerNameLabel.Size = new System.Drawing.Size(51, 20);
            this.EmployerNameLabel.TabIndex = 3;
            this.EmployerNameLabel.Text = "Name";
            // 
            // EmployerNameBox
            // 
            this.EmployerNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EmployerNameBox.Location = new System.Drawing.Point(10, 45);
            this.EmployerNameBox.MaxLength = 50;
            this.EmployerNameBox.Name = "EmployerNameBox";
            this.EmployerNameBox.Size = new System.Drawing.Size(495, 26);
            this.EmployerNameBox.TabIndex = 0;
            this.EmployerNameBox.TextChanged += new System.EventHandler(this.EmployerNameBox_TextChanged);
            // 
            // AddEmployerButton
            // 
            this.AddEmployerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddEmployerButton.Location = new System.Drawing.Point(12, 412);
            this.AddEmployerButton.Name = "AddEmployerButton";
            this.AddEmployerButton.Size = new System.Drawing.Size(217, 29);
            this.AddEmployerButton.TabIndex = 1;
            this.AddEmployerButton.Text = "Add Employer";
            this.AddEmployerButton.UseVisualStyleBackColor = true;
            this.AddEmployerButton.Click += new System.EventHandler(this.AddEmployerButton_Click);
            // 
            // PaystubsGroup
            // 
            this.PaystubsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PaystubsGroup.Controls.Add(this.PaystubsPanel);
            this.PaystubsGroup.Location = new System.Drawing.Point(235, 103);
            this.PaystubsGroup.Name = "PaystubsGroup";
            this.PaystubsGroup.Size = new System.Drawing.Size(747, 303);
            this.PaystubsGroup.TabIndex = 3;
            this.PaystubsGroup.TabStop = false;
            this.PaystubsGroup.Text = "Pay Stubs";
            // 
            // PaystubsPanel
            // 
            this.PaystubsPanel.AutoScroll = true;
            this.PaystubsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaystubsPanel.Location = new System.Drawing.Point(3, 22);
            this.PaystubsPanel.Name = "PaystubsPanel";
            this.PaystubsPanel.Size = new System.Drawing.Size(741, 278);
            this.PaystubsPanel.TabIndex = 0;
            // 
            // AddPaystubButton
            // 
            this.AddPaystubButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddPaystubButton.Location = new System.Drawing.Point(235, 412);
            this.AddPaystubButton.Name = "AddPaystubButton";
            this.AddPaystubButton.Size = new System.Drawing.Size(142, 29);
            this.AddPaystubButton.TabIndex = 4;
            this.AddPaystubButton.Text = "Add Pay Stub";
            this.AddPaystubButton.UseVisualStyleBackColor = true;
            this.AddPaystubButton.Click += new System.EventHandler(this.AddPaystubButton_Click);
            // 
            // TotalsLabel
            // 
            this.TotalsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TotalsLabel.AutoSize = true;
            this.TotalsLabel.Location = new System.Drawing.Point(383, 416);
            this.TotalsLabel.Name = "TotalsLabel";
            this.TotalsLabel.Size = new System.Drawing.Size(85, 20);
            this.TotalsLabel.TabIndex = 8;
            this.TotalsLabel.Text = "Total AGI: ";
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(867, 412);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(112, 29);
            this.OkButton.TabIndex = 5;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // PaystubsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 453);
            this.ControlBox = false;
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.TotalsLabel);
            this.Controls.Add(this.AddPaystubButton);
            this.Controls.Add(this.PaystubsGroup);
            this.Controls.Add(this.AddEmployerButton);
            this.Controls.Add(this.EmployerGroup);
            this.Controls.Add(this.EmployersBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1010, 492);
            this.Name = "PaystubsForm";
            this.Text = "Pay Stubs";
            this.EmployerGroup.ResumeLayout(false);
            this.EmployerGroup.PerformLayout();
            this.PaystubsGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox EmployersBox;
        private System.Windows.Forms.GroupBox EmployerGroup;
        private System.Windows.Forms.Button RemoveEmployerButton;
        private System.Windows.Forms.Label EmployerNameLabel;
        private System.Windows.Forms.TextBox EmployerNameBox;
        private System.Windows.Forms.Button AddEmployerButton;
        private System.Windows.Forms.GroupBox PaystubsGroup;
        private System.Windows.Forms.Panel PaystubsPanel;
        private System.Windows.Forms.Button AddPaystubButton;
        private System.Windows.Forms.Label TotalsLabel;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Label FrequencyLabel;
        private System.Windows.Forms.ComboBox PayFrequency;
    }
}