namespace VERFORBFED
{
    partial class CollectionSuspenseReason
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
            this.Def = new System.Windows.Forms.RadioButton();
            this.Forb = new System.Windows.Forms.RadioButton();
            this.LoanCon = new System.Windows.Forms.RadioButton();
            this.IDR = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Continue = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Def
            // 
            this.Def.AutoSize = true;
            this.Def.Location = new System.Drawing.Point(16, 94);
            this.Def.Name = "Def";
            this.Def.Size = new System.Drawing.Size(103, 24);
            this.Def.TabIndex = 0;
            this.Def.Text = "Deferment";
            this.Def.UseVisualStyleBackColor = true;
            // 
            // Forb
            // 
            this.Forb.AutoSize = true;
            this.Forb.Location = new System.Drawing.Point(16, 136);
            this.Forb.Name = "Forb";
            this.Forb.Size = new System.Drawing.Size(118, 24);
            this.Forb.TabIndex = 1;
            this.Forb.Text = "Forbearance";
            this.Forb.UseVisualStyleBackColor = true;
            // 
            // LoanCon
            // 
            this.LoanCon.AutoSize = true;
            this.LoanCon.Location = new System.Drawing.Point(16, 220);
            this.LoanCon.Name = "LoanCon";
            this.LoanCon.Size = new System.Drawing.Size(163, 24);
            this.LoanCon.TabIndex = 2;
            this.LoanCon.Text = "Loan Consolidation";
            this.LoanCon.UseVisualStyleBackColor = true;
            // 
            // IDR
            // 
            this.IDR.AutoSize = true;
            this.IDR.Location = new System.Drawing.Point(16, 178);
            this.IDR.Name = "IDR";
            this.IDR.Size = new System.Drawing.Size(56, 24);
            this.IDR.TabIndex = 3;
            this.IDR.Text = "IDR";
            this.IDR.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 66);
            this.label1.TabIndex = 4;
            this.label1.Text = "Please select the reason why the Collection Suspension Forbearance is being reque" +
    "sted.";
            // 
            // Continue
            // 
            this.Continue.Location = new System.Drawing.Point(73, 263);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(100, 41);
            this.Continue.TabIndex = 5;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(179, 263);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 41);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // CollectionSuspenseReason
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 327);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IDR);
            this.Controls.Add(this.LoanCon);
            this.Controls.Add(this.Forb);
            this.Controls.Add(this.Def);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(310, 375);
            this.MinimumSize = new System.Drawing.Size(307, 366);
            this.Name = "CollectionSuspenseReason";
            this.ShowIcon = false;
            this.Text = "Select the Reasons for Forbearance";
            this.Load += new System.EventHandler(this.CollectionSuspenseReason_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton Def;
        private System.Windows.Forms.RadioButton Forb;
        private System.Windows.Forms.RadioButton LoanCon;
        private System.Windows.Forms.RadioButton IDR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.Button button1;
    }
}