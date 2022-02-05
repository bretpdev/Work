namespace TRDPRTYRES
{
	partial class PossibleMatchRefSelection
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
            this.lblDirections = new System.Windows.Forms.Label();
            this.NewReference = new System.Windows.Forms.Button();
            this.References = new System.Windows.Forms.ListView();
            this.refId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.refName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Continue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDirections
            // 
            this.lblDirections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDirections.Location = new System.Drawing.Point(14, 14);
            this.lblDirections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDirections.Name = "lblDirections";
            this.lblDirections.Size = new System.Drawing.Size(423, 80);
            this.lblDirections.TabIndex = 1;
            this.lblDirections.Text = "The Reference you are trying to add may already be in the System.  Please review " +
    "the Reference(s) below.  If the Reference does not match please select the \"New " +
    "Reference\" button to add the reference.";
            // 
            // NewReference
            // 
            this.NewReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NewReference.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.NewReference.Location = new System.Drawing.Point(291, 417);
            this.NewReference.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NewReference.Name = "NewReference";
            this.NewReference.Size = new System.Drawing.Size(160, 34);
            this.NewReference.TabIndex = 2;
            this.NewReference.Text = "New Reference";
            this.NewReference.UseVisualStyleBackColor = true;
            // 
            // References
            // 
            this.References.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.References.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.References.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.refId,
            this.refName});
            this.References.HideSelection = false;
            this.References.Location = new System.Drawing.Point(18, 118);
            this.References.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.References.MultiSelect = false;
            this.References.Name = "References";
            this.References.Size = new System.Drawing.Size(432, 267);
            this.References.TabIndex = 3;
            this.References.UseCompatibleStateImageBehavior = false;
            this.References.View = System.Windows.Forms.View.Details;
            // 
            // refId
            // 
            this.refId.Text = "Reference Id";
            this.refId.Width = 100;
            // 
            // refName
            // 
            this.refName.Text = "Reference Name";
            this.refName.Width = 185;
            // 
            // Continue
            // 
            this.Continue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Continue.Location = new System.Drawing.Point(18, 417);
            this.Continue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(112, 35);
            this.Continue.TabIndex = 4;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // PossibleMatchRefSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 469);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.References);
            this.Controls.Add(this.NewReference);
            this.Controls.Add(this.lblDirections);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(491, 512);
            this.Name = "PossibleMatchRefSelection";
            this.ShowIcon = false;
            this.Text = "Select Reference";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblDirections;
        private System.Windows.Forms.Button NewReference;
        private System.Windows.Forms.ListView References;
        private System.Windows.Forms.ColumnHeader refId;
        private System.Windows.Forms.ColumnHeader refName;
        private System.Windows.Forms.Button Continue;
	}
}