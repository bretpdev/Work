namespace BARCODEFED
{
    partial class ReceivedDate
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
            this.selectedDate = new System.Windows.Forms.DateTimePicker();
            this.Select = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // selectedDate
            // 
            this.selectedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedDate.Location = new System.Drawing.Point(63, 34);
            this.selectedDate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectedDate.Name = "selectedDate";
            this.selectedDate.Size = new System.Drawing.Size(288, 26);
            this.selectedDate.TabIndex = 0;
            this.selectedDate.Value = new System.DateTime(2019, 6, 27, 11, 29, 47, 0);
            // 
            // Select
            // 
            this.Select.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Select.Location = new System.Drawing.Point(152, 70);
            this.Select.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Select.Name = "Select";
            this.Select.Size = new System.Drawing.Size(93, 31);
            this.Select.TabIndex = 1;
            this.Select.Text = "Select";
            this.Select.UseVisualStyleBackColor = true;
            this.Select.Click += new System.EventHandler(this.Select_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(388, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please select the date the returned mail was received.";
            // 
            // ReceivedDate
            // 
            this.AcceptButton = this.Select;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 112);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Select);
            this.Controls.Add(this.selectedDate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(426, 151);
            this.MinimumSize = new System.Drawing.Size(426, 151);
            this.Name = "ReceivedDate";
            this.Text = "Received Date";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Select;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DateTimePicker selectedDate;
    }
}