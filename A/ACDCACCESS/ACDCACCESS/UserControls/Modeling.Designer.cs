namespace ACDCAccess
{
    partial class Modeling
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.dgvModelsAccess = new System.Windows.Forms.DataGridView();
			this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.applicationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.businessUnitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.userIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.legalNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.userAccessKeyBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.btnPerformModeling = new System.Windows.Forms.Button();
			this.cmbUserToModelAfter = new Q.ComboBoxWithAutoCompleteExtended();
			this.cmbUserToChangeInModeling = new Q.ComboBoxWithAutoCompleteExtended();
			this.modelAfterKeyBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dgvChangesToBeMade = new System.Windows.Forms.DataGridView();
			this.actionToBeTakenDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.userKeyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.applicationDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.businessUnitDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.calculatedIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgvModelsAccess)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.userAccessKeyBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.modelAfterKeyBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvChangesToBeMade)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(98, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "User to Model After";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 283);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(138, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "User to Change in Modeling";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(81, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Model\'s Access";
			// 
			// dgvModelsAccess
			// 
			this.dgvModelsAccess.AllowUserToAddRows = false;
			this.dgvModelsAccess.AllowUserToDeleteRows = false;
			this.dgvModelsAccess.AutoGenerateColumns = false;
			this.dgvModelsAccess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvModelsAccess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.applicationDataGridViewTextBoxColumn,
            this.businessUnitDataGridViewTextBoxColumn,
            this.typeDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.userIDDataGridViewTextBoxColumn,
            this.iDDataGridViewTextBoxColumn,
            this.legalNameDataGridViewTextBoxColumn});
			this.dgvModelsAccess.DataSource = this.userAccessKeyBindingSource;
			this.dgvModelsAccess.Location = new System.Drawing.Point(154, 32);
			this.dgvModelsAccess.Name = "dgvModelsAccess";
			this.dgvModelsAccess.ReadOnly = true;
			this.dgvModelsAccess.RowHeadersVisible = false;
			this.dgvModelsAccess.Size = new System.Drawing.Size(860, 241);
			this.dgvModelsAccess.TabIndex = 0;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			this.nameDataGridViewTextBoxColumn.HeaderText = "Key";
			this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			this.nameDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// applicationDataGridViewTextBoxColumn
			// 
			this.applicationDataGridViewTextBoxColumn.DataPropertyName = "Application";
			this.applicationDataGridViewTextBoxColumn.HeaderText = "Application";
			this.applicationDataGridViewTextBoxColumn.Name = "applicationDataGridViewTextBoxColumn";
			this.applicationDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// businessUnitDataGridViewTextBoxColumn
			// 
			this.businessUnitDataGridViewTextBoxColumn.DataPropertyName = "BusinessUnit";
			this.businessUnitDataGridViewTextBoxColumn.HeaderText = "Business Unit";
			this.businessUnitDataGridViewTextBoxColumn.Name = "businessUnitDataGridViewTextBoxColumn";
			this.businessUnitDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// typeDataGridViewTextBoxColumn
			// 
			this.typeDataGridViewTextBoxColumn.DataPropertyName = "Type";
			this.typeDataGridViewTextBoxColumn.HeaderText = "Type";
			this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
			this.typeDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// descriptionDataGridViewTextBoxColumn
			// 
			this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
			this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
			this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
			this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// userIDDataGridViewTextBoxColumn
			// 
			this.userIDDataGridViewTextBoxColumn.DataPropertyName = "UserID";
			this.userIDDataGridViewTextBoxColumn.HeaderText = "UserID";
			this.userIDDataGridViewTextBoxColumn.Name = "userIDDataGridViewTextBoxColumn";
			this.userIDDataGridViewTextBoxColumn.ReadOnly = true;
			this.userIDDataGridViewTextBoxColumn.Visible = false;
			// 
			// iDDataGridViewTextBoxColumn
			// 
			this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
			this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
			this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
			this.iDDataGridViewTextBoxColumn.ReadOnly = true;
			this.iDDataGridViewTextBoxColumn.Visible = false;
			// 
			// legalNameDataGridViewTextBoxColumn
			// 
			this.legalNameDataGridViewTextBoxColumn.DataPropertyName = "LegalName";
			this.legalNameDataGridViewTextBoxColumn.HeaderText = "LegalName";
			this.legalNameDataGridViewTextBoxColumn.Name = "legalNameDataGridViewTextBoxColumn";
			this.legalNameDataGridViewTextBoxColumn.ReadOnly = true;
			this.legalNameDataGridViewTextBoxColumn.Visible = false;
			// 
			// userAccessKeyBindingSource
			// 
			this.userAccessKeyBindingSource.DataSource = typeof(ACDCAccess.UserAccessKey);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 316);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(106, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Changes to be Made";
			// 
			// btnPerformModeling
			// 
			this.btnPerformModeling.Location = new System.Drawing.Point(438, 558);
			this.btnPerformModeling.Name = "btnPerformModeling";
			this.btnPerformModeling.Size = new System.Drawing.Size(151, 23);
			this.btnPerformModeling.TabIndex = 10;
			this.btnPerformModeling.Text = "Perform Access Modeling";
			this.btnPerformModeling.UseVisualStyleBackColor = true;
			this.btnPerformModeling.Click += new System.EventHandler(this.btnPerformModeling_Click);
			// 
			// cmbUserToModelAfter
			// 
			this.cmbUserToModelAfter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbUserToModelAfter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbUserToModelAfter.FormattingEnabled = true;
			this.cmbUserToModelAfter.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbUserToModelAfter.Location = new System.Drawing.Point(159, 6);
			this.cmbUserToModelAfter.Name = "cmbUserToModelAfter";
			this.cmbUserToModelAfter.Size = new System.Drawing.Size(325, 21);
			this.cmbUserToModelAfter.TabIndex = 1;
			this.cmbUserToModelAfter.SelectedIndexChanged += new System.EventHandler(this.cmbUserToModelAfter_SelectedIndexChanged);
			// 
			// cmbUserToChangeInModeling
			// 
			this.cmbUserToChangeInModeling.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbUserToChangeInModeling.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbUserToChangeInModeling.FormattingEnabled = true;
			this.cmbUserToChangeInModeling.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbUserToChangeInModeling.Location = new System.Drawing.Point(159, 280);
			this.cmbUserToChangeInModeling.Name = "cmbUserToChangeInModeling";
			this.cmbUserToChangeInModeling.Size = new System.Drawing.Size(325, 21);
			this.cmbUserToChangeInModeling.TabIndex = 7;
			this.cmbUserToChangeInModeling.SelectedIndexChanged += new System.EventHandler(this.cmbUserChangedInModeling_SelectedIndexChanged);
			// 
			// modelAfterKeyBindingSource
			// 
			this.modelAfterKeyBindingSource.DataSource = typeof(ACDCAccess.ModelAfterKey);
			// 
			// dgvChangesToBeMade
			// 
			this.dgvChangesToBeMade.AllowUserToAddRows = false;
			this.dgvChangesToBeMade.AllowUserToDeleteRows = false;
			this.dgvChangesToBeMade.AutoGenerateColumns = false;
			this.dgvChangesToBeMade.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvChangesToBeMade.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.actionToBeTakenDataGridViewTextBoxColumn,
            this.userKeyDataGridViewTextBoxColumn,
            this.applicationDataGridViewTextBoxColumn1,
            this.businessUnitDataGridViewTextBoxColumn1,
            this.calculatedIDDataGridViewTextBoxColumn});
			this.dgvChangesToBeMade.DataSource = this.modelAfterKeyBindingSource;
			this.dgvChangesToBeMade.Location = new System.Drawing.Point(154, 309);
			this.dgvChangesToBeMade.Name = "dgvChangesToBeMade";
			this.dgvChangesToBeMade.ReadOnly = true;
			this.dgvChangesToBeMade.RowHeadersVisible = false;
			this.dgvChangesToBeMade.Size = new System.Drawing.Size(860, 241);
			this.dgvChangesToBeMade.TabIndex = 11;
			// 
			// actionToBeTakenDataGridViewTextBoxColumn
			// 
			this.actionToBeTakenDataGridViewTextBoxColumn.DataPropertyName = "ActionToBeTaken";
			this.actionToBeTakenDataGridViewTextBoxColumn.HeaderText = "Action to Take";
			this.actionToBeTakenDataGridViewTextBoxColumn.Name = "actionToBeTakenDataGridViewTextBoxColumn";
			this.actionToBeTakenDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// userKeyDataGridViewTextBoxColumn
			// 
			this.userKeyDataGridViewTextBoxColumn.DataPropertyName = "UserKey";
			this.userKeyDataGridViewTextBoxColumn.HeaderText = "Key";
			this.userKeyDataGridViewTextBoxColumn.Name = "userKeyDataGridViewTextBoxColumn";
			this.userKeyDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// applicationDataGridViewTextBoxColumn1
			// 
			this.applicationDataGridViewTextBoxColumn1.DataPropertyName = "Application";
			this.applicationDataGridViewTextBoxColumn1.HeaderText = "Application";
			this.applicationDataGridViewTextBoxColumn1.Name = "applicationDataGridViewTextBoxColumn1";
			this.applicationDataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// businessUnitDataGridViewTextBoxColumn1
			// 
			this.businessUnitDataGridViewTextBoxColumn1.DataPropertyName = "BusinessUnit";
			this.businessUnitDataGridViewTextBoxColumn1.HeaderText = "Business Unit";
			this.businessUnitDataGridViewTextBoxColumn1.Name = "businessUnitDataGridViewTextBoxColumn1";
			this.businessUnitDataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// calculatedIDDataGridViewTextBoxColumn
			// 
			this.calculatedIDDataGridViewTextBoxColumn.DataPropertyName = "CalculatedID";
			this.calculatedIDDataGridViewTextBoxColumn.HeaderText = "CalculatedID";
			this.calculatedIDDataGridViewTextBoxColumn.Name = "calculatedIDDataGridViewTextBoxColumn";
			this.calculatedIDDataGridViewTextBoxColumn.ReadOnly = true;
			this.calculatedIDDataGridViewTextBoxColumn.Visible = false;
			// 
			// Modeling
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dgvChangesToBeMade);
			this.Controls.Add(this.dgvModelsAccess);
			this.Controls.Add(this.cmbUserToChangeInModeling);
			this.Controls.Add(this.cmbUserToModelAfter);
			this.Controls.Add(this.btnPerformModeling);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Modeling";
			((System.ComponentModel.ISupportInitialize)(this.dgvModelsAccess)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.userAccessKeyBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.modelAfterKeyBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvChangesToBeMade)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPerformModeling;
        private Q.ComboBoxWithAutoCompleteExtended cmbUserToModelAfter;
        private Q.ComboBoxWithAutoCompleteExtended cmbUserToChangeInModeling;
		private System.Windows.Forms.DataGridView dgvModelsAccess;
		private System.Windows.Forms.BindingSource userAccessKeyBindingSource;
		private System.Windows.Forms.BindingSource modelAfterKeyBindingSource;
		private System.Windows.Forms.DataGridView dgvChangesToBeMade;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn applicationDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn businessUnitDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn userIDDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn legalNameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn actionToBeTakenDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn userKeyDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn applicationDataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn businessUnitDataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn calculatedIDDataGridViewTextBoxColumn;
    }
}
