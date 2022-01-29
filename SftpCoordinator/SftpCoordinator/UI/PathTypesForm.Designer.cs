namespace SftpCoordinator
{
    partial class PathTypesForm
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
            Uheaa.Common.WinForms.BehaviorInstallation behaviorInstallation1 = new Uheaa.Common.WinForms.BehaviorInstallation();
            Uheaa.Common.WinForms.BehaviorInstallation behaviorInstallation2 = new Uheaa.Common.WinForms.BehaviorInstallation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathTypesForm));
            this.PathTypesList = new System.Windows.Forms.ListBox();
            this.EditGroup = new System.Windows.Forms.GroupBox();
            this.ViewLabel = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.RootPathText = new Uheaa.Common.WinForms.BehaviorTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NameText = new Uheaa.Common.WinForms.BehaviorTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.PathTypesGroup = new System.Windows.Forms.GroupBox();
            this.EditGroup.SuspendLayout();
            this.PathTypesGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // PathTypesList
            // 
            this.PathTypesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PathTypesList.DisplayMember = "Name";
            this.PathTypesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PathTypesList.FormattingEnabled = true;
            this.PathTypesList.ItemHeight = 20;
            this.PathTypesList.Location = new System.Drawing.Point(9, 23);
            this.PathTypesList.Name = "PathTypesList";
            this.PathTypesList.Size = new System.Drawing.Size(245, 124);
            this.PathTypesList.TabIndex = 29;
            this.PathTypesList.SelectedIndexChanged += new System.EventHandler(this.PathTypesList_SelectedIndexChanged);
            this.PathTypesList.DoubleClick += new System.EventHandler(this.PathTypesList_DoubleClick);
            // 
            // EditGroup
            // 
            this.EditGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditGroup.Controls.Add(this.ViewLabel);
            this.EditGroup.Controls.Add(this.label3);
            this.EditGroup.Controls.Add(this.CancelButton);
            this.EditGroup.Controls.Add(this.SaveButton);
            this.EditGroup.Controls.Add(this.RootPathText);
            this.EditGroup.Controls.Add(this.label2);
            this.EditGroup.Controls.Add(this.NameText);
            this.EditGroup.Controls.Add(this.label1);
            this.EditGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditGroup.Location = new System.Drawing.Point(283, 12);
            this.EditGroup.Name = "EditGroup";
            this.EditGroup.Size = new System.Drawing.Size(288, 199);
            this.EditGroup.TabIndex = 31;
            this.EditGroup.TabStop = false;
            this.EditGroup.Text = "Edit";
            // 
            // ViewLabel
            // 
            this.ViewLabel.ActiveLinkColor = System.Drawing.Color.DarkGray;
            this.ViewLabel.AutoSize = true;
            this.ViewLabel.LinkColor = System.Drawing.Color.DimGray;
            this.ViewLabel.Location = new System.Drawing.Point(5, 124);
            this.ViewLabel.Name = "ViewLabel";
            this.ViewLabel.Size = new System.Drawing.Size(207, 20);
            this.ViewLabel.TabIndex = 39;
            this.ViewLabel.TabStop = true;
            this.ViewLabel.Text = "{0} project files use this path";
            this.ViewLabel.Visible = false;
            this.ViewLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ViewLabel_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 18);
            this.label3.TabIndex = 35;
            this.label3.Text = "Root Path";
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(9, 161);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 32);
            this.CancelButton.TabIndex = 4;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Location = new System.Drawing.Point(190, 161);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(92, 32);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RootPathText
            // 
            this.RootPathText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RootPathText.InstalledBehaviors = behaviorInstallation1;
            this.RootPathText.Location = new System.Drawing.Point(9, 94);
            this.RootPathText.Name = "RootPathText";
            this.RootPathText.Size = new System.Drawing.Size(273, 26);
            this.RootPathText.TabIndex = 2;
            this.RootPathText.Value = "";
            this.RootPathText.ValueChanged += new Uheaa.Common.WinForms.OnValueChanged(this.TextBox_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 18);
            this.label2.TabIndex = 33;
            this.label2.Text = "Root Path";
            // 
            // NameText
            // 
            this.NameText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameText.InstalledBehaviors = behaviorInstallation2;
            this.NameText.Location = new System.Drawing.Point(9, 44);
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(273, 26);
            this.NameText.TabIndex = 1;
            this.NameText.Value = "";
            this.NameText.ValueChanged += new Uheaa.Common.WinForms.OnValueChanged(this.TextBox_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 31;
            this.label1.Text = "Name";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.Location = new System.Drawing.Point(9, 164);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 32);
            this.DeleteButton.TabIndex = 32;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddButton.Location = new System.Drawing.Point(179, 164);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 32);
            this.AddButton.TabIndex = 33;
            this.AddButton.Text = "Add +";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // PathTypesGroup
            // 
            this.PathTypesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PathTypesGroup.Controls.Add(this.PathTypesList);
            this.PathTypesGroup.Controls.Add(this.AddButton);
            this.PathTypesGroup.Controls.Add(this.DeleteButton);
            this.PathTypesGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PathTypesGroup.Location = new System.Drawing.Point(12, 9);
            this.PathTypesGroup.Name = "PathTypesGroup";
            this.PathTypesGroup.Size = new System.Drawing.Size(265, 202);
            this.PathTypesGroup.TabIndex = 34;
            this.PathTypesGroup.TabStop = false;
            this.PathTypesGroup.Text = "Path Types";
            // 
            // PathTypesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 222);
            this.Controls.Add(this.PathTypesGroup);
            this.Controls.Add(this.EditGroup);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PathTypesForm";
            this.Text = "Path Types & Destinations";
            this.EditGroup.ResumeLayout(false);
            this.EditGroup.PerformLayout();
            this.PathTypesGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox PathTypesList;
        private System.Windows.Forms.GroupBox EditGroup;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button SaveButton;
        private Uheaa.Common.WinForms.BehaviorTextBox RootPathText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.GroupBox PathTypesGroup;
        private System.Windows.Forms.LinkLabel ViewLabel;
        private System.Windows.Forms.Label label3;
        private Uheaa.Common.WinForms.BehaviorTextBox NameText;

    }
}