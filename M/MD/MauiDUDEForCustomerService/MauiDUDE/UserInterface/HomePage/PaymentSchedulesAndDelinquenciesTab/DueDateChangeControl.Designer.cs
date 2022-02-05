namespace MauiDUDE
{
    partial class DueDateChangeControl
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
            this.buttonCreateComment = new System.Windows.Forms.Button();
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // buttonCreateComment
            // 
            this.buttonCreateComment.Location = new System.Drawing.Point(101, 176);
            this.buttonCreateComment.Name = "buttonCreateComment";
            this.buttonCreateComment.Size = new System.Drawing.Size(174, 23);
            this.buttonCreateComment.TabIndex = 0;
            this.buttonCreateComment.Text = "Create Comment For Change";
            this.buttonCreateComment.UseVisualStyleBackColor = true;
            // 
            // monthCalendar
            // 
            this.monthCalendar.CalendarDimensions = new System.Drawing.Size(2, 1);
            this.monthCalendar.Location = new System.Drawing.Point(9, 9);
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 1;
            // 
            // DueDateChangeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.monthCalendar);
            this.Controls.Add(this.buttonCreateComment);
            this.Name = "DueDateChangeControl";
            this.Size = new System.Drawing.Size(474, 210);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Button buttonCreateComment;
        public System.Windows.Forms.MonthCalendar monthCalendar;
    }
}
