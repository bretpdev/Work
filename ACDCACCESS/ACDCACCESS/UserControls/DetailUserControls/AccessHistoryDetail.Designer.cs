namespace ACDCAccess
{
    partial class AccessHistoryDetail
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
			this.lblSystem = new System.Windows.Forms.Label();
			this.fullAccessRecordBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.lblKey = new System.Windows.Forms.Label();
			this.lblAddedBy = new System.Windows.Forms.Label();
			this.lblBusinessUnit = new System.Windows.Forms.Label();
			this.lblDateAdded = new System.Windows.Forms.Label();
			this.lblDateRemoved = new System.Windows.Forms.Label();
			this.lblRemovedBy = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.fullAccessRecordBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// lblSystem
			// 
			this.lblSystem.AutoSize = true;
			this.lblSystem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblSystem.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fullAccessRecordBindingSource, "Application", true));
			this.lblSystem.Location = new System.Drawing.Point(160, 0);
			this.lblSystem.MaximumSize = new System.Drawing.Size(111, 18);
			this.lblSystem.MinimumSize = new System.Drawing.Size(111, 18);
			this.lblSystem.Name = "lblSystem";
			this.lblSystem.Size = new System.Drawing.Size(111, 18);
			this.lblSystem.TabIndex = 5;
			this.lblSystem.Text = "Application";
			this.lblSystem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// fullAccessRecordBindingSource
			// 
			this.fullAccessRecordBindingSource.DataSource = typeof(ACDCAccess.FullAccessRecord);
			// 
			// lblKey
			// 
			this.lblKey.AutoSize = true;
			this.lblKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fullAccessRecordBindingSource, "UserKey", true));
			this.lblKey.Location = new System.Drawing.Point(3, 0);
			this.lblKey.MaximumSize = new System.Drawing.Size(151, 18);
			this.lblKey.MinimumSize = new System.Drawing.Size(151, 18);
			this.lblKey.Name = "lblKey";
			this.lblKey.Size = new System.Drawing.Size(151, 18);
			this.lblKey.TabIndex = 4;
			this.lblKey.Text = "Key";
			this.lblKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblAddedBy
			// 
			this.lblAddedBy.AutoSize = true;
			this.lblAddedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblAddedBy.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fullAccessRecordBindingSource, "AddedByUserName", true));
			this.lblAddedBy.Location = new System.Drawing.Point(394, 0);
			this.lblAddedBy.MaximumSize = new System.Drawing.Size(105, 18);
			this.lblAddedBy.MinimumSize = new System.Drawing.Size(105, 18);
			this.lblAddedBy.Name = "lblAddedBy";
			this.lblAddedBy.Size = new System.Drawing.Size(105, 18);
			this.lblAddedBy.TabIndex = 8;
			this.lblAddedBy.Text = "Added By";
			this.lblAddedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblBusinessUnit
			// 
			this.lblBusinessUnit.AutoSize = true;
			this.lblBusinessUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblBusinessUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fullAccessRecordBindingSource, "BusinessUnit", true));
			this.lblBusinessUnit.Location = new System.Drawing.Point(277, 0);
			this.lblBusinessUnit.MaximumSize = new System.Drawing.Size(111, 2);
			this.lblBusinessUnit.MinimumSize = new System.Drawing.Size(111, 18);
			this.lblBusinessUnit.Name = "lblBusinessUnit";
			this.lblBusinessUnit.Size = new System.Drawing.Size(111, 18);
			this.lblBusinessUnit.TabIndex = 6;
			this.lblBusinessUnit.Text = "Business Unit";
			this.lblBusinessUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblDateAdded
			// 
			this.lblDateAdded.AutoSize = true;
			this.lblDateAdded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblDateAdded.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fullAccessRecordBindingSource, "StartDate", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "d"));
			this.lblDateAdded.Location = new System.Drawing.Point(505, 0);
			this.lblDateAdded.MaximumSize = new System.Drawing.Size(105, 18);
			this.lblDateAdded.MinimumSize = new System.Drawing.Size(105, 18);
			this.lblDateAdded.Name = "lblDateAdded";
			this.lblDateAdded.Size = new System.Drawing.Size(105, 18);
			this.lblDateAdded.TabIndex = 9;
			this.lblDateAdded.Text = "Date Added";
			this.lblDateAdded.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblDateRemoved
			// 
			this.lblDateRemoved.AutoSize = true;
			this.lblDateRemoved.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblDateRemoved.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fullAccessRecordBindingSource, "EndDate", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "d"));
			this.lblDateRemoved.Location = new System.Drawing.Point(727, 0);
			this.lblDateRemoved.MaximumSize = new System.Drawing.Size(105, 18);
			this.lblDateRemoved.MinimumSize = new System.Drawing.Size(105, 18);
			this.lblDateRemoved.Name = "lblDateRemoved";
			this.lblDateRemoved.Size = new System.Drawing.Size(105, 18);
			this.lblDateRemoved.TabIndex = 11;
			this.lblDateRemoved.Text = "Date Removed";
			this.lblDateRemoved.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblRemovedBy
			// 
			this.lblRemovedBy.AutoSize = true;
			this.lblRemovedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblRemovedBy.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.fullAccessRecordBindingSource, "RemovedByUserName", true));
			this.lblRemovedBy.Location = new System.Drawing.Point(616, 0);
			this.lblRemovedBy.MaximumSize = new System.Drawing.Size(105, 18);
			this.lblRemovedBy.MinimumSize = new System.Drawing.Size(105, 18);
			this.lblRemovedBy.Name = "lblRemovedBy";
			this.lblRemovedBy.Size = new System.Drawing.Size(105, 18);
			this.lblRemovedBy.TabIndex = 10;
			this.lblRemovedBy.Text = "Removed By";
			this.lblRemovedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AccessHistoryDetail
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.lblDateRemoved);
			this.Controls.Add(this.lblRemovedBy);
			this.Controls.Add(this.lblDateAdded);
			this.Controls.Add(this.lblAddedBy);
			this.Controls.Add(this.lblBusinessUnit);
			this.Controls.Add(this.lblSystem);
			this.Controls.Add(this.lblKey);
			this.MaximumSize = new System.Drawing.Size(850, 0);
			this.MinimumSize = new System.Drawing.Size(850, 20);
			this.Name = "AccessHistoryDetail";
			this.Size = new System.Drawing.Size(850, 20);
			((System.ComponentModel.ISupportInitialize)(this.fullAccessRecordBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label lblSystem;
        protected System.Windows.Forms.Label lblKey;
        protected System.Windows.Forms.Label lblAddedBy;
        protected System.Windows.Forms.Label lblBusinessUnit;
        protected System.Windows.Forms.Label lblDateAdded;
        protected System.Windows.Forms.Label lblDateRemoved;
        protected System.Windows.Forms.Label lblRemovedBy;
        private System.Windows.Forms.BindingSource fullAccessRecordBindingSource;
    }
}
