using Uheaa.Common.WinForms;

namespace LNSLSSNFED
{
    partial class UserInput
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
            this.pslf = new Uheaa.Common.WinForms.CheckButton();
            this.split = new Uheaa.Common.WinForms.CheckButton();
            this.goButton = new Uheaa.Common.WinForms.ValidationButton();
            this.accountNo = new Uheaa.Common.WinForms.RequiredTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnc = new System.Windows.Forms.RadioButton();
            this.dlo = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(228, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sale ID";
            // 
            // pslf
            // 
            this.pslf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pslf.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pslf.IsChecked = false;
            this.pslf.Location = new System.Drawing.Point(308, 70);
            this.pslf.Name = "pslf";
            this.pslf.Size = new System.Drawing.Size(158, 31);
            this.pslf.TabIndex = 6;
            this.pslf.Text = "PSLF";
            this.pslf.UseVisualStyleBackColor = true;
            this.pslf.Click += new System.EventHandler(this.pslf_Click);
            // 
            // split
            // 
            this.split.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.split.IsChecked = false;
            this.split.Location = new System.Drawing.Point(56, 70);
            this.split.Name = "split";
            this.split.Size = new System.Drawing.Size(158, 31);
            this.split.TabIndex = 5;
            this.split.Text = "Split Loan";
            this.split.UseVisualStyleBackColor = true;
            this.split.Click += new System.EventHandler(this.split_Click);
            // 
            // goButton
            // 
            this.goButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.goButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.goButton.Location = new System.Drawing.Point(126, 213);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(262, 43);
            this.goButton.TabIndex = 4;
            this.goButton.Text = "OK";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // accountNo
            // 
            this.accountNo.Location = new System.Drawing.Point(174, 12);
            this.accountNo.Name = "accountNo";
            this.accountNo.Size = new System.Drawing.Size(162, 20);
            this.accountNo.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lnc);
            this.groupBox1.Controls.Add(this.dlo);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(88, 120);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 54);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Portfolio";
            // 
            // lnc
            // 
            this.lnc.AutoSize = true;
            this.lnc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnc.Location = new System.Drawing.Point(220, 22);
            this.lnc.Name = "lnc";
            this.lnc.Size = new System.Drawing.Size(58, 24);
            this.lnc.TabIndex = 1;
            this.lnc.TabStop = true;
            this.lnc.Text = "LNC";
            this.lnc.UseVisualStyleBackColor = true;
            // 
            // dlo
            // 
            this.dlo.AutoSize = true;
            this.dlo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlo.Location = new System.Drawing.Point(66, 22);
            this.dlo.Name = "dlo";
            this.dlo.Size = new System.Drawing.Size(60, 24);
            this.dlo.TabIndex = 0;
            this.dlo.TabStop = true;
            this.dlo.Text = "DLO";
            this.dlo.UseVisualStyleBackColor = true;
            // 
            // UserInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 286);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pslf);
            this.Controls.Add(this.split);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.accountNo);
            this.MinimumSize = new System.Drawing.Size(500, 200);
            this.Name = "UserInput";
            this.Text = "UserInput";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.RequiredTextBox accountNo;
        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.ValidationButton goButton;
        private Uheaa.Common.WinForms.CheckButton split;
        private Uheaa.Common.WinForms.CheckButton pslf;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton lnc;
        private System.Windows.Forms.RadioButton dlo;
    }
}