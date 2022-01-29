namespace LS008
{
    partial class ManualCommentTask
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualCommentTask));
            this.label1 = new System.Windows.Forms.Label();
            this.Arc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ArcComment = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TaskComment = new System.Windows.Forms.TextBox();
            this.ArcsToAdd = new System.Windows.Forms.DataGridView();
            this.RowDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.Add = new System.Windows.Forms.Button();
            this.Continue = new System.Windows.Forms.Button();
            this.Loans = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Recipid = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ArcsToAdd)).BeginInit();
            this.RowDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "ATD22 ARC:";
            // 
            // Arc
            // 
            this.Arc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Arc.Location = new System.Drawing.Point(119, 6);
            this.Arc.MaxLength = 5;
            this.Arc.Name = "Arc";
            this.Arc.Size = new System.Drawing.Size(100, 26);
            this.Arc.TabIndex = 1;
            this.Arc.Leave += new System.EventHandler(this.Arc_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "ARC Comment:";
            // 
            // ArcComment
            // 
            this.ArcComment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.ArcComment.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ArcComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArcComment.Location = new System.Drawing.Point(16, 100);
            this.ArcComment.MaxLength = 1000;
            this.ArcComment.Multiline = true;
            this.ArcComment.Name = "ArcComment";
            this.ArcComment.Size = new System.Drawing.Size(438, 168);
            this.ArcComment.TabIndex = 6;
            this.ArcComment.Leave += new System.EventHandler(this.ArcComment_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "LS008 Task Comment:";
            // 
            // TaskComment
            // 
            this.TaskComment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.TaskComment.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TaskComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaskComment.Location = new System.Drawing.Point(16, 294);
            this.TaskComment.MaxLength = 260;
            this.TaskComment.Multiline = true;
            this.TaskComment.Name = "TaskComment";
            this.TaskComment.Size = new System.Drawing.Size(438, 168);
            this.TaskComment.TabIndex = 8;
            this.TaskComment.TextChanged += new System.EventHandler(this.TaskComment_TextChanged);
            this.TaskComment.Leave += new System.EventHandler(this.TaskComment_Leave);
            // 
            // ArcsToAdd
            // 
            this.ArcsToAdd.AllowUserToAddRows = false;
            this.ArcsToAdd.AllowUserToDeleteRows = false;
            this.ArcsToAdd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ArcsToAdd.ContextMenuStrip = this.RowDelete;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ArcsToAdd.DefaultCellStyle = dataGridViewCellStyle1;
            this.ArcsToAdd.Location = new System.Drawing.Point(469, 27);
            this.ArcsToAdd.Name = "ArcsToAdd";
            this.ArcsToAdd.ReadOnly = true;
            this.ArcsToAdd.Size = new System.Drawing.Size(628, 435);
            this.ArcsToAdd.TabIndex = 12;
            this.ArcsToAdd.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.ArcsToAdd_CellContextMenuStripNeeded);
            this.ArcsToAdd.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ArcsToAdd_CellDoubleClick);
            // 
            // RowDelete
            // 
            this.RowDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Delete});
            this.RowDelete.Name = "RowDelete";
            this.RowDelete.Size = new System.Drawing.Size(108, 26);
            // 
            // Delete
            // 
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(107, 22);
            this.Delete.Text = "Delete";
            this.Delete.ToolTipText = "Delete";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(465, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Arc to be added";
            // 
            // Add
            // 
            this.Add.Enabled = false;
            this.Add.Location = new System.Drawing.Point(379, 468);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 34);
            this.Add.TabIndex = 9;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Continue
            // 
            this.Continue.Location = new System.Drawing.Point(469, 468);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(93, 34);
            this.Continue.TabIndex = 10;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // Loans
            // 
            this.Loans.Enabled = false;
            this.Loans.Location = new System.Drawing.Point(240, 4);
            this.Loans.Name = "Loans";
            this.Loans.Size = new System.Drawing.Size(112, 31);
            this.Loans.TabIndex = 2;
            this.Loans.Text = "Select Loans";
            this.Loans.UseVisualStyleBackColor = true;
            this.Loans.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "Recipient ID:";
            // 
            // Recipid
            // 
            this.Recipid.Location = new System.Drawing.Point(119, 42);
            this.Recipid.MaxLength = 9;
            this.Recipid.Name = "Recipid";
            this.Recipid.Size = new System.Drawing.Size(140, 26);
            this.Recipid.TabIndex = 4;
            // 
            // ManualCommentTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 514);
            this.Controls.Add(this.Recipid);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Loans);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ArcsToAdd);
            this.Controls.Add(this.TaskComment);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ArcComment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Arc);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ManualCommentTask";
            this.Text = "Enter  Comments";
            ((System.ComponentModel.ISupportInitialize)(this.ArcsToAdd)).EndInit();
            this.RowDelete.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Arc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ArcComment;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TaskComment;
        private System.Windows.Forms.DataGridView ArcsToAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.ContextMenuStrip RowDelete;
        private System.Windows.Forms.ToolStripMenuItem Delete;
        private System.Windows.Forms.Button Loans;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Recipid;
    }
}