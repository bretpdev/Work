namespace ErrorFinder
{
    partial class ExportPopup
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
            this.OpenLocationButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ProgressBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // OpenLocationButton
            // 
            this.OpenLocationButton.Location = new System.Drawing.Point(217, 4);
            this.OpenLocationButton.Name = "OpenLocationButton";
            this.OpenLocationButton.Size = new System.Drawing.Size(132, 23);
            this.OpenLocationButton.TabIndex = 1;
            this.OpenLocationButton.Text = "Open Export Location";
            this.OpenLocationButton.UseVisualStyleBackColor = true;
            this.OpenLocationButton.Visible = false;
            this.OpenLocationButton.Click += new System.EventHandler(this.OpenLocationButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Progress";
            // 
            // ProgressBox
            // 
            this.ProgressBox.FormattingEnabled = true;
            this.ProgressBox.Location = new System.Drawing.Point(15, 33);
            this.ProgressBox.Name = "ProgressBox";
            this.ProgressBox.Size = new System.Drawing.Size(334, 95);
            this.ProgressBox.TabIndex = 4;
            // 
            // ExportPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 132);
            this.Controls.Add(this.ProgressBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OpenLocationButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportPopup";
            this.Text = "Generating Report";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Popup_FormClosing);
            this.Shown += new System.EventHandler(this.Popup_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenLocationButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox ProgressBox;
    }
}