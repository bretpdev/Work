namespace TCPAFEDPS
{
    partial class ModeChooser
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Batch = new System.Windows.Forms.Button();
            this.Manual = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(12, 46);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 44);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 34);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please select which mode you wish to run in: ";
            // 
            // Batch
            // 
            this.Batch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Batch.Location = new System.Drawing.Point(234, 46);
            this.Batch.Name = "Batch";
            this.Batch.Size = new System.Drawing.Size(105, 44);
            this.Batch.TabIndex = 3;
            this.Batch.Text = "Batch";
            this.Batch.UseVisualStyleBackColor = true;
            this.Batch.Click += new System.EventHandler(this.Batch_Click);
            // 
            // Manual
            // 
            this.Manual.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Manual.Location = new System.Drawing.Point(123, 46);
            this.Manual.Name = "Manual";
            this.Manual.Size = new System.Drawing.Size(105, 44);
            this.Manual.TabIndex = 4;
            this.Manual.Text = "Manual";
            this.Manual.UseVisualStyleBackColor = true;
            this.Manual.Click += new System.EventHandler(this.Manual_Click);
            // 
            // ModeChooser
            // 
            this.AcceptButton = this.Batch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 110);
            this.Controls.Add(this.Manual);
            this.Controls.Add(this.Batch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Name = "ModeChooser";
            this.Text = "Mode Chooser";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Batch;
        private System.Windows.Forms.Button Manual;
    }
}