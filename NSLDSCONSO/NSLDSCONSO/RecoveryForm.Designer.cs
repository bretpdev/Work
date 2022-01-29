namespace NSLDSCONSO
{
    partial class RecoveryForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.StartBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.UpdateBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ElapsedBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ProgressBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.IdBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.FilenameBox = new System.Windows.Forms.TextBox();
            this.RecoveryButton = new System.Windows.Forms.Button();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.FileCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(469, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "The last Data History Load seems to have not finished successfully.";
            // 
            // StartBox
            // 
            this.StartBox.BackColor = System.Drawing.Color.White;
            this.StartBox.Location = new System.Drawing.Point(15, 61);
            this.StartBox.Name = "StartBox";
            this.StartBox.ReadOnly = true;
            this.StartBox.Size = new System.Drawing.Size(261, 26);
            this.StartBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start";
            // 
            // UpdateBox
            // 
            this.UpdateBox.BackColor = System.Drawing.Color.White;
            this.UpdateBox.Location = new System.Drawing.Point(282, 61);
            this.UpdateBox.Name = "UpdateBox";
            this.UpdateBox.ReadOnly = true;
            this.UpdateBox.Size = new System.Drawing.Size(257, 26);
            this.UpdateBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(279, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Last Update";
            // 
            // ElapsedBox
            // 
            this.ElapsedBox.BackColor = System.Drawing.Color.White;
            this.ElapsedBox.Location = new System.Drawing.Point(15, 111);
            this.ElapsedBox.Name = "ElapsedBox";
            this.ElapsedBox.ReadOnly = true;
            this.ElapsedBox.Size = new System.Drawing.Size(127, 26);
            this.ElapsedBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "Elapsed";
            // 
            // ProgressBox
            // 
            this.ProgressBox.BackColor = System.Drawing.Color.White;
            this.ProgressBox.Location = new System.Drawing.Point(282, 111);
            this.ProgressBox.Name = "ProgressBox";
            this.ProgressBox.ReadOnly = true;
            this.ProgressBox.Size = new System.Drawing.Size(257, 26);
            this.ProgressBox.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(279, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 18);
            this.label5.TabIndex = 10;
            this.label5.Text = "Progress";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(146, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 18);
            this.label6.TabIndex = 13;
            this.label6.Text = "DataLoadRunId";
            // 
            // IdBox
            // 
            this.IdBox.BackColor = System.Drawing.Color.White;
            this.IdBox.Location = new System.Drawing.Point(149, 111);
            this.IdBox.Name = "IdBox";
            this.IdBox.ReadOnly = true;
            this.IdBox.Size = new System.Drawing.Size(127, 26);
            this.IdBox.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 18);
            this.label7.TabIndex = 15;
            this.label7.Text = "Filename";
            // 
            // FilenameBox
            // 
            this.FilenameBox.BackColor = System.Drawing.Color.White;
            this.FilenameBox.Location = new System.Drawing.Point(15, 161);
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.ReadOnly = true;
            this.FilenameBox.Size = new System.Drawing.Size(524, 26);
            this.FilenameBox.TabIndex = 14;
            // 
            // RecoveryButton
            // 
            this.RecoveryButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.RecoveryButton.Enabled = false;
            this.RecoveryButton.Location = new System.Drawing.Point(355, 193);
            this.RecoveryButton.Name = "RecoveryButton";
            this.RecoveryButton.Size = new System.Drawing.Size(184, 40);
            this.RecoveryButton.TabIndex = 16;
            this.RecoveryButton.Text = "Attempt Recovery";
            this.RecoveryButton.UseVisualStyleBackColor = true;
            // 
            // ContinueButton
            // 
            this.ContinueButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ContinueButton.Location = new System.Drawing.Point(15, 193);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(223, 40);
            this.ContinueButton.TabIndex = 17;
            this.ContinueButton.Text = "Continue without Recovery";
            this.ContinueButton.UseVisualStyleBackColor = true;
            // 
            // FileCheckTimer
            // 
            this.FileCheckTimer.Enabled = true;
            this.FileCheckTimer.Interval = 1000;
            this.FileCheckTimer.Tick += new System.EventHandler(this.FileCheckTimer_Tick);
            // 
            // RecoveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 243);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.RecoveryButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FilenameBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.IdBox);
            this.Controls.Add(this.ProgressBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ElapsedBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.UpdateBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StartBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecoveryForm";
            this.Text = "NSLDSCONSO - Recovery";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox StartBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UpdateBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ElapsedBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ProgressBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox IdBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox FilenameBox;
        private System.Windows.Forms.Button RecoveryButton;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.Timer FileCheckTimer;
    }
}