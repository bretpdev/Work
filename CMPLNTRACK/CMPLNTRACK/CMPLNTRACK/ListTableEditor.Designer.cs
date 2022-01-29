namespace CMPLNTRACK
{
    partial class ListTableEditor
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
            this.MainList = new System.Windows.Forms.ListBox();
            this.NewButton = new System.Windows.Forms.Button();
            this.RetireButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainList
            // 
            this.MainList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainList.FormattingEnabled = true;
            this.MainList.IntegralHeight = false;
            this.MainList.ItemHeight = 16;
            this.MainList.Location = new System.Drawing.Point(13, 13);
            this.MainList.Margin = new System.Windows.Forms.Padding(4);
            this.MainList.Name = "MainList";
            this.MainList.Size = new System.Drawing.Size(282, 223);
            this.MainList.TabIndex = 0;
            this.MainList.SelectedIndexChanged += new System.EventHandler(this.MainList_SelectedIndexChanged);
            // 
            // NewButton
            // 
            this.NewButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewButton.Location = new System.Drawing.Point(120, 243);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(175, 30);
            this.NewButton.TabIndex = 1;
            this.NewButton.Text = "New {0}";
            this.NewButton.UseVisualStyleBackColor = true;
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // RetireButton
            // 
            this.RetireButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RetireButton.Enabled = false;
            this.RetireButton.Location = new System.Drawing.Point(13, 243);
            this.RetireButton.Name = "RetireButton";
            this.RetireButton.Size = new System.Drawing.Size(100, 30);
            this.RetireButton.TabIndex = 2;
            this.RetireButton.Text = "Retire";
            this.RetireButton.UseVisualStyleBackColor = true;
            this.RetireButton.Click += new System.EventHandler(this.RetireButton_Click);
            // 
            // ListTableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 285);
            this.Controls.Add(this.RetireButton);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.MainList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(322, 324);
            this.Name = "ListTableEditor";
            this.Text = "Manage {1}";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox MainList;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.Button RetireButton;
    }
}