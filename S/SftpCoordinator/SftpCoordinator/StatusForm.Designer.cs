namespace SftpCoordinator
{
    partial class StatusForm
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
            this.ActivityList = new System.Windows.Forms.ListBox();
            this.StopStartBox = new System.Windows.Forms.PictureBox();
            this.SettingsBox = new System.Windows.Forms.PictureBox();
            this.Divider = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MinimizeBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.StopStartBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimizeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Activity Log";
            // 
            // ActivityList
            // 
            this.ActivityList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivityList.FormattingEnabled = true;
            this.ActivityList.ItemHeight = 16;
            this.ActivityList.Location = new System.Drawing.Point(12, 120);
            this.ActivityList.Name = "ActivityList";
            this.ActivityList.Size = new System.Drawing.Size(785, 420);
            this.ActivityList.TabIndex = 1;
            // 
            // StopStartBox
            // 
            this.StopStartBox.Image = global::SftpCoordinator.Properties.Resources.stop;
            this.StopStartBox.InitialImage = null;
            this.StopStartBox.Location = new System.Drawing.Point(12, 12);
            this.StopStartBox.Name = "StopStartBox";
            this.StopStartBox.Size = new System.Drawing.Size(70, 70);
            this.StopStartBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.StopStartBox.TabIndex = 2;
            this.StopStartBox.TabStop = false;
            this.StopStartBox.Click += new System.EventHandler(this.StopStartBox_Click);
            // 
            // SettingsBox
            // 
            this.SettingsBox.Image = global::SftpCoordinator.Properties.Resources.settings;
            this.SettingsBox.InitialImage = null;
            this.SettingsBox.Location = new System.Drawing.Point(727, 12);
            this.SettingsBox.Name = "SettingsBox";
            this.SettingsBox.Size = new System.Drawing.Size(70, 70);
            this.SettingsBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SettingsBox.TabIndex = 3;
            this.SettingsBox.TabStop = false;
            this.SettingsBox.Click += new System.EventHandler(this.SettingsBox_Click);
            // 
            // Divider
            // 
            this.Divider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Divider.Location = new System.Drawing.Point(12, 85);
            this.Divider.Name = "Divider";
            this.Divider.Size = new System.Drawing.Size(785, 3);
            this.Divider.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(365, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // MinimizeBox
            // 
            this.MinimizeBox.Image = global::SftpCoordinator.Properties.Resources.minimize;
            this.MinimizeBox.InitialImage = null;
            this.MinimizeBox.Location = new System.Drawing.Point(170, 12);
            this.MinimizeBox.Name = "MinimizeBox";
            this.MinimizeBox.Size = new System.Drawing.Size(70, 70);
            this.MinimizeBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MinimizeBox.TabIndex = 6;
            this.MinimizeBox.TabStop = false;
            // 
            // StatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 556);
            this.Controls.Add(this.MinimizeBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Divider);
            this.Controls.Add(this.SettingsBox);
            this.Controls.Add(this.StopStartBox);
            this.Controls.Add(this.ActivityList);
            this.Controls.Add(this.label1);
            this.Name = "StatusForm";
            this.Text = "SFTP Coordinator";
            ((System.ComponentModel.ISupportInitialize)(this.StopStartBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinimizeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox ActivityList;
        private System.Windows.Forms.PictureBox StopStartBox;
        private System.Windows.Forms.PictureBox SettingsBox;
        private System.Windows.Forms.Label Divider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox MinimizeBox;
    }
}