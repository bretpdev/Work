namespace Dagobah
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnlFront = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvRequests = new System.Windows.Forms.DataGridView();
            this.lblLetter = new System.Windows.Forms.Label();
            this.lblSas = new System.Windows.Forms.Label();
            this.lblScript = new System.Windows.Forms.Label();
            this.lblDagobah = new System.Windows.Forms.Label();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.radByCourt = new System.Windows.Forms.RadioButton();
            this.radByProgrammer = new System.Windows.Forms.RadioButton();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lstUsers = new System.Windows.Forms.ListView();
            this.chkWhisper = new System.Windows.Forms.CheckBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lstSkins = new System.Windows.Forms.ListBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.txtReceive = new System.Windows.Forms.TextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlFront.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).BeginInit();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFront
            // 
            this.pnlFront.BackColor = System.Drawing.Color.Transparent;
            this.pnlFront.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlFront.Controls.Add(this.lblTitle);
            this.pnlFront.Controls.Add(this.dgvRequests);
            this.pnlFront.Controls.Add(this.lblLetter);
            this.pnlFront.Controls.Add(this.lblSas);
            this.pnlFront.Controls.Add(this.lblScript);
            this.pnlFront.Controls.Add(this.lblDagobah);
            this.pnlFront.Controls.Add(this.txtSend);
            this.pnlFront.Controls.Add(this.radByCourt);
            this.pnlFront.Controls.Add(this.radByProgrammer);
            this.pnlFront.Controls.Add(this.btnExit);
            this.pnlFront.Controls.Add(this.btnMinimize);
            this.pnlFront.Location = new System.Drawing.Point(200, 0);
            this.pnlFront.Name = "pnlFront";
            this.pnlFront.Size = new System.Drawing.Size(400, 300);
            this.pnlFront.TabIndex = 0;
            this.pnlFront.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlFront_MouseMove);
            this.pnlFront.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlFront_MouseDown);
            this.pnlFront.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlFront_MouseUp);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 17);
            this.lblTitle.TabIndex = 10;
            this.lblTitle.Text = "User Name: System";
            this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseMove);
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseUp);
            // 
            // dgvRequests
            // 
            this.dgvRequests.AllowUserToAddRows = false;
            this.dgvRequests.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Lime;
            this.dgvRequests.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvRequests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRequests.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvRequests.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRequests.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRequests.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvRequests.Location = new System.Drawing.Point(7, 42);
            this.dgvRequests.Name = "dgvRequests";
            this.dgvRequests.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRequests.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvRequests.RowHeadersVisible = false;
            this.dgvRequests.RowHeadersWidth = 4;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Lime;
            this.dgvRequests.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvRequests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvRequests.Size = new System.Drawing.Size(387, 169);
            this.dgvRequests.TabIndex = 9;
            // 
            // lblLetter
            // 
            this.lblLetter.Font = new System.Drawing.Font("OCR A Extended", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLetter.Location = new System.Drawing.Point(226, 233);
            this.lblLetter.Name = "lblLetter";
            this.lblLetter.Size = new System.Drawing.Size(64, 56);
            this.lblLetter.TabIndex = 8;
            this.lblLetter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLetter.Click += new System.EventHandler(this.lblLetter_Click);
            // 
            // lblSas
            // 
            this.lblSas.Font = new System.Drawing.Font("OCR A Extended", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSas.Location = new System.Drawing.Point(156, 233);
            this.lblSas.Name = "lblSas";
            this.lblSas.Size = new System.Drawing.Size(64, 56);
            this.lblSas.TabIndex = 7;
            this.lblSas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSas.Click += new System.EventHandler(this.lblSas_Click);
            // 
            // lblScript
            // 
            this.lblScript.Font = new System.Drawing.Font("OCR A Extended", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScript.Location = new System.Drawing.Point(86, 233);
            this.lblScript.Name = "lblScript";
            this.lblScript.Size = new System.Drawing.Size(64, 56);
            this.lblScript.TabIndex = 6;
            this.lblScript.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblScript.Click += new System.EventHandler(this.lblScript_Click);
            // 
            // lblDagobah
            // 
            this.lblDagobah.Font = new System.Drawing.Font("OCR A Extended", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDagobah.Location = new System.Drawing.Point(16, 233);
            this.lblDagobah.Name = "lblDagobah";
            this.lblDagobah.Size = new System.Drawing.Size(64, 56);
            this.lblDagobah.TabIndex = 5;
            this.lblDagobah.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDagobah.Click += new System.EventHandler(this.lblDagobah_Click);
            // 
            // txtSend
            // 
            this.txtSend.BackColor = System.Drawing.Color.Black;
            this.txtSend.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSend.ForeColor = System.Drawing.Color.Lime;
            this.txtSend.Location = new System.Drawing.Point(296, 217);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(88, 72);
            this.txtSend.TabIndex = 4;
            this.txtSend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSend_KeyPress);
            // 
            // radByCourt
            // 
            this.radByCourt.AutoSize = true;
            this.radByCourt.Checked = true;
            this.radByCourt.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radByCourt.Location = new System.Drawing.Point(226, 217);
            this.radByCourt.Name = "radByCourt";
            this.radByCourt.Size = new System.Drawing.Size(53, 13);
            this.radByCourt.TabIndex = 3;
            this.radByCourt.TabStop = true;
            this.radByCourt.Text = "By Court";
            this.radByCourt.UseVisualStyleBackColor = true;
            this.radByCourt.CheckedChanged += new System.EventHandler(this.radByCourt_CheckedChanged);
            // 
            // radByProgrammer
            // 
            this.radByProgrammer.AutoSize = true;
            this.radByProgrammer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radByProgrammer.Location = new System.Drawing.Point(116, 217);
            this.radByProgrammer.Name = "radByProgrammer";
            this.radByProgrammer.Size = new System.Drawing.Size(78, 13);
            this.radByProgrammer.TabIndex = 2;
            this.radByProgrammer.Text = "By Programmer";
            this.radByProgrammer.UseVisualStyleBackColor = true;
            this.radByProgrammer.CheckedChanged += new System.EventHandler(this.radByProgrammer_CheckedChanged);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Black;
            this.btnExit.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.Lime;
            this.btnExit.Location = new System.Drawing.Point(360, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(24, 24);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "X";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Black;
            this.btnMinimize.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.Lime;
            this.btnMinimize.Location = new System.Drawing.Point(330, 12);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(24, 24);
            this.btnMinimize.TabIndex = 0;
            this.btnMinimize.Text = "_";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.Transparent;
            this.pnlLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlLeft.Controls.Add(this.lstUsers);
            this.pnlLeft.Controls.Add(this.chkWhisper);
            this.pnlLeft.Location = new System.Drawing.Point(0, 25);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(200, 250);
            this.pnlLeft.TabIndex = 1;
            this.pnlLeft.DoubleClick += new System.EventHandler(this.pnlLeft_DoubleClick);
            // 
            // lstUsers
            // 
            this.lstUsers.BackColor = System.Drawing.Color.Black;
            this.lstUsers.ForeColor = System.Drawing.Color.Silver;
            this.lstUsers.Location = new System.Drawing.Point(73, 29);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(121, 160);
            this.lstUsers.TabIndex = 2;
            this.lstUsers.UseCompatibleStateImageBehavior = false;
            this.lstUsers.View = System.Windows.Forms.View.List;
            this.lstUsers.DoubleClick += new System.EventHandler(this.lstUsers_DoubleClick);
            // 
            // chkWhisper
            // 
            this.chkWhisper.AutoSize = true;
            this.chkWhisper.ForeColor = System.Drawing.Color.White;
            this.chkWhisper.Location = new System.Drawing.Point(105, 208);
            this.chkWhisper.Name = "chkWhisper";
            this.chkWhisper.Size = new System.Drawing.Size(65, 17);
            this.chkWhisper.TabIndex = 1;
            this.chkWhisper.Text = "Whisper";
            this.chkWhisper.UseVisualStyleBackColor = true;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.Transparent;
            this.pnlRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlRight.Controls.Add(this.lstSkins);
            this.pnlRight.Location = new System.Drawing.Point(600, 25);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(200, 250);
            this.pnlRight.TabIndex = 2;
            this.pnlRight.DoubleClick += new System.EventHandler(this.pnlRight_DoubleClick);
            // 
            // lstSkins
            // 
            this.lstSkins.BackColor = System.Drawing.Color.Black;
            this.lstSkins.ForeColor = System.Drawing.Color.Lime;
            this.lstSkins.FormattingEnabled = true;
            this.lstSkins.Location = new System.Drawing.Point(7, 29);
            this.lstSkins.Name = "lstSkins";
            this.lstSkins.Size = new System.Drawing.Size(120, 160);
            this.lstSkins.TabIndex = 0;
            this.lstSkins.DoubleClick += new System.EventHandler(this.lstSkins_DoubleClick);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlBottom.Controls.Add(this.txtReceive);
            this.pnlBottom.Location = new System.Drawing.Point(225, 300);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(350, 200);
            this.pnlBottom.TabIndex = 3;
            this.pnlBottom.DoubleClick += new System.EventHandler(this.pnlBottom_DoubleClick);
            // 
            // txtReceive
            // 
            this.txtReceive.BackColor = System.Drawing.Color.Black;
            this.txtReceive.ForeColor = System.Drawing.Color.Lime;
            this.txtReceive.Location = new System.Drawing.Point(25, 3);
            this.txtReceive.Multiline = true;
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.ReadOnly = true;
            this.txtReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReceive.Size = new System.Drawing.Size(300, 150);
            this.txtReceive.TabIndex = 0;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Dagobah";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(802, 501);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlFront);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Opacity = 0;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dagobah";
            this.TransparencyKey = System.Drawing.Color.OldLace;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.pnlFront.ResumeLayout(false);
            this.pnlFront.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFront;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.RadioButton radByCourt;
        private System.Windows.Forms.RadioButton radByProgrammer;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Label lblLetter;
        private System.Windows.Forms.Label lblSas;
        private System.Windows.Forms.Label lblScript;
        private System.Windows.Forms.Label lblDagobah;
        private System.Windows.Forms.DataGridView dgvRequests;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox chkWhisper;
        private System.Windows.Forms.TextBox txtReceive;
        private System.Windows.Forms.ListBox lstSkins;
        private System.Windows.Forms.ListView lstUsers;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

