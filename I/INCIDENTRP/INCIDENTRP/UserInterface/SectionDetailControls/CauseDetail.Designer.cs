namespace INCIDENTRP
{
	partial class CauseDetail
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbCauseOptions = new System.Windows.Forms.ComboBox();
            this.incidentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.pnlOptionContent = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbCauseOptions);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pnlOptionContent);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(454, 294);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cause";
            // 
            // cmbCauseOptions
            // 
            this.cmbCauseOptions.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cmbCauseOptions.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCauseOptions.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.incidentBindingSource, "Cause", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbCauseOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCauseOptions.FormattingEnabled = true;
            this.cmbCauseOptions.Location = new System.Drawing.Point(142, 19);
            this.cmbCauseOptions.Name = "cmbCauseOptions";
            this.cmbCauseOptions.Size = new System.Drawing.Size(306, 21);
            this.cmbCauseOptions.TabIndex = 2;
            this.cmbCauseOptions.SelectedIndexChanged += new System.EventHandler(this.cmbCauseOptions_SelectedIndexChanged);
            // 
            // incidentBindingSource
            // 
            this.incidentBindingSource.DataSource = typeof(INCIDENTRP.Incident);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cause Options";
            // 
            // pnlOptionContent
            // 
            this.pnlOptionContent.Location = new System.Drawing.Point(7, 46);
            this.pnlOptionContent.Name = "pnlOptionContent";
            this.pnlOptionContent.Size = new System.Drawing.Size(441, 245);
            this.pnlOptionContent.TabIndex = 0;
            // 
            // CauseDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "CauseDetail";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.incidentBindingSource)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cmbCauseOptions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlOptionContent;
		private System.Windows.Forms.BindingSource incidentBindingSource;
	}
}