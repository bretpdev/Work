namespace ACHSETUPFD
{
	partial class PaymentDueDate
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
            this.components = new System.ComponentModel.Container();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCannotDetermine = new System.Windows.Forms.Button();
            this.txtDueDate = new System.Windows.Forms.TextBox();
            this.nextPaymentDueDateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nextPaymentDueDateBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(308, 63);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(112, 35);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCannotDetermine
            // 
            this.btnCannotDetermine.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCannotDetermine.Location = new System.Drawing.Point(18, 63);
            this.btnCannotDetermine.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCannotDetermine.Name = "btnCannotDetermine";
            this.btnCannotDetermine.Size = new System.Drawing.Size(214, 35);
            this.btnCannotDetermine.TabIndex = 1;
            this.btnCannotDetermine.Text = "Cannot Be Determined";
            this.btnCannotDetermine.UseVisualStyleBackColor = true;
            // 
            // txtDueDate
            // 
            this.txtDueDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.nextPaymentDueDateBindingSource, "PaymentDueDate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtDueDate.Location = new System.Drawing.Point(303, 23);
            this.txtDueDate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDueDate.Name = "txtDueDate";
            this.txtDueDate.Size = new System.Drawing.Size(148, 26);
            this.txtDueDate.TabIndex = 2;
            // 
            // nextPaymentDueDateBindingSource
            // 
            this.nextPaymentDueDateBindingSource.DataSource = typeof(ACHSETUPFD.NextPaymentDueDate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Next Payment Due Date(MM/DD/YYYY)";
            // 
            // PaymentDueDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 117);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDueDate);
            this.Controls.Add(this.btnCannotDetermine);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(465, 160);
            this.MinimumSize = new System.Drawing.Size(464, 156);
            this.Name = "PaymentDueDate";
            this.Text = "Next Payment Due Date";
            ((System.ComponentModel.ISupportInitialize)(this.nextPaymentDueDateBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCannotDetermine;
		private System.Windows.Forms.TextBox txtDueDate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.BindingSource nextPaymentDueDateBindingSource;
	}
}