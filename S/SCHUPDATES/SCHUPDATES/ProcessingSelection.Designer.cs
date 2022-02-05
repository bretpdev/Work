namespace SCHUPDATES
{
    partial class ProcessingSelection
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
            this.File = new System.Windows.Forms.Button();
            this.User = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // File
            // 
            this.File.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.File.Location = new System.Drawing.Point(4, 14);
            this.File.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(162, 77);
            this.File.TabIndex = 0;
            this.File.Text = "File Processing";
            this.File.UseVisualStyleBackColor = true;
            this.File.Click += new System.EventHandler(this.File_Click);
            // 
            // User
            // 
            this.User.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.User.Location = new System.Drawing.Point(4, 105);
            this.User.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.User.Name = "User";
            this.User.Size = new System.Drawing.Size(162, 77);
            this.User.TabIndex = 1;
            this.User.Text = "User Processing";
            this.User.UseVisualStyleBackColor = true;
            // 
            // ProcessingSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(174, 194);
            this.ControlBox = false;
            this.Controls.Add(this.User);
            this.Controls.Add(this.File);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(180, 200);
            this.MinimumSize = new System.Drawing.Size(180, 200);
            this.Name = "ProcessingSelection";
            this.ShowIcon = false;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button File;
        private System.Windows.Forms.Button User;
    }
}