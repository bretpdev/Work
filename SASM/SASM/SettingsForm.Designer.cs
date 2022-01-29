namespace SASM
{
    partial class SettingsForm
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
            this.UsernameText = new System.Windows.Forms.TextBox();
            this.LegendText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DusterLiveText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.DusterTestText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // UsernameText
            // 
            this.UsernameText.Location = new System.Drawing.Point(14, 31);
            this.UsernameText.MaxLength = 8;
            this.UsernameText.Name = "UsernameText";
            this.UsernameText.Size = new System.Drawing.Size(145, 23);
            this.UsernameText.TabIndex = 1;
            // 
            // LegendText
            // 
            this.LegendText.Location = new System.Drawing.Point(14, 79);
            this.LegendText.Name = "LegendText";
            this.LegendText.Size = new System.Drawing.Size(145, 23);
            this.LegendText.TabIndex = 3;
            this.LegendText.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Legend Password";
            // 
            // DusterLiveText
            // 
            this.DusterLiveText.Location = new System.Drawing.Point(14, 127);
            this.DusterLiveText.Name = "DusterLiveText";
            this.DusterLiveText.Size = new System.Drawing.Size(145, 23);
            this.DusterLiveText.TabIndex = 5;
            this.DusterLiveText.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Duster LIVE Password";
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(11, 202);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(148, 29);
            this.OkButton.TabIndex = 7;
            this.OkButton.Text = "Save Settings";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // DusterTestText
            // 
            this.DusterTestText.Location = new System.Drawing.Point(14, 173);
            this.DusterTestText.Name = "DusterTestText";
            this.DusterTestText.Size = new System.Drawing.Size(145, 23);
            this.DusterTestText.TabIndex = 15;
            this.DusterTestText.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Duster TEST Password";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(174, 243);
            this.Controls.Add(this.DusterTestText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.DusterLiveText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LegendText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UsernameText);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UsernameText;
        private System.Windows.Forms.TextBox LegendText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DusterLiveText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.TextBox DusterTestText;
        private System.Windows.Forms.Label label4;
    }
}