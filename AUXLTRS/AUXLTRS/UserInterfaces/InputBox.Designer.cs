namespace AUXLTRS
{
    partial class InputBox
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
            this.DisplayMessage = new System.Windows.Forms.TextBox();
            this.UserInput = new System.Windows.Forms.TextBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DisplayMessage
            // 
            this.DisplayMessage.BackColor = System.Drawing.SystemColors.Control;
            this.DisplayMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DisplayMessage.Enabled = false;
            this.DisplayMessage.Location = new System.Drawing.Point(12, 12);
            this.DisplayMessage.Multiline = true;
            this.DisplayMessage.Name = "DisplayMessage";
            this.DisplayMessage.Size = new System.Drawing.Size(588, 63);
            this.DisplayMessage.TabIndex = 0;
            // 
            // UserInput
            // 
            this.UserInput.Location = new System.Drawing.Point(12, 81);
            this.UserInput.Multiline = true;
            this.UserInput.Name = "UserInput";
            this.UserInput.Size = new System.Drawing.Size(576, 55);
            this.UserInput.TabIndex = 1;
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(500, 142);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(88, 37);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(12, 142);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(88, 37);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // InputBox
            // 
            this.AcceptButton = this.Cancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(612, 191);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.UserInput);
            this.Controls.Add(this.DisplayMessage);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "InputBox";
            this.Text = "Input Box";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InputBox_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DisplayMessage;
        private System.Windows.Forms.TextBox UserInput;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
    }
}