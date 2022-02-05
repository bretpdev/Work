namespace MD
{
    partial class ImageAnnotator
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
            this.ToolsMenu = new System.Windows.Forms.MenuStrip();
            this.ChangeColorMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearAnnotationsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveChangesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.CancelMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.EditPicture = new System.Windows.Forms.PictureBox();
            this.ToolsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EditPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // ToolsMenu
            // 
            this.ToolsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeColorMenu,
            this.ClearAnnotationsMenu,
            this.SaveChangesMenu,
            this.CancelMenu});
            this.ToolsMenu.Location = new System.Drawing.Point(0, 0);
            this.ToolsMenu.Name = "ToolsMenu";
            this.ToolsMenu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.ToolsMenu.Size = new System.Drawing.Size(762, 24);
            this.ToolsMenu.TabIndex = 0;
            this.ToolsMenu.Text = "menuStrip1";
            // 
            // ChangeColorMenu
            // 
            this.ChangeColorMenu.Name = "ChangeColorMenu";
            this.ChangeColorMenu.Size = new System.Drawing.Size(92, 20);
            this.ChangeColorMenu.Text = "Change Color";
            this.ChangeColorMenu.Click += new System.EventHandler(this.ChangeColorMenu_Click);
            this.ChangeColorMenu.MouseEnter += new System.EventHandler(this.ChangeColorMenu_MouseEnter);
            this.ChangeColorMenu.MouseLeave += new System.EventHandler(this.ChangeColorMenu_MouseLeave);
            // 
            // ClearAnnotationsMenu
            // 
            this.ClearAnnotationsMenu.Name = "ClearAnnotationsMenu";
            this.ClearAnnotationsMenu.Size = new System.Drawing.Size(114, 20);
            this.ClearAnnotationsMenu.Text = "Clear Annotations";
            this.ClearAnnotationsMenu.Click += new System.EventHandler(this.ClearAnnotationsMenu_Click);
            // 
            // SaveChangesMenu
            // 
            this.SaveChangesMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SaveChangesMenu.Name = "SaveChangesMenu";
            this.SaveChangesMenu.Size = new System.Drawing.Size(92, 20);
            this.SaveChangesMenu.Text = "Save Changes";
            this.SaveChangesMenu.Click += new System.EventHandler(this.SaveChangesMenu_Click);
            // 
            // CancelMenu
            // 
            this.CancelMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CancelMenu.Name = "CancelMenu";
            this.CancelMenu.Size = new System.Drawing.Size(55, 20);
            this.CancelMenu.Text = "Cancel";
            this.CancelMenu.Click += new System.EventHandler(this.CancelMenu_Click);
            // 
            // EditPicture
            // 
            this.EditPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditPicture.Location = new System.Drawing.Point(0, 24);
            this.EditPicture.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EditPicture.Name = "EditPicture";
            this.EditPicture.Size = new System.Drawing.Size(762, 478);
            this.EditPicture.TabIndex = 1;
            this.EditPicture.TabStop = false;
            this.EditPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditPicture_MouseDown);
            this.EditPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EditPicture_MouseMove);
            this.EditPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EditPicture_MouseUp);
            // 
            // ImageAnnotator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 502);
            this.Controls.Add(this.EditPicture);
            this.Controls.Add(this.ToolsMenu);
            this.MainMenuStrip = this.ToolsMenu;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(778, 541);
            this.Name = "ImageAnnotator";
            this.Text = "Screenshot";
            this.ResizeEnd += new System.EventHandler(this.ImageAnnotator_ResizeEnd);
            this.ToolsMenu.ResumeLayout(false);
            this.ToolsMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EditPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip ToolsMenu;
        private System.Windows.Forms.PictureBox EditPicture;
        private System.Windows.Forms.ToolStripMenuItem ChangeColorMenu;
        private System.Windows.Forms.ToolStripMenuItem ClearAnnotationsMenu;
        private System.Windows.Forms.ToolStripMenuItem SaveChangesMenu;
        private System.Windows.Forms.ToolStripMenuItem CancelMenu;
    }
}