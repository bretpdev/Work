namespace MauiDUDE
{
    partial class GenericScriptAndServicesEnabled
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericScriptAndServicesEnabled));
            this.menuStripScriptsAndServices = new System.Windows.Forms.MenuStrip();
            this.SuspendLayout();
            // 
            // menuStripScriptsAndServices
            // 
            this.menuStripScriptsAndServices.Location = new System.Drawing.Point(0, 0);
            this.menuStripScriptsAndServices.Name = "menuStripScriptsAndServices";
            this.menuStripScriptsAndServices.Size = new System.Drawing.Size(785, 24);
            this.menuStripScriptsAndServices.TabIndex = 0;
            this.menuStripScriptsAndServices.Text = "menuStrip1";
            // 
            // GenericScriptAndServicesEnabled
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(174)))), ((int)(((byte)(231)))));
            this.ClientSize = new System.Drawing.Size(785, 589);
            this.Controls.Add(this.menuStripScriptsAndServices);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripScriptsAndServices;
            this.Name = "GenericScriptAndServicesEnabled";
            this.Text = "GenericScriptAndServicesEnabled";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripScriptsAndServices;
    }
}