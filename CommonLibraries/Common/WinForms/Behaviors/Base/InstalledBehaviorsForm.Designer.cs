namespace Uheaa.Common.WinForms
{
    partial class InstalledBehaviorsForm
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
            this.BehaviorsList = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelChangeButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BehaviorsList
            // 
            this.BehaviorsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BehaviorsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BehaviorsList.FormattingEnabled = true;
            this.BehaviorsList.Location = new System.Drawing.Point(0, 0);
            this.BehaviorsList.Name = "BehaviorsList";
            this.BehaviorsList.Size = new System.Drawing.Size(315, 224);
            this.BehaviorsList.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OkButton);
            this.panel1.Controls.Add(this.CancelChangeButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 167);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(315, 57);
            this.panel1.TabIndex = 1;
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkButton.Location = new System.Drawing.Point(225, 11);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 34);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelChangeButton
            // 
            this.CancelChangeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelChangeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelChangeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelChangeButton.Location = new System.Drawing.Point(12, 11);
            this.CancelChangeButton.Name = "CancelChangeButton";
            this.CancelChangeButton.Size = new System.Drawing.Size(75, 34);
            this.CancelChangeButton.TabIndex = 0;
            this.CancelChangeButton.Text = "Cancel";
            this.CancelChangeButton.UseVisualStyleBackColor = true;
            // 
            // InstalledBehaviorsForm
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 224);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BehaviorsList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstalledBehaviorsForm";
            this.Text = "Installed Behaviors";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox BehaviorsList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelChangeButton;
    }
}