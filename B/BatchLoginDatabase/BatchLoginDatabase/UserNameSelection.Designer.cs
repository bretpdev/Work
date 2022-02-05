namespace BatchLoginDatabase
{
    partial class UserNameSelection
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.UserIds = new System.Windows.Forms.DataGridView();
            this.Add = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ChangeScriptInfo = new System.Windows.Forms.Button();
            this.Login = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UserIds)).BeginInit();
            this.SuspendLayout();
            // 
            // UserIds
            // 
            this.UserIds.AllowUserToAddRows = false;
            this.UserIds.AllowUserToDeleteRows = false;
            this.UserIds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserIds.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UserIds.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.UserIds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.UserIds.DefaultCellStyle = dataGridViewCellStyle4;
            this.UserIds.Location = new System.Drawing.Point(13, 64);
            this.UserIds.Name = "UserIds";
            this.UserIds.ReadOnly = true;
            this.UserIds.RowHeadersVisible = false;
            this.UserIds.Size = new System.Drawing.Size(695, 273);
            this.UserIds.TabIndex = 0;
            this.UserIds.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.UserIds_CellDoubleClick);
            // 
            // Add
            // 
            this.Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Add.Location = new System.Drawing.Point(338, 12);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(120, 36);
            this.Add.TabIndex = 1;
            this.Add.Text = "Add User Id";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Double click an Id below to change or delete";
            // 
            // ChangeScriptInfo
            // 
            this.ChangeScriptInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChangeScriptInfo.Location = new System.Drawing.Point(464, 12);
            this.ChangeScriptInfo.Name = "ChangeScriptInfo";
            this.ChangeScriptInfo.Size = new System.Drawing.Size(120, 36);
            this.ChangeScriptInfo.TabIndex = 3;
            this.ChangeScriptInfo.Text = "Script Info";
            this.ChangeScriptInfo.UseVisualStyleBackColor = true;
            this.ChangeScriptInfo.Click += new System.EventHandler(this.ChangeScriptInfo_Click);
            // 
            // Login
            // 
            this.Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Login.Location = new System.Drawing.Point(590, 12);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(120, 36);
            this.Login.TabIndex = 4;
            this.Login.Text = "Login Types";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.Login_Click);
            // 
            // UserNameSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 349);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.ChangeScriptInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.UserIds);
            this.Name = "UserNameSelection";
            this.ShowIcon = false;
            this.Text = "UserNameSelection";
            ((System.ComponentModel.ISupportInitialize)(this.UserIds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView UserIds;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ChangeScriptInfo;
        private System.Windows.Forms.Button Login;
    }
}

