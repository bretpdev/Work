namespace MassAssignRangeAssignment
{
    partial class Queues
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
            this.QueueNameBox = new System.Windows.Forms.CheckBox();
            this.FutureDatedBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // QueueNameBox
            // 
            this.QueueNameBox.AutoSize = true;
            this.QueueNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QueueNameBox.Location = new System.Drawing.Point(3, 2);
            this.QueueNameBox.Name = "QueueNameBox";
            this.QueueNameBox.Size = new System.Drawing.Size(15, 14);
            this.QueueNameBox.TabIndex = 0;
            this.QueueNameBox.UseVisualStyleBackColor = true;
            this.QueueNameBox.CheckedChanged += new System.EventHandler(this.QueueNameBox_CheckedChanged);
            // 
            // FutureDatedBox
            // 
            this.FutureDatedBox.AutoSize = true;
            this.FutureDatedBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FutureDatedBox.Location = new System.Drawing.Point(94, 0);
            this.FutureDatedBox.Name = "FutureDatedBox";
            this.FutureDatedBox.Size = new System.Drawing.Size(104, 20);
            this.FutureDatedBox.TabIndex = 1;
            this.FutureDatedBox.Text = "Future Dated";
            this.FutureDatedBox.UseVisualStyleBackColor = true;
            this.FutureDatedBox.CheckedChanged += new System.EventHandler(this.FutureDatedBox_CheckedChanged);
            // 
            // Queues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FutureDatedBox);
            this.Controls.Add(this.QueueNameBox);
            this.Name = "Queues";
            this.Size = new System.Drawing.Size(198, 19);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox QueueNameBox;
        private System.Windows.Forms.CheckBox FutureDatedBox;
    }
}
