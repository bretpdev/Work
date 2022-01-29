namespace SIRPTFED.Views
{
    partial class MetricAssign
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
            this.ADUsers = new System.Windows.Forms.ComboBox();
            this.Metric = new System.Windows.Forms.ComboBox();
            this.assignButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ADUsers
            // 
            this.ADUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ADUsers.FormattingEnabled = true;
            this.ADUsers.Location = new System.Drawing.Point(108, 30);
            this.ADUsers.Name = "ADUsers";
            this.ADUsers.Size = new System.Drawing.Size(409, 28);
            this.ADUsers.TabIndex = 0;
            // 
            // Metric
            // 
            this.Metric.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Metric.FormattingEnabled = true;
            this.Metric.Location = new System.Drawing.Point(108, 86);
            this.Metric.Name = "Metric";
            this.Metric.Size = new System.Drawing.Size(409, 28);
            this.Metric.TabIndex = 1;
            this.Metric.SelectedIndexChanged += new System.EventHandler(this.Metric_SelectedIndexChanged);
            this.Metric.SelectionChangeCommitted += new System.EventHandler(this.Metric_SelectionChangeCommitted);
            // 
            // assignButton
            // 
            this.assignButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assignButton.Location = new System.Drawing.Point(273, 143);
            this.assignButton.Name = "assignButton";
            this.assignButton.Size = new System.Drawing.Size(96, 33);
            this.assignButton.TabIndex = 2;
            this.assignButton.Text = "Assign";
            this.assignButton.UseVisualStyleBackColor = true;
            this.assignButton.Click += new System.EventHandler(this.assignButton_Click);
            // 
            // MetricAssign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 211);
            this.Controls.Add(this.assignButton);
            this.Controls.Add(this.Metric);
            this.Controls.Add(this.ADUsers);
            this.Name = "MetricAssign";
            this.Text = "MetricAssign";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ADUsers;
        private System.Windows.Forms.ComboBox Metric;
        private System.Windows.Forms.Button assignButton;
    }
}