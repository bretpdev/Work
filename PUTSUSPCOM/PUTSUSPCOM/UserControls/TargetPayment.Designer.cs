namespace PUTSUSPCOM
{
    partial class TargetPayment
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.targetPaymentOptionProcessorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.optionProcessorBaseBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetPaymentOptionProcessorBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(172, 61);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(3, 61);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, -4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 33);
            this.label1.TabIndex = 34;
            this.label1.Text = "Loan Sequence #s (Comma Delimited For Multiple)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.targetPaymentOptionProcessorBindingSource, "LoanSequenceNumbers", true));
            this.textBox1.Location = new System.Drawing.Point(172, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(660, 20);
            this.textBox1.TabIndex = 35;
            // 
            // targetPaymentOptionProcessorBindingSource
            // 
            this.targetPaymentOptionProcessorBindingSource.DataSource = typeof(PUTSUSPCOM.TargetPaymentOptionProcessor);
            // 
            // txtAmount
            // 
            this.txtAmount.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.targetPaymentOptionProcessorBindingSource, "Amount", true));
            this.txtAmount.Location = new System.Drawing.Point(172, 33);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(96, 20);
            this.txtAmount.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 33);
            this.label2.TabIndex = 36;
            this.label2.Text = "Amount";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(274, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(558, 23);
            this.label3.TabIndex = 38;
            this.label3.Text = "NOTE: When an amount is provided only one loan sequence number may be entered abo" +
                "ve.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TargetPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "TargetPayment";
            this.Size = new System.Drawing.Size(835, 123);
            this.Load += new System.EventHandler(this.TargetPayment_Load);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.textBox14, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtAmount, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            ((System.ComponentModel.ISupportInitialize)(this.optionProcessorBaseBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetPaymentOptionProcessorBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.BindingSource targetPaymentOptionProcessorBindingSource;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

    }
}
