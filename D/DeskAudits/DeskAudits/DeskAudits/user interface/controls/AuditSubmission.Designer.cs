
using System;

namespace DeskAudits
{
    partial class AuditSubmission
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
            this.AuditDateLabel = new System.Windows.Forms.Label();
            this.OtherReasonField = new System.Windows.Forms.TextBox();
            this.SubmitAuditButton = new System.Windows.Forms.Button();
            this.AuditDatePicker = new System.Windows.Forms.DateTimePicker();
            this.CustomReasonLabel = new System.Windows.Forms.Label();
            this.SharedFieldsControl = new DeskAudits.SharedFields();
            this.ClearFormButton = new System.Windows.Forms.Button();
            this.AuditTimePicker = new System.Windows.Forms.DateTimePicker();
            this.AuditTimeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AuditDateLabel
            // 
            this.AuditDateLabel.AutoSize = true;
            this.AuditDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.AuditDateLabel.Location = new System.Drawing.Point(64, 258);
            this.AuditDateLabel.Name = "AuditDateLabel";
            this.AuditDateLabel.Size = new System.Drawing.Size(96, 24);
            this.AuditDateLabel.TabIndex = 2;
            this.AuditDateLabel.Text = "Audit Date";
            // 
            // OtherReasonField
            // 
            this.OtherReasonField.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.5F);
            this.OtherReasonField.Location = new System.Drawing.Point(16, 207);
            this.OtherReasonField.MaxLength = 2000;
            this.OtherReasonField.Name = "OtherReasonField";
            this.OtherReasonField.Size = new System.Drawing.Size(450, 28);
            this.OtherReasonField.TabIndex = 3;
            // 
            // SubmitAuditButton
            // 
            this.SubmitAuditButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.SubmitAuditButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.SubmitAuditButton.Location = new System.Drawing.Point(49, 343);
            this.SubmitAuditButton.Name = "SubmitAuditButton";
            this.SubmitAuditButton.Size = new System.Drawing.Size(167, 52);
            this.SubmitAuditButton.TabIndex = 5;
            this.SubmitAuditButton.Text = "Submit Audit";
            this.SubmitAuditButton.UseVisualStyleBackColor = false;
            this.SubmitAuditButton.Click += new System.EventHandler(this.SubmitAuditButton_Click);
            // 
            // AuditDatePicker
            // 
            this.AuditDatePicker.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.AuditDatePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.AuditDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.AuditDatePicker.CustomFormat = "MM/dd/yyyy";
            this.AuditDatePicker.Location = new System.Drawing.Point(21, 285);
            this.AuditDatePicker.Name = "AuditDatePicker";
            this.AuditDatePicker.Size = new System.Drawing.Size(195, 30);
            this.AuditDatePicker.TabIndex = 12;
            // 
            // CustomReasonLabel
            // 
            this.CustomReasonLabel.AutoSize = true;
            this.CustomReasonLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.CustomReasonLabel.Location = new System.Drawing.Point(170, 180);
            this.CustomReasonLabel.Name = "CustomReasonLabel";
            this.CustomReasonLabel.Size = new System.Drawing.Size(127, 24);
            this.CustomReasonLabel.TabIndex = 13;
            this.CustomReasonLabel.Text = "Other Reason";
            // 
            // SharedFieldsControl
            // 
            this.SharedFieldsControl.AutoSize = true;
            this.SharedFieldsControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SharedFieldsControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SharedFieldsControl.Location = new System.Drawing.Point(9, 15);
            this.SharedFieldsControl.Name = "SharedFieldsControl";
            this.SharedFieldsControl.Size = new System.Drawing.Size(464, 145);
            this.SharedFieldsControl.TabIndex = 4;
            // 
            // ClearFormButton
            // 
            this.ClearFormButton.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClearFormButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.ClearFormButton.Location = new System.Drawing.Point(254, 343);
            this.ClearFormButton.Name = "ClearFormButton";
            this.ClearFormButton.Size = new System.Drawing.Size(167, 52);
            this.ClearFormButton.TabIndex = 15;
            this.ClearFormButton.Text = "Clear Form";
            this.ClearFormButton.UseVisualStyleBackColor = false;
            this.ClearFormButton.Click += new System.EventHandler(this.ClearFormButton_Click);
            // 
            // AuditTimePicker
            // 
            this.AuditTimePicker.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.AuditTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.AuditTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.AuditTimePicker.Location = new System.Drawing.Point(254, 285);
            this.AuditTimePicker.Name = "AuditTimePicker";
            this.AuditTimePicker.ShowUpDown = true;
            this.AuditTimePicker.Size = new System.Drawing.Size(213, 30);
            this.AuditTimePicker.TabIndex = 17;
            // 
            // AuditTimeLabel
            // 
            this.AuditTimeLabel.AutoSize = true;
            this.AuditTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.AuditTimeLabel.Location = new System.Drawing.Point(301, 258);
            this.AuditTimeLabel.Name = "AuditTimeLabel";
            this.AuditTimeLabel.Size = new System.Drawing.Size(101, 24);
            this.AuditTimeLabel.TabIndex = 16;
            this.AuditTimeLabel.Text = "Audit Time";
            // 
            // AuditSubmission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.AuditTimePicker);
            this.Controls.Add(this.AuditTimeLabel);
            this.Controls.Add(this.ClearFormButton);
            this.Controls.Add(this.CustomReasonLabel);
            this.Controls.Add(this.AuditDatePicker);
            this.Controls.Add(this.SubmitAuditButton);
            this.Controls.Add(this.SharedFieldsControl);
            this.Controls.Add(this.OtherReasonField);
            this.Controls.Add(this.AuditDateLabel);
            this.Name = "AuditSubmission";
            this.Size = new System.Drawing.Size(476, 422);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void OtherReasonChanged(object sender, bool e)
        {
            if (OtherReasonField != null)
            {
                if (e)
                    OtherReasonField.Enabled = true;
                else
                {
                    OtherReasonField.Text = null;
                    OtherReasonField.Enabled = false;
                }

            }
        }

        #endregion
        private System.Windows.Forms.Label AuditDateLabel;
        private System.Windows.Forms.TextBox OtherReasonField;
        public SharedFields SharedFieldsControl;
        private System.Windows.Forms.Button SubmitAuditButton;
        private System.Windows.Forms.DateTimePicker AuditDatePicker;
        private System.Windows.Forms.Label CustomReasonLabel;
        private System.Windows.Forms.Button ClearFormButton;
        private System.Windows.Forms.DateTimePicker AuditTimePicker;
        private System.Windows.Forms.Label AuditTimeLabel;
    }
}
