namespace TRDPRTYRES
{
	partial class AdditionalComments
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
            this.OK = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.borReferenceInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblEnterComments = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.borReferenceInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(441, 209);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(112, 35);
            this.OK.TabIndex = 0;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.borReferenceInfoBindingSource, "AdditionalComments", true));
            this.textBox1.Location = new System.Drawing.Point(18, 55);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.MaxLength = 600;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(534, 128);
            this.textBox1.TabIndex = 1;
            // 
            // borReferenceInfoBindingSource
            // 
            this.borReferenceInfoBindingSource.DataSource = typeof(TRDPRTYRES.BorReferenceInfo);
            // 
            // lblEnterComments
            // 
            this.lblEnterComments.AutoSize = true;
            this.lblEnterComments.Location = new System.Drawing.Point(14, 14);
            this.lblEnterComments.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEnterComments.Name = "lblEnterComments";
            this.lblEnterComments.Size = new System.Drawing.Size(247, 20);
            this.lblEnterComments.TabIndex = 2;
            this.lblEnterComments.Text = "Enter additional comments below:";
            // 
            // AdditionalComments
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 258);
            this.Controls.Add(this.lblEnterComments);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.OK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(592, 301);
            this.Name = "AdditionalComments";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.borReferenceInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label lblEnterComments;
		private System.Windows.Forms.BindingSource borReferenceInfoBindingSource;
	}
}