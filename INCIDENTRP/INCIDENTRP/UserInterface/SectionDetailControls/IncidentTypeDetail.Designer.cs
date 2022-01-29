namespace INCIDENTRP
{
	partial class IncidentTypeDetail
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
            this.pnlOptionContent = new System.Windows.Forms.Panel();
            this.cmbTypeOptions = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pnlOptionContent);
            this.groupBox1.Controls.Add(this.cmbTypeOptions);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 294);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Incident Type";
            // 
            // pnlOptionContent
            // 
            this.pnlOptionContent.Location = new System.Drawing.Point(6, 46);
            this.pnlOptionContent.Name = "pnlOptionContent";
            this.pnlOptionContent.Size = new System.Drawing.Size(441, 245);
            this.pnlOptionContent.TabIndex = 2;
            // 
            // cmbTypeOptions
            // 
            this.cmbTypeOptions.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbTypeOptions.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbTypeOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypeOptions.FormattingEnabled = true;
            this.cmbTypeOptions.Location = new System.Drawing.Point(141, 19);
            this.cmbTypeOptions.MaxDropDownItems = 20;
            this.cmbTypeOptions.Name = "cmbTypeOptions";
            this.cmbTypeOptions.Size = new System.Drawing.Size(306, 21);
            this.cmbTypeOptions.TabIndex = 1;
            this.cmbTypeOptions.SelectedIndexChanged += new System.EventHandler(this.cmbTypeOptions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Incident Types";
            // 
            // IncidentTypeDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "IncidentTypeDetail";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel pnlOptionContent;
		private System.Windows.Forms.ComboBox cmbTypeOptions;
		private System.Windows.Forms.Label label1;
	}
}