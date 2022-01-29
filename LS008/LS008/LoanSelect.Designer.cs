namespace LS008
{
    partial class LoanSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoanSelect));
            this.Continue = new System.Windows.Forms.Button();
            this.Loans = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // Continue
            // 
            this.Continue.Location = new System.Drawing.Point(146, 362);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(91, 34);
            this.Continue.TabIndex = 1;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // Loans
            // 
            this.Loans.CheckOnClick = true;
            this.Loans.FormattingEnabled = true;
            this.Loans.Location = new System.Drawing.Point(12, 12);
            this.Loans.Name = "Loans";
            this.Loans.Size = new System.Drawing.Size(225, 340);
            this.Loans.TabIndex = 2;
            // 
            // LoanSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 407);
            this.Controls.Add(this.Loans);
            this.Controls.Add(this.Continue);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "LoanSelect";
            this.Text = "Select Loans for ARC";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.CheckedListBox Loans;
    }
}