namespace INCIDENTRP
{
	partial class DataInvolvedDetail
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbDataInvolvedOptions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlOptionContent = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbDataInvolvedOptions);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pnlOptionContent);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 294);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Involved";
            // 
            // cmbDataInvolvedOptions
            // 
            this.cmbDataInvolvedOptions.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbDataInvolvedOptions.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDataInvolvedOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataInvolvedOptions.FormattingEnabled = true;
            this.cmbDataInvolvedOptions.Location = new System.Drawing.Point(142, 19);
            this.cmbDataInvolvedOptions.Name = "cmbDataInvolvedOptions";
            this.cmbDataInvolvedOptions.Size = new System.Drawing.Size(306, 21);
            this.cmbDataInvolvedOptions.TabIndex = 2;
            this.cmbDataInvolvedOptions.SelectedIndexChanged += new System.EventHandler(this.cmbDataInvolvedOptions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Data Involved Options";
            // 
            // pnlOptionContent
            // 
            this.pnlOptionContent.Location = new System.Drawing.Point(7, 46);
            this.pnlOptionContent.Name = "pnlOptionContent";
            this.pnlOptionContent.Size = new System.Drawing.Size(441, 245);
            this.pnlOptionContent.TabIndex = 0;
            // 
            // DataInvolvedDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "DataInvolvedDetail";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cmbDataInvolvedOptions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlOptionContent;

	}
}