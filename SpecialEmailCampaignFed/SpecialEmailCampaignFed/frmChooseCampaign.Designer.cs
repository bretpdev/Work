namespace SpecialEmailCampaignFed
{
	partial class frmChooseCampaign
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
			System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
			this.label1 = new System.Windows.Forms.Label();
			this.lvwExsistingCamp = new System.Windows.Forms.ListView();
			this.chCampId = new System.Windows.Forms.ColumnHeader();
			this.chSubjectLine = new System.Windows.Forms.ColumnHeader();
			this.chDataFIle = new System.Windows.Forms.ColumnHeader();
			this.chHTMLFile = new System.Windows.Forms.ColumnHeader();
			this.btnOpen = new System.Windows.Forms.Button();
			this.btnNew = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(10, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(599, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please select an exsisting campaign and click the open button, or click the new b" +
				"utton to create a new camapign";
			// 
			// lvwExsistingCamp
			// 
			this.lvwExsistingCamp.Activation = System.Windows.Forms.ItemActivation.TwoClick;
			this.lvwExsistingCamp.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chCampId,
            this.chSubjectLine,
            this.chDataFIle,
            this.chHTMLFile});
			this.lvwExsistingCamp.FullRowSelect = true;
			listViewGroup1.Header = "ListViewGroup";
			listViewGroup1.Name = "listViewGroup1";
			this.lvwExsistingCamp.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
			this.lvwExsistingCamp.Location = new System.Drawing.Point(3, 37);
			this.lvwExsistingCamp.MultiSelect = false;
			this.lvwExsistingCamp.Name = "lvwExsistingCamp";
			this.lvwExsistingCamp.Size = new System.Drawing.Size(627, 187);
			this.lvwExsistingCamp.TabIndex = 1;
			this.lvwExsistingCamp.UseCompatibleStateImageBehavior = false;
			this.lvwExsistingCamp.View = System.Windows.Forms.View.Details;
			// 
			// chCampId
			// 
			this.chCampId.Text = "Campaign Id";
			this.chCampId.Width = 74;
			// 
			// chSubjectLine
			// 
			this.chSubjectLine.Text = "Subject Line";
			this.chSubjectLine.Width = 216;
			// 
			// chDataFIle
			// 
			this.chDataFIle.Text = "DataFile";
			this.chDataFIle.Width = 162;
			// 
			// chHTMLFile
			// 
			this.chHTMLFile.Text = "HTML File";
			this.chHTMLFile.Width = 167;
			// 
			// btnOpen
			// 
			this.btnOpen.Location = new System.Drawing.Point(242, 231);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(75, 23);
			this.btnOpen.TabIndex = 2;
			this.btnOpen.Text = "Open";
			this.btnOpen.UseVisualStyleBackColor = true;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnNew
			// 
			this.btnNew.Location = new System.Drawing.Point(338, 230);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(75, 23);
			this.btnNew.TabIndex = 3;
			this.btnNew.Text = "New";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// frmChooseCampaign
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 266);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.lvwExsistingCamp);
			this.Controls.Add(this.label1);
			this.Name = "frmChooseCampaign";
			this.Text = "Pick Your Email Campaign";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		internal System.Windows.Forms.ColumnHeader chSubjectLine;
		internal System.Windows.Forms.ColumnHeader chDataFIle;
		internal System.Windows.Forms.ColumnHeader chHTMLFile;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.Button btnNew;
		internal System.Windows.Forms.ListView lvwExsistingCamp;
		private System.Windows.Forms.ColumnHeader chCampId;
	}
}

