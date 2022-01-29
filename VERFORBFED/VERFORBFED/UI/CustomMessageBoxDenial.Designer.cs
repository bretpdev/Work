namespace VERFORBFED
{
    partial class CustomMessageBoxDenial
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
            this.Icon = new System.Windows.Forms.PictureBox();
            this.Message = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).BeginInit();
            this.SuspendLayout();
            // 
            // Icon
            // 
            this.Icon.Image = global::VERFORBFED.Properties.Resources.Dialog_warning_icon1;
            this.Icon.InitialImage = null;
            this.Icon.Location = new System.Drawing.Point(12, 47);
            this.Icon.Name = "Icon";
            this.Icon.Size = new System.Drawing.Size(128, 128);
            this.Icon.TabIndex = 6;
            this.Icon.TabStop = false;
            // 
            // Message
            // 
            this.Message.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Message.Location = new System.Drawing.Point(146, 9);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(353, 166);
            this.Message.TabIndex = 7;
            this.Message.Text = "Insert Text";
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(193, 191);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(125, 51);
            this.OK.TabIndex = 8;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            // 
            // CustomMessageBoxDenial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 276);
            this.ControlBox = false;
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.Icon);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(525, 315);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(525, 293);
            this.Name = "CustomMessageBoxDenial";
            this.ShowIcon = false;
            this.Text = "Important Notice!!";
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Icon;
        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.Button OK;
    }
}