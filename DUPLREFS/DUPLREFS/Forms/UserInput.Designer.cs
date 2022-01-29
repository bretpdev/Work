namespace DUPLREFS
{
    partial class UserInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInput));
            this.lbl_UserPrompt = new System.Windows.Forms.Label();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.refControl2 = new DUPLREFS.Forms.ReferenceControl();
            this.refControl3 = new DUPLREFS.Forms.ReferenceControl();
            this.refControl4 = new DUPLREFS.Forms.ReferenceControl();
            this.refControl1 = new DUPLREFS.Forms.ReferenceControl();
            this.SuspendLayout();
            // 
            // lbl_UserPrompt
            // 
            this.lbl_UserPrompt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_UserPrompt.Location = new System.Drawing.Point(91, 9);
            this.lbl_UserPrompt.Name = "lbl_UserPrompt";
            this.lbl_UserPrompt.Size = new System.Drawing.Size(626, 36);
            this.lbl_UserPrompt.TabIndex = 20;
            this.lbl_UserPrompt.Text = resources.GetString("lbl_UserPrompt.Text");
            // 
            // btn_Ok
            // 
            this.btn_Ok.Location = new System.Drawing.Point(699, 478);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(108, 44);
            this.btn_Ok.TabIndex = 81;
            this.btn_Ok.Text = "OK";
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // refControl2
            // 
            this.refControl2.Location = new System.Drawing.Point(405, 57);
            this.refControl2.Name = "refControl2";
            this.refControl2.Size = new System.Drawing.Size(385, 204);
            this.refControl2.TabIndex = 89;
            // 
            // refControl3
            // 
            this.refControl3.Location = new System.Drawing.Point(12, 267);
            this.refControl3.Name = "refControl3";
            this.refControl3.Size = new System.Drawing.Size(385, 204);
            this.refControl3.TabIndex = 90;
            // 
            // refControl4
            // 
            this.refControl4.Location = new System.Drawing.Point(405, 267);
            this.refControl4.Name = "refControl4";
            this.refControl4.Size = new System.Drawing.Size(385, 204);
            this.refControl4.TabIndex = 91;
            // 
            // refControl1
            // 
            this.refControl1.Location = new System.Drawing.Point(11, 57);
            this.refControl1.Name = "refControl1";
            this.refControl1.Size = new System.Drawing.Size(385, 204);
            this.refControl1.TabIndex = 88;
            // 
            // UserInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 534);
            this.Controls.Add(this.refControl4);
            this.Controls.Add(this.refControl3);
            this.Controls.Add(this.refControl2);
            this.Controls.Add(this.refControl1);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.lbl_UserPrompt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "UserInput";
            this.Text = "UserInput";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbl_UserPrompt;
        private System.Windows.Forms.Button btn_Ok;
        private Forms.ReferenceControl refControl2;
        private Forms.ReferenceControl refControl3;
        private Forms.ReferenceControl refControl4;
        private Forms.ReferenceControl refControl1;
    }
}