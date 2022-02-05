namespace OLDEMOS
{
    partial class LoginForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UtIdBox = new Uheaa.Common.WinForms.UtIdTextBox();
            this.QuickUtIdPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.WarningLabel = new System.Windows.Forms.Label();
            this.CommonMenu1 = new OLDEMOS.CommonMenu();
            this.TestModeCheckButton = new Uheaa.Common.WinForms.CheckButton();
            this.groupBox1.SuspendLayout();
            this.QuickUtIdPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.UtIdBox);
            this.groupBox1.Controls.Add(this.QuickUtIdPanel);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F);
            this.groupBox1.Location = new System.Drawing.Point(10, 303);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 90);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User ID";
            // 
            // UtIdBox
            // 
            this.UtIdBox.Location = new System.Drawing.Point(3, 19);
            this.UtIdBox.Margin = new System.Windows.Forms.Padding(0);
            this.UtIdBox.Name = "UtIdBox";
            this.UtIdBox.Size = new System.Drawing.Size(185, 26);
            this.UtIdBox.TabIndex = 1;
            this.UtIdBox.UtIdChanged += new System.EventHandler(this.UtIdBox_UtIdChanged);
            // 
            // QuickUtIdPanel
            // 
            this.QuickUtIdPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuickUtIdPanel.AutoScroll = true;
            this.QuickUtIdPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.QuickUtIdPanel.Controls.Add(this.linkLabel1);
            this.QuickUtIdPanel.Controls.Add(this.linkLabel2);
            this.QuickUtIdPanel.Location = new System.Drawing.Point(6, 52);
            this.QuickUtIdPanel.Margin = new System.Windows.Forms.Padding(0);
            this.QuickUtIdPanel.Name = "QuickUtIdPanel";
            this.QuickUtIdPanel.Size = new System.Drawing.Size(182, 35);
            this.QuickUtIdPanel.TabIndex = 100;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(0, 0);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(73, 18);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "UT00905";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(73, 0);
            this.linkLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(73, 18);
            this.linkLabel2.TabIndex = 1;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "UT00906";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F);
            this.label2.Location = new System.Drawing.Point(207, 303);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // PasswordBox
            // 
            this.PasswordBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PasswordBox.Font = new System.Drawing.Font("Arial", 12F);
            this.PasswordBox.Location = new System.Drawing.Point(210, 322);
            this.PasswordBox.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordBox.MaxLength = 8;
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.PasswordChar = '*';
            this.PasswordBox.Size = new System.Drawing.Size(199, 26);
            this.PasswordBox.TabIndex = 2;
            this.PasswordBox.TextChanged += new System.EventHandler(this.PasswordBox_TextChanged);
            this.PasswordBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordBox_KeyDown);
            // 
            // LoginButton
            // 
            this.LoginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginButton.Enabled = false;
            this.LoginButton.Font = new System.Drawing.Font("Arial", 12F);
            this.LoginButton.Location = new System.Drawing.Point(210, 399);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(199, 35);
            this.LoginButton.TabIndex = 5;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginCallsButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.WarningLabel);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F);
            this.groupBox2.Location = new System.Drawing.Point(10, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(399, 270);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PHEAA Warning";
            // 
            // WarningLabel
            // 
            this.WarningLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WarningLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WarningLabel.Location = new System.Drawing.Point(3, 22);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(393, 245);
            this.WarningLabel.TabIndex = 0;
            // 
            // CommonMenu1
            // 
            this.CommonMenu1.Location = new System.Drawing.Point(0, 0);
            this.CommonMenu1.Name = "CommonMenu1";
            this.CommonMenu1.Size = new System.Drawing.Size(420, 24);
            this.CommonMenu1.TabIndex = 6;
            this.CommonMenu1.Text = "commonMenu1";
            // 
            // TestModeCheckButton
            // 
            this.TestModeCheckButton.Font = new System.Drawing.Font("Arial", 12F);
            this.TestModeCheckButton.IsChecked = false;
            this.TestModeCheckButton.Location = new System.Drawing.Point(210, 355);
            this.TestModeCheckButton.Name = "TestModeCheckButton";
            this.TestModeCheckButton.Size = new System.Drawing.Size(198, 35);
            this.TestModeCheckButton.TabIndex = 3;
            this.TestModeCheckButton.Text = "Test Mode";
            this.TestModeCheckButton.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.LoginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 442);
            this.Controls.Add(this.TestModeCheckButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.CommonMenu1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.CommonMenu1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(436, 481);
            this.Name = "LoginForm";
            this.Text = "Login";
            this.groupBox1.ResumeLayout(false);
            this.QuickUtIdPanel.ResumeLayout(false);
            this.QuickUtIdPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox PasswordBox;
        public Uheaa.Common.WinForms.UtIdTextBox UtIdBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel QuickUtIdPanel;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label WarningLabel;
        private CommonMenu CommonMenu1;
        private Uheaa.Common.WinForms.CheckButton TestModeCheckButton;
    }
}