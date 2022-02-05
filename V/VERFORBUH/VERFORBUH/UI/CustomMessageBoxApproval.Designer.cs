namespace VERFORBUH
{
    partial class CustomMessageBoxApproval
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
            this.Text1 = new System.Windows.Forms.Label();
            this.EndDate = new System.Windows.Forms.Label();
            this.Text2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.Icon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).BeginInit();
            this.SuspendLayout();
            // 
            // Text1
            // 
            this.Text1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text1.Location = new System.Drawing.Point(165, 9);
            this.Text1.Name = "Text1";
            this.Text1.Size = new System.Drawing.Size(337, 57);
            this.Text1.TabIndex = 0;
            this.Text1.Text = "Due to the status of the borrowers account, the forbearance was approved until:";
            this.Text1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EndDate
            // 
            this.EndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndDate.ForeColor = System.Drawing.Color.Red;
            this.EndDate.Location = new System.Drawing.Point(165, 77);
            this.EndDate.Name = "EndDate";
            this.EndDate.Size = new System.Drawing.Size(337, 30);
            this.EndDate.TabIndex = 2;
            this.EndDate.Text = "Forbearance End Date: ";
            this.EndDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Text2
            // 
            this.Text2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text2.Location = new System.Drawing.Point(165, 118);
            this.Text2.Name = "Text2";
            this.Text2.Size = new System.Drawing.Size(337, 85);
            this.Text2.TabIndex = 3;
            this.Text2.Text = "Forbearance Has Been Approved";
            this.Text2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(206, 206);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 51);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Icon
            // 
            this.Icon.Image = global::VERFORBUH.Properties.Resources.webdev_ok_icon___Copy1;
            this.Icon.InitialImage = null;
            this.Icon.Location = new System.Drawing.Point(12, 46);
            this.Icon.Name = "Icon";
            this.Icon.Size = new System.Drawing.Size(128, 128);
            this.Icon.TabIndex = 5;
            this.Icon.TabStop = false;
            // 
            // CustomMessageBoxApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 257);
            this.ControlBox = false;
            this.Controls.Add(this.Icon);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Text2);
            this.Controls.Add(this.EndDate);
            this.Controls.Add(this.Text1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(575, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 0);
            this.Name = "CustomMessageBoxApproval";
            this.ShowIcon = false;
            this.Text = "Important Notice!!";
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Text1;
        private System.Windows.Forms.Label EndDate;
        private System.Windows.Forms.Label Text2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox Icon;
    }
}