
namespace DeskAudits
{
    partial class AuditSearch
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
            this.SearchAuditsButton = new System.Windows.Forms.Button();
            this.BeginDateLabel = new System.Windows.Forms.Label();
            this.EndDateLabel = new System.Windows.Forms.Label();
            this.BeginDateField = new System.Windows.Forms.DateTimePicker();
            this.EndDateField = new System.Windows.Forms.DateTimePicker();
            this.AuditsView = new System.Windows.Forms.DataGridView();
            this.ClearFormButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.SharedFieldsControl = new DeskAudits.SharedFields();
            ((System.ComponentModel.ISupportInitialize)(this.AuditsView)).BeginInit();
            this.SuspendLayout();
            // 
            // SearchAuditsButton
            // 
            this.SearchAuditsButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.SearchAuditsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.SearchAuditsButton.Location = new System.Drawing.Point(46, 277);
            this.SearchAuditsButton.Name = "SearchAuditsButton";
            this.SearchAuditsButton.Size = new System.Drawing.Size(167, 52);
            this.SearchAuditsButton.TabIndex = 9;
            this.SearchAuditsButton.Text = "Search Audits";
            this.SearchAuditsButton.UseVisualStyleBackColor = false;
            this.SearchAuditsButton.Click += new System.EventHandler(this.SearchAuditsButton_Click);
            // 
            // BeginDateLabel
            // 
            this.BeginDateLabel.AutoSize = true;
            this.BeginDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.BeginDateLabel.Location = new System.Drawing.Point(57, 175);
            this.BeginDateLabel.Name = "BeginDateLabel";
            this.BeginDateLabel.Size = new System.Drawing.Size(102, 24);
            this.BeginDateLabel.TabIndex = 7;
            this.BeginDateLabel.Text = "Begin Date";
            // 
            // EndDateLabel
            // 
            this.EndDateLabel.AutoSize = true;
            this.EndDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.EndDateLabel.Location = new System.Drawing.Point(309, 174);
            this.EndDateLabel.Name = "EndDateLabel";
            this.EndDateLabel.Size = new System.Drawing.Size(88, 24);
            this.EndDateLabel.TabIndex = 10;
            this.EndDateLabel.Text = "End Date";
            // 
            // BeginDateField
            // 
            this.BeginDateField.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.BeginDateField.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.BeginDateField.CustomFormat = "MM/dd/yyyy";
            this.BeginDateField.Location = new System.Drawing.Point(18, 207);
            this.BeginDateField.Name = "BeginDateField";
            this.BeginDateField.Size = new System.Drawing.Size(195, 30);
            this.BeginDateField.TabIndex = 11;
            // 
            // EndDateField
            // 
            this.EndDateField.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.EndDateField.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateField.CustomFormat = "MM/dd/yyyy";
            this.EndDateField.Location = new System.Drawing.Point(248, 207);
            this.EndDateField.Name = "EndDateField";
            this.EndDateField.Size = new System.Drawing.Size(217, 30);
            this.EndDateField.TabIndex = 12;
            // 
            // AuditsView
            // 
            this.AuditsView.AllowUserToAddRows = false;
            this.AuditsView.AllowUserToDeleteRows = false;
            this.AuditsView.AllowUserToOrderColumns = true;
            this.AuditsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AuditsView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AuditsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AuditsView.Location = new System.Drawing.Point(475, 0);
            this.AuditsView.Name = "AuditsView";
            this.AuditsView.ReadOnly = true;
            this.AuditsView.Size = new System.Drawing.Size(444, 398);
            this.AuditsView.TabIndex = 13;
            this.AuditsView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.AuditsView_ColumnHeaderMouseClick);
            // 
            // ClearFormButton
            // 
            this.ClearFormButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClearFormButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.ClearFormButton.Location = new System.Drawing.Point(248, 277);
            this.ClearFormButton.Name = "ClearFormButton";
            this.ClearFormButton.Size = new System.Drawing.Size(167, 52);
            this.ClearFormButton.TabIndex = 14;
            this.ClearFormButton.Text = "Clear Form";
            this.ClearFormButton.UseVisualStyleBackColor = false;
            this.ClearFormButton.Click += new System.EventHandler(this.ClearFormButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ExportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.ExportButton.Location = new System.Drawing.Point(145, 367);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(167, 52);
            this.ExportButton.TabIndex = 15;
            this.ExportButton.Text = "Export Data";
            this.ExportButton.UseVisualStyleBackColor = false;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // SharedFieldsControl
            // 
            this.SharedFieldsControl.AutoSize = true;
            this.SharedFieldsControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SharedFieldsControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SharedFieldsControl.Location = new System.Drawing.Point(5, 13);
            this.SharedFieldsControl.Name = "SharedFieldsControl";
            this.SharedFieldsControl.Size = new System.Drawing.Size(464, 145);
            this.SharedFieldsControl.TabIndex = 0;
            // 
            // AuditSearch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.AuditsView);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.ClearFormButton);
            this.Controls.Add(this.EndDateField);
            this.Controls.Add(this.BeginDateField);
            this.Controls.Add(this.EndDateLabel);
            this.Controls.Add(this.SearchAuditsButton);
            this.Controls.Add(this.BeginDateLabel);
            this.Controls.Add(this.SharedFieldsControl);
            this.Name = "AuditSearch";
            this.Size = new System.Drawing.Size(936, 422);
            ((System.ComponentModel.ISupportInitialize)(this.AuditsView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public SharedFields SharedFieldsControl;
        private System.Windows.Forms.Button SearchAuditsButton;
        private System.Windows.Forms.Label BeginDateLabel;
        private System.Windows.Forms.Label EndDateLabel;
        private System.Windows.Forms.DateTimePicker BeginDateField;
        private System.Windows.Forms.DateTimePicker EndDateField;
        private System.Windows.Forms.DataGridView AuditsView;
        private System.Windows.Forms.Button ClearFormButton;
        private System.Windows.Forms.Button ExportButton;
    }
}
