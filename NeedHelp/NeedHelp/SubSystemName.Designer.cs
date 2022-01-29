namespace NeedHelp
{
    partial class SubSystemName
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
            this.btnSubSystemName = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSubSystemName
            // 
            this.btnSubSystemName.BackColor = System.Drawing.Color.LightGray;
            this.btnSubSystemName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSubSystemName.FlatAppearance.BorderSize = 0;
            this.btnSubSystemName.Location = new System.Drawing.Point(0, 0);
            this.btnSubSystemName.Name = "btnSubSystemName";
            this.btnSubSystemName.Size = new System.Drawing.Size(167, 23);
            this.btnSubSystemName.TabIndex = 0;
            this.btnSubSystemName.UseVisualStyleBackColor = true;
            this.btnSubSystemName.Click += new System.EventHandler(this.BtnSubSystemName_Click);
            // 
            // SubSystemName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSubSystemName);
            this.Name = "SubSystemName";
            this.Size = new System.Drawing.Size(167, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSubSystemName;

    }
}
