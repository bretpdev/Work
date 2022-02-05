namespace ScriptSyncTester
{
    partial class TestForm
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
            this.ScriptIdText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ModeSelector = new System.Windows.Forms.ComboBox();
            this.TestButton = new System.Windows.Forms.Button();
            this.TestRunButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FullTestMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Script ID";
            // 
            // ScriptIdText
            // 
            this.ScriptIdText.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScriptIdText.Location = new System.Drawing.Point(114, 21);
            this.ScriptIdText.MaxLength = 10;
            this.ScriptIdText.Name = "ScriptIdText";
            this.ScriptIdText.Size = new System.Drawing.Size(228, 32);
            this.ScriptIdText.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mode";
            // 
            // ModeSelector
            // 
            this.ModeSelector.DisplayMember = "Text";
            this.ModeSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModeSelector.FormattingEnabled = true;
            this.ModeSelector.Location = new System.Drawing.Point(114, 64);
            this.ModeSelector.Name = "ModeSelector";
            this.ModeSelector.Size = new System.Drawing.Size(228, 33);
            this.ModeSelector.TabIndex = 3;
            this.ModeSelector.ValueMember = "Value";
            // 
            // TestButton
            // 
            this.TestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestButton.Location = new System.Drawing.Point(222, 103);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(120, 46);
            this.TestButton.TabIndex = 4;
            this.TestButton.Text = "Test";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // TestRunButton
            // 
            this.TestRunButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestRunButton.Location = new System.Drawing.Point(12, 103);
            this.TestRunButton.Name = "TestRunButton";
            this.TestRunButton.Size = new System.Drawing.Size(204, 46);
            this.TestRunButton.TabIndex = 5;
            this.TestRunButton.Text = "Test and Run";
            this.TestRunButton.UseVisualStyleBackColor = true;
            this.TestRunButton.Click += new System.EventHandler(this.TestRunButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FullTestMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(354, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FullTestMenu
            // 
            this.FullTestMenu.Name = "FullTestMenu";
            this.FullTestMenu.Size = new System.Drawing.Size(63, 20);
            this.FullTestMenu.Text = "Full Test";
            this.FullTestMenu.Click += new System.EventHandler(this.FullTestMenu_Click);
            // 
            // TestForm
            // 
            this.AcceptButton = this.TestButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 162);
            this.Controls.Add(this.TestRunButton);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.ModeSelector);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ScriptIdText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestForm";
            this.Text = "ScriptSync Tester";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ScriptIdText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ModeSelector;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.Button TestRunButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FullTestMenu;
    }
}

