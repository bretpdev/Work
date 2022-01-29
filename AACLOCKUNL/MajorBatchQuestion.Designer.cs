namespace AACLOCKUNL
{
    partial class MajorBatchQuestion
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
            this.HaveBatch = new System.Windows.Forms.Button();
            this.DoNotHaveBatch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HaveBatch
            // 
            this.HaveBatch.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.HaveBatch.Location = new System.Drawing.Point(27, 78);
            this.HaveBatch.Name = "HaveBatch";
            this.HaveBatch.Size = new System.Drawing.Size(246, 23);
            this.HaveBatch.TabIndex = 0;
            this.HaveBatch.Text = "I have the Major Batch Numbers";
            this.HaveBatch.UseVisualStyleBackColor = true;
            // 
            // DoNotHaveBatch
            // 
            this.DoNotHaveBatch.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoNotHaveBatch.Location = new System.Drawing.Point(27, 120);
            this.DoNotHaveBatch.Name = "DoNotHaveBatch";
            this.DoNotHaveBatch.Size = new System.Drawing.Size(246, 23);
            this.DoNotHaveBatch.TabIndex = 1;
            this.DoNotHaveBatch.Text = "I do not have the Major Batch Numbers yet";
            this.DoNotHaveBatch.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 19);
            this.label1.MaximumSize = new System.Drawing.Size(250, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 39);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please note, this script will only go in and lock or unlock the minor batches for" +
    " all Major Batches that are entered on the next screen.";
            // 
            // MajorBatchQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 171);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DoNotHaveBatch);
            this.Controls.Add(this.HaveBatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MajorBatchQuestion";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AAC Lock & Unlock";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button HaveBatch;
        private System.Windows.Forms.Button DoNotHaveBatch;
        private System.Windows.Forms.Label label1;
    }
}