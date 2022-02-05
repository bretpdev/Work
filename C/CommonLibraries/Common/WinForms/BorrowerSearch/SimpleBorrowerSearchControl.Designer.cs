namespace Uheaa.Common.WinForms
{
    partial class SimpleBorrowerSearchControl
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
            this.SearchButton = new System.Windows.Forms.Button();
            this.LastNameBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.FirstNameBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.SuspendLayout();
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Enabled = false;
            this.SearchButton.Location = new System.Drawing.Point(233, 3);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(87, 26);
            this.SearchButton.TabIndex = 16;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // LastNameBox
            // 
            this.LastNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LastNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.LastNameBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LastNameBox.Location = new System.Drawing.Point(105, 3);
            this.LastNameBox.MaxLength = 26;
            this.LastNameBox.Name = "LastNameBox";
            this.LastNameBox.Size = new System.Drawing.Size(122, 26);
            this.LastNameBox.TabIndex = 15;
            this.LastNameBox.Text = "Last Name";
            this.LastNameBox.Watermark = "Last Name";
            this.LastNameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
            this.LastNameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Key_Press);
            this.LastNameBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // FirstNameBox
            // 
            this.FirstNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.FirstNameBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FirstNameBox.Location = new System.Drawing.Point(3, 3);
            this.FirstNameBox.MaxLength = 13;
            this.FirstNameBox.Name = "FirstNameBox";
            this.FirstNameBox.Size = new System.Drawing.Size(96, 26);
            this.FirstNameBox.TabIndex = 14;
            this.FirstNameBox.Text = "First Name";
            this.FirstNameBox.Watermark = "First Name";
            this.FirstNameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
            this.FirstNameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Key_Press);
            this.FirstNameBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // SimpleBorrowerSearchControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.FirstNameBox);
            this.Controls.Add(this.LastNameBox);
            this.Name = "SimpleBorrowerSearchControl";
            this.Size = new System.Drawing.Size(330, 33);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchButton;
        private WatermarkTextBox LastNameBox;
        private WatermarkTextBox FirstNameBox;
    }
}
