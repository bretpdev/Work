namespace ApplicationSettings
{
    partial class ApplicationMaintenance
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.ApplicationName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.StartingClass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SourcePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SetSource = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ApplicationsList = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.New = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.toolTipSource = new System.Windows.Forms.ToolTip(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.ArgumentsGroup = new System.Windows.Forms.GroupBox();
            this.AddRemove = new System.Windows.Forms.PictureBox();
            this.Down = new System.Windows.Forms.PictureBox();
            this.Up = new System.Windows.Forms.PictureBox();
            this.Selected = new System.Windows.Forms.ListBox();
            this.label12 = new System.Windows.Forms.Label();
            this.Available = new System.Windows.Forms.ListBox();
            this.label11 = new System.Windows.Forms.Label();
            this.StartingDll = new System.Windows.Forms.TextBox();
            this.ApplicationImage = new System.Windows.Forms.PictureBox();
            this.AccessKey = new System.Windows.Forms.ComboBox();
            this.Delete = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.Upload = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.dllText = new System.Windows.Forms.Label();
            this.TypeHttp = new System.Windows.Forms.RadioButton();
            this.TypeExe = new System.Windows.Forms.RadioButton();
            this.TypeDll = new System.Windows.Forms.RadioButton();
            this.ApplitcationType = new System.Windows.Forms.GroupBox();
            this.TypeMdb = new System.Windows.Forms.RadioButton();
            this.ApplicationText = new System.Windows.Forms.Label();
            this.VersionNumber = new System.Windows.Forms.Label();
            this.ArgumentsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Up)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplicationImage)).BeginInit();
            this.ApplitcationType.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Application Name *";
            // 
            // ApplicationName
            // 
            this.ApplicationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplicationName.Enabled = false;
            this.ApplicationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplicationName.Location = new System.Drawing.Point(30, 151);
            this.ApplicationName.MaxLength = 100;
            this.ApplicationName.Name = "ApplicationName";
            this.ApplicationName.Size = new System.Drawing.Size(366, 26);
            this.ApplicationName.TabIndex = 6;
            this.ApplicationName.TextChanged += new System.EventHandler(this.ApplicationName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(26, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Access Key";
            // 
            // StartingClass
            // 
            this.StartingClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartingClass.Enabled = false;
            this.StartingClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartingClass.Location = new System.Drawing.Point(30, 312);
            this.StartingClass.MaxLength = 50;
            this.StartingClass.Name = "StartingClass";
            this.StartingClass.Size = new System.Drawing.Size(366, 26);
            this.StartingClass.TabIndex = 9;
            this.toolTipSource.SetToolTip(this.StartingClass, "Class that is called to start the application");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 287);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Starting Class";
            // 
            // SourcePath
            // 
            this.SourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SourcePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SourcePath.Location = new System.Drawing.Point(30, 369);
            this.SourcePath.MaxLength = 256;
            this.SourcePath.Name = "SourcePath";
            this.SourcePath.ReadOnly = true;
            this.SourcePath.Size = new System.Drawing.Size(702, 26);
            this.SourcePath.TabIndex = 13;
            this.toolTipSource.SetToolTip(this.SourcePath, "The location of the live application");
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(26, 342);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Source Path Folder *";
            this.toolTipSource.SetToolTip(this.label4, "The folder location of the live application");
            // 
            // SetSource
            // 
            this.SetSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SetSource.Enabled = false;
            this.SetSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetSource.Location = new System.Drawing.Point(749, 362);
            this.SetSource.Name = "SetSource";
            this.SetSource.Size = new System.Drawing.Size(111, 33);
            this.SetSource.TabIndex = 11;
            this.SetSource.Text = "Source";
            this.SetSource.UseVisualStyleBackColor = true;
            this.SetSource.Click += new System.EventHandler(this.SetSource_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(12, 47);
            this.label6.MinimumSize = new System.Drawing.Size(780, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(850, 2);
            this.label6.TabIndex = 12;
            // 
            // ApplicationsList
            // 
            this.ApplicationsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplicationsList.DropDownHeight = 300;
            this.ApplicationsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplicationsList.FormattingEnabled = true;
            this.ApplicationsList.IntegralHeight = false;
            this.ApplicationsList.Location = new System.Drawing.Point(173, 8);
            this.ApplicationsList.Name = "ApplicationsList";
            this.ApplicationsList.Size = new System.Drawing.Size(391, 28);
            this.ApplicationsList.TabIndex = 0;
            this.ApplicationsList.SelectedIndexChanged += new System.EventHandler(this.ApplicationsList_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(26, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Active Applications";
            // 
            // New
            // 
            this.New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.New.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.New.Location = new System.Drawing.Point(575, 8);
            this.New.Name = "New";
            this.New.Size = new System.Drawing.Size(104, 28);
            this.New.TabIndex = 1;
            this.New.Text = "Clear Form";
            this.New.UseVisualStyleBackColor = true;
            this.New.Click += new System.EventHandler(this.New_Click);
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.Enabled = false;
            this.Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Save.Location = new System.Drawing.Point(749, 417);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(111, 28);
            this.Save.TabIndex = 12;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // toolTipSource
            // 
            this.toolTipSource.IsBalloon = true;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(349, 425);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(130, 20);
            this.label10.TabIndex = 24;
            this.label10.Text = "* Reguired Fields";
            this.toolTipSource.SetToolTip(this.label10, "The location of the live application");
            // 
            // ArgumentsGroup
            // 
            this.ArgumentsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgumentsGroup.Controls.Add(this.AddRemove);
            this.ArgumentsGroup.Controls.Add(this.Down);
            this.ArgumentsGroup.Controls.Add(this.Up);
            this.ArgumentsGroup.Controls.Add(this.Selected);
            this.ArgumentsGroup.Controls.Add(this.label12);
            this.ArgumentsGroup.Controls.Add(this.Available);
            this.ArgumentsGroup.Controls.Add(this.label11);
            this.ArgumentsGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArgumentsGroup.Location = new System.Drawing.Point(412, 125);
            this.ArgumentsGroup.Name = "ArgumentsGroup";
            this.ArgumentsGroup.Size = new System.Drawing.Size(320, 224);
            this.ArgumentsGroup.TabIndex = 29;
            this.ArgumentsGroup.TabStop = false;
            this.ArgumentsGroup.Text = "Arguments";
            this.toolTipSource.SetToolTip(this.ArgumentsGroup, "Arguments must be in the order the application will receive them.");
            // 
            // AddRemove
            // 
            this.AddRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.AddRemove.Location = new System.Drawing.Point(133, 120);
            this.AddRemove.Name = "AddRemove";
            this.AddRemove.Size = new System.Drawing.Size(22, 22);
            this.AddRemove.TabIndex = 31;
            this.AddRemove.TabStop = false;
            this.AddRemove.Click += new System.EventHandler(this.AddRemove_Click);
            // 
            // Down
            // 
            this.Down.BackgroundImage = global::ApplicationSettings.Properties.Resources.Down;
            this.Down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Down.Location = new System.Drawing.Point(289, 197);
            this.Down.Name = "Down";
            this.Down.Size = new System.Drawing.Size(22, 22);
            this.Down.TabIndex = 30;
            this.Down.TabStop = false;
            this.Down.Click += new System.EventHandler(this.Down_Click);
            // 
            // Up
            // 
            this.Up.BackColor = System.Drawing.SystemColors.Control;
            this.Up.BackgroundImage = global::ApplicationSettings.Properties.Resources.Up;
            this.Up.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Up.Location = new System.Drawing.Point(289, 53);
            this.Up.Name = "Up";
            this.Up.Size = new System.Drawing.Size(22, 22);
            this.Up.TabIndex = 29;
            this.Up.TabStop = false;
            this.Up.Click += new System.EventHandler(this.Up_Click);
            // 
            // Selected
            // 
            this.Selected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Selected.Enabled = false;
            this.Selected.FormattingEnabled = true;
            this.Selected.ItemHeight = 20;
            this.Selected.Location = new System.Drawing.Point(163, 53);
            this.Selected.Name = "Selected";
            this.Selected.Size = new System.Drawing.Size(120, 164);
            this.Selected.TabIndex = 25;
            this.toolTipSource.SetToolTip(this.Selected, "Arguments must be in the order the application will receive them.");
            this.Selected.Click += new System.EventHandler(this.Selected_Click);
            this.Selected.SelectedIndexChanged += new System.EventHandler(this.Selected_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(6, 29);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 20);
            this.label12.TabIndex = 28;
            this.label12.Text = "Available";
            // 
            // Available
            // 
            this.Available.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Available.Enabled = false;
            this.Available.FormattingEnabled = true;
            this.Available.ItemHeight = 20;
            this.Available.Location = new System.Drawing.Point(7, 53);
            this.Available.Name = "Available";
            this.Available.Size = new System.Drawing.Size(120, 164);
            this.Available.TabIndex = 26;
            this.toolTipSource.SetToolTip(this.Available, "Arguments must be in the order the application will receive them.");
            this.Available.Click += new System.EventHandler(this.Available_Click);
            this.Available.SelectedIndexChanged += new System.EventHandler(this.Available_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(159, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 20);
            this.label11.TabIndex = 27;
            this.label11.Text = "Selected";
            // 
            // StartingDll
            // 
            this.StartingDll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartingDll.Enabled = false;
            this.StartingDll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartingDll.Location = new System.Drawing.Point(30, 259);
            this.StartingDll.MaxLength = 50;
            this.StartingDll.Name = "StartingDll";
            this.StartingDll.Size = new System.Drawing.Size(366, 26);
            this.StartingDll.TabIndex = 8;
            this.toolTipSource.SetToolTip(this.StartingDll, "DLL Name");
            this.StartingDll.Enter += new System.EventHandler(this.StartingDll_Enter);
            this.StartingDll.Leave += new System.EventHandler(this.StartingDll_Leave);
            // 
            // ApplicationImage
            // 
            this.ApplicationImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplicationImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ApplicationImage.Location = new System.Drawing.Point(749, 141);
            this.ApplicationImage.Name = "ApplicationImage";
            this.ApplicationImage.Size = new System.Drawing.Size(52, 52);
            this.ApplicationImage.TabIndex = 31;
            this.ApplicationImage.TabStop = false;
            this.toolTipSource.SetToolTip(this.ApplicationImage, "Can be any size or image type. It will be converted to a 50x50 jpg");
            // 
            // AccessKey
            // 
            this.AccessKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccessKey.DropDownWidth = 500;
            this.AccessKey.Enabled = false;
            this.AccessKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccessKey.FormattingEnabled = true;
            this.AccessKey.Location = new System.Drawing.Point(30, 202);
            this.AccessKey.Name = "AccessKey";
            this.AccessKey.Size = new System.Drawing.Size(366, 28);
            this.AccessKey.TabIndex = 7;
            this.toolTipSource.SetToolTip(this.AccessKey, "Access key must be created before assigning to an application");
            // 
            // Delete
            // 
            this.Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Delete.Enabled = false;
            this.Delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Delete.Location = new System.Drawing.Point(12, 417);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(107, 28);
            this.Delete.TabIndex = 14;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(745, 118);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 20);
            this.label9.TabIndex = 30;
            this.label9.Text = "Icon";
            // 
            // Upload
            // 
            this.Upload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Upload.Enabled = false;
            this.Upload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Upload.Location = new System.Drawing.Point(749, 197);
            this.Upload.Name = "Upload";
            this.Upload.Size = new System.Drawing.Size(111, 33);
            this.Upload.TabIndex = 10;
            this.Upload.Text = "Upload";
            this.Upload.UseVisualStyleBackColor = true;
            this.Upload.Click += new System.EventHandler(this.Upload_Click);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(802, 178);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 33;
            this.label13.Text = "50 x 50 px";
            // 
            // dllText
            // 
            this.dllText.AutoSize = true;
            this.dllText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dllText.Location = new System.Drawing.Point(26, 233);
            this.dllText.Name = "dllText";
            this.dllText.Size = new System.Drawing.Size(99, 20);
            this.dllText.TabIndex = 35;
            this.dllText.Text = "Starting DLL";
            // 
            // TypeHttp
            // 
            this.TypeHttp.AutoSize = true;
            this.TypeHttp.Checked = true;
            this.TypeHttp.Location = new System.Drawing.Point(291, 25);
            this.TypeHttp.Name = "TypeHttp";
            this.TypeHttp.Size = new System.Drawing.Size(67, 24);
            this.TypeHttp.TabIndex = 5;
            this.TypeHttp.TabStop = true;
            this.TypeHttp.Text = "HTTP";
            this.TypeHttp.UseVisualStyleBackColor = true;
            this.TypeHttp.CheckedChanged += new System.EventHandler(this.TypeHttp_CheckedChanged);
            // 
            // TypeExe
            // 
            this.TypeExe.AutoSize = true;
            this.TypeExe.Location = new System.Drawing.Point(98, 25);
            this.TypeExe.Name = "TypeExe";
            this.TypeExe.Size = new System.Drawing.Size(64, 24);
            this.TypeExe.TabIndex = 3;
            this.TypeExe.Text = ".EXE";
            this.TypeExe.UseVisualStyleBackColor = true;
            this.TypeExe.CheckedChanged += new System.EventHandler(this.TypeExe_CheckedChanged);
            // 
            // TypeDll
            // 
            this.TypeDll.AutoSize = true;
            this.TypeDll.Location = new System.Drawing.Point(6, 25);
            this.TypeDll.Name = "TypeDll";
            this.TypeDll.Size = new System.Drawing.Size(61, 24);
            this.TypeDll.TabIndex = 2;
            this.TypeDll.Text = ".DLL";
            this.TypeDll.UseVisualStyleBackColor = true;
            this.TypeDll.CheckedChanged += new System.EventHandler(this.TypeDll_CheckedChanged);
            // 
            // ApplitcationType
            // 
            this.ApplitcationType.Controls.Add(this.TypeMdb);
            this.ApplitcationType.Controls.Add(this.TypeHttp);
            this.ApplitcationType.Controls.Add(this.TypeDll);
            this.ApplitcationType.Controls.Add(this.TypeExe);
            this.ApplitcationType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplitcationType.Location = new System.Drawing.Point(30, 58);
            this.ApplitcationType.Name = "ApplitcationType";
            this.ApplitcationType.Size = new System.Drawing.Size(366, 64);
            this.ApplitcationType.TabIndex = 3;
            this.ApplitcationType.TabStop = false;
            this.ApplitcationType.Text = "Type of Application";
            // 
            // TypeMdb
            // 
            this.TypeMdb.AutoSize = true;
            this.TypeMdb.Location = new System.Drawing.Point(193, 25);
            this.TypeMdb.Name = "TypeMdb";
            this.TypeMdb.Size = new System.Drawing.Size(67, 24);
            this.TypeMdb.TabIndex = 4;
            this.TypeMdb.Text = ".MDB";
            this.TypeMdb.UseVisualStyleBackColor = true;
            this.TypeMdb.CheckedChanged += new System.EventHandler(this.TypeMdb_CheckedChanged);
            // 
            // ApplicationText
            // 
            this.ApplicationText.AutoSize = true;
            this.ApplicationText.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplicationText.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.ApplicationText.Location = new System.Drawing.Point(421, 72);
            this.ApplicationText.Name = "ApplicationText";
            this.ApplicationText.Size = new System.Drawing.Size(0, 25);
            this.ApplicationText.TabIndex = 41;
            // 
            // VersionNumber
            // 
            this.VersionNumber.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VersionNumber.AutoSize = true;
            this.VersionNumber.Location = new System.Drawing.Point(561, 435);
            this.VersionNumber.Name = "VersionNumber";
            this.VersionNumber.Size = new System.Drawing.Size(0, 13);
            this.VersionNumber.TabIndex = 42;
            // 
            // ApplicationMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 457);
            this.Controls.Add(this.VersionNumber);
            this.Controls.Add(this.ApplicationText);
            this.Controls.Add(this.ApplitcationType);
            this.Controls.Add(this.StartingDll);
            this.Controls.Add(this.dllText);
            this.Controls.Add(this.AccessKey);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.Upload);
            this.Controls.Add(this.ApplicationImage);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ArgumentsGroup);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.New);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ApplicationsList);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SetSource);
            this.Controls.Add(this.SourcePath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.StartingClass);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ApplicationName);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(888, 494);
            this.Name = "ApplicationMaintenance";
            this.Text = "Application Maintenance";
            this.ArgumentsGroup.ResumeLayout(false);
            this.ArgumentsGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Up)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApplicationImage)).EndInit();
            this.ApplitcationType.ResumeLayout(false);
            this.ApplitcationType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ApplicationName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox StartingClass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SourcePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button SetSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ApplicationsList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button New;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.ToolTip toolTipSource;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListBox Selected;
        private System.Windows.Forms.ListBox Available;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox ArgumentsGroup;
        private System.Windows.Forms.PictureBox AddRemove;
        private System.Windows.Forms.PictureBox Down;
        private System.Windows.Forms.PictureBox Up;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox ApplicationImage;
        private System.Windows.Forms.Button Upload;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox AccessKey;
        private System.Windows.Forms.TextBox StartingDll;
        private System.Windows.Forms.Label dllText;
        private System.Windows.Forms.RadioButton TypeHttp;
        private System.Windows.Forms.RadioButton TypeExe;
        private System.Windows.Forms.RadioButton TypeDll;
        private System.Windows.Forms.GroupBox ApplitcationType;
        private System.Windows.Forms.Label ApplicationText;
        private System.Windows.Forms.Label VersionNumber;
        private System.Windows.Forms.RadioButton TypeMdb;
    }
}