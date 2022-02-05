namespace INCIDENTRP
{
    partial class HandleSsn
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
            this.MaskSsn = new System.Windows.Forms.Button();
            this.NotSsn = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 106);
            this.label1.TabIndex = 0;
            this.label1.Text = "A 9 digit number or what appears to be a Social Security Number (SSN) was found i" +
    "n the ticket. Please select a course of action:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MaskSsn
            // 
            this.MaskSsn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaskSsn.Location = new System.Drawing.Point(41, 121);
            this.MaskSsn.Name = "MaskSsn";
            this.MaskSsn.Size = new System.Drawing.Size(200, 37);
            this.MaskSsn.TabIndex = 1;
            this.MaskSsn.Text = "Mask SSN";
            this.MaskSsn.UseVisualStyleBackColor = true;
            this.MaskSsn.Click += new System.EventHandler(this.MaskSsn_Click);
            // 
            // NotSsn
            // 
            this.NotSsn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NotSsn.Location = new System.Drawing.Point(41, 164);
            this.NotSsn.Name = "NotSsn";
            this.NotSsn.Size = new System.Drawing.Size(200, 37);
            this.NotSsn.TabIndex = 2;
            this.NotSsn.Text = "Don\'t Mask";
            this.NotSsn.UseVisualStyleBackColor = true;
            this.NotSsn.Click += new System.EventHandler(this.NotSsn_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.Location = new System.Drawing.Point(41, 207);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(200, 37);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // HandleSsn
            // 
            this.AcceptButton = this.MaskSsn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(283, 260);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.NotSsn);
            this.Controls.Add(this.MaskSsn);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(299, 294);
            this.MinimumSize = new System.Drawing.Size(299, 294);
            this.Name = "HandleSsn";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleSsn_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MaskSsn;
        private System.Windows.Forms.Button NotSsn;
        private System.Windows.Forms.Button Cancel;
    }
}