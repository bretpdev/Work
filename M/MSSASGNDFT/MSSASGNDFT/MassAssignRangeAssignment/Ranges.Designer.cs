namespace MassAssignRangeAssignment
{
    partial class Ranges
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
            this.UserName = new System.Windows.Forms.Label();
            this.UserId = new System.Windows.Forms.ComboBox();
            this.AesId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Remove = new System.Windows.Forms.Button();
            this.Count = new System.Windows.Forms.Label();
            this.Begin = new System.Windows.Forms.NumericUpDown();
            this.End = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.Begin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.End)).BeginInit();
            this.SuspendLayout();
            // 
            // UserName
            // 
            this.UserName.AutoSize = true;
            this.UserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserName.Location = new System.Drawing.Point(3, 8);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(0, 20);
            this.UserName.TabIndex = 0;
            // 
            // UserId
            // 
            this.UserId.FormattingEnabled = true;
            this.UserId.Location = new System.Drawing.Point(34, 6);
            this.UserId.Name = "UserId";
            this.UserId.Size = new System.Drawing.Size(166, 21);
            this.UserId.TabIndex = 0;
            this.UserId.SelectedIndexChanged += new System.EventHandler(this.UserId_SelectedIndexChanged);
            // 
            // AesId
            // 
            this.AesId.Enabled = false;
            this.AesId.Location = new System.Drawing.Point(217, 6);
            this.AesId.MaxLength = 7;
            this.AesId.Name = "AesId";
            this.AesId.Size = new System.Drawing.Size(100, 20);
            this.AesId.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(391, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "-";
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(488, 4);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(75, 23);
            this.Remove.TabIndex = 3;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Count
            // 
            this.Count.AutoSize = true;
            this.Count.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Count.Location = new System.Drawing.Point(7, 9);
            this.Count.Name = "Count";
            this.Count.Size = new System.Drawing.Size(45, 16);
            this.Count.TabIndex = 8;
            this.Count.Text = "label2";
            // 
            // Begin
            // 
            this.Begin.BackColor = System.Drawing.Color.White;
            this.Begin.Location = new System.Drawing.Point(334, 6);
            this.Begin.Maximum = new decimal(new int[] {
            98,
            0,
            0,
            0});
            this.Begin.Name = "Begin";
            this.Begin.Size = new System.Drawing.Size(51, 20);
            this.Begin.TabIndex = 1;
            this.Begin.ValueChanged += new System.EventHandler(this.Begin_ValueChanged);
            this.Begin.Click += new System.EventHandler(this.Begin_ValueChanged);
            // 
            // End
            // 
            this.End.BackColor = System.Drawing.Color.White;
            this.End.Location = new System.Drawing.Point(417, 7);
            this.End.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.End.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.End.Name = "End";
            this.End.Size = new System.Drawing.Size(51, 20);
            this.End.TabIndex = 2;
            this.End.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.End.ValueChanged += new System.EventHandler(this.End_ValueChanged);
            this.End.Click += new System.EventHandler(this.End_ValueChanged);
            // 
            // Ranges
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.Controls.Add(this.End);
            this.Controls.Add(this.Begin);
            this.Controls.Add(this.Count);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AesId);
            this.Controls.Add(this.UserId);
            this.Controls.Add(this.UserName);
            this.Name = "Ranges";
            this.Size = new System.Drawing.Size(574, 32);
            ((System.ComponentModel.ISupportInitialize)(this.Begin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.End)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UserName;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox UserId;
        public System.Windows.Forms.TextBox AesId;
        public System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Label Count;
        public System.Windows.Forms.NumericUpDown Begin;
        public System.Windows.Forms.NumericUpDown End;
    }
}
