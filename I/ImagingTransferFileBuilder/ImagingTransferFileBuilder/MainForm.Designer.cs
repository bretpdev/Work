namespace ImagingTransferFileBuilder
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
            this.Tabs = new System.Windows.Forms.TabControl();
            this.GeneratorTab = new System.Windows.Forms.TabPage();
            this.generatorControl1 = new ImagingTransferFileBuilder.GeneratorControl();
            this.ImageScraperTab = new System.Windows.Forms.TabPage();
            this.imageScraperControl1 = new ImagingTransferFileBuilder.ImageScraperControl();
            this.PdfScraperTab = new System.Windows.Forms.TabPage();
            this.pdfScraperControl1 = new ImagingTransferFileBuilder.PdfScraperControl();
            this.AggregatorTab = new System.Windows.Forms.TabPage();
            this.aggregatorControl1 = new ImagingTransferFileBuilder.AggregatorControl();
            this.ValidatorTab = new System.Windows.Forms.TabPage();
            this.validatorControl1 = new ImagingTransferFileBuilder.ValidatorControl();
            this.DocTypesTab = new System.Windows.Forms.TabPage();
            this.ResultsList = new System.Windows.Forms.ListBox();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.CopyAllButton = new System.Windows.Forms.Button();
            this.docTypesControl1 = new ImagingTransferFileBuilder.UserControls.DocTypesControl();
            this.Tabs.SuspendLayout();
            this.GeneratorTab.SuspendLayout();
            this.ImageScraperTab.SuspendLayout();
            this.PdfScraperTab.SuspendLayout();
            this.AggregatorTab.SuspendLayout();
            this.ValidatorTab.SuspendLayout();
            this.DocTypesTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.Tabs.Controls.Add(this.GeneratorTab);
            this.Tabs.Controls.Add(this.ImageScraperTab);
            this.Tabs.Controls.Add(this.PdfScraperTab);
            this.Tabs.Controls.Add(this.AggregatorTab);
            this.Tabs.Controls.Add(this.ValidatorTab);
            this.Tabs.Controls.Add(this.DocTypesTab);
            this.Tabs.ItemSize = new System.Drawing.Size(50, 160);
            this.Tabs.Location = new System.Drawing.Point(12, 3);
            this.Tabs.Multiline = true;
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(571, 427);
            this.Tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.Tabs.TabIndex = 0;
            this.Tabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Tabs_DrawItem);
            // 
            // GeneratorTab
            // 
            this.GeneratorTab.Controls.Add(this.generatorControl1);
            this.GeneratorTab.Location = new System.Drawing.Point(164, 4);
            this.GeneratorTab.Name = "GeneratorTab";
            this.GeneratorTab.Padding = new System.Windows.Forms.Padding(3);
            this.GeneratorTab.Size = new System.Drawing.Size(403, 419);
            this.GeneratorTab.TabIndex = 0;
            this.GeneratorTab.Text = "Generator";
            this.GeneratorTab.UseVisualStyleBackColor = true;
            // 
            // generatorControl1
            // 
            this.generatorControl1.Location = new System.Drawing.Point(6, 5);
            this.generatorControl1.Name = "generatorControl1";
            this.generatorControl1.Size = new System.Drawing.Size(391, 422);
            this.generatorControl1.TabIndex = 1;
            // 
            // ImageScraperTab
            // 
            this.ImageScraperTab.Controls.Add(this.imageScraperControl1);
            this.ImageScraperTab.Location = new System.Drawing.Point(164, 4);
            this.ImageScraperTab.Name = "ImageScraperTab";
            this.ImageScraperTab.Padding = new System.Windows.Forms.Padding(3);
            this.ImageScraperTab.Size = new System.Drawing.Size(403, 419);
            this.ImageScraperTab.TabIndex = 1;
            this.ImageScraperTab.Text = "Image Scraper";
            this.ImageScraperTab.UseVisualStyleBackColor = true;
            // 
            // imageScraperControl1
            // 
            this.imageScraperControl1.Location = new System.Drawing.Point(7, 6);
            this.imageScraperControl1.Name = "imageScraperControl1";
            this.imageScraperControl1.Size = new System.Drawing.Size(393, 407);
            this.imageScraperControl1.TabIndex = 0;
            // 
            // PdfScraperTab
            // 
            this.PdfScraperTab.Controls.Add(this.pdfScraperControl1);
            this.PdfScraperTab.Location = new System.Drawing.Point(164, 4);
            this.PdfScraperTab.Name = "PdfScraperTab";
            this.PdfScraperTab.Padding = new System.Windows.Forms.Padding(3);
            this.PdfScraperTab.Size = new System.Drawing.Size(403, 419);
            this.PdfScraperTab.TabIndex = 2;
            this.PdfScraperTab.Text = "PDF Scraper";
            this.PdfScraperTab.UseVisualStyleBackColor = true;
            // 
            // pdfScraperControl1
            // 
            this.pdfScraperControl1.Location = new System.Drawing.Point(3, 3);
            this.pdfScraperControl1.Name = "pdfScraperControl1";
            this.pdfScraperControl1.Size = new System.Drawing.Size(394, 400);
            this.pdfScraperControl1.TabIndex = 0;
            // 
            // AggregatorTab
            // 
            this.AggregatorTab.Controls.Add(this.aggregatorControl1);
            this.AggregatorTab.Location = new System.Drawing.Point(164, 4);
            this.AggregatorTab.Name = "AggregatorTab";
            this.AggregatorTab.Padding = new System.Windows.Forms.Padding(3);
            this.AggregatorTab.Size = new System.Drawing.Size(403, 419);
            this.AggregatorTab.TabIndex = 3;
            this.AggregatorTab.Text = "Aggregator";
            this.AggregatorTab.UseVisualStyleBackColor = true;
            // 
            // aggregatorControl1
            // 
            this.aggregatorControl1.Location = new System.Drawing.Point(5, 6);
            this.aggregatorControl1.Name = "aggregatorControl1";
            this.aggregatorControl1.Size = new System.Drawing.Size(395, 188);
            this.aggregatorControl1.TabIndex = 0;
            // 
            // ValidatorTab
            // 
            this.ValidatorTab.Controls.Add(this.validatorControl1);
            this.ValidatorTab.Location = new System.Drawing.Point(164, 4);
            this.ValidatorTab.Name = "ValidatorTab";
            this.ValidatorTab.Padding = new System.Windows.Forms.Padding(3);
            this.ValidatorTab.Size = new System.Drawing.Size(403, 419);
            this.ValidatorTab.TabIndex = 4;
            this.ValidatorTab.Text = "Validator";
            this.ValidatorTab.UseVisualStyleBackColor = true;
            // 
            // validatorControl1
            // 
            this.validatorControl1.Location = new System.Drawing.Point(6, 6);
            this.validatorControl1.Name = "validatorControl1";
            this.validatorControl1.Size = new System.Drawing.Size(394, 264);
            this.validatorControl1.TabIndex = 0;
            // 
            // DocTypesTab
            // 
            this.DocTypesTab.Controls.Add(this.docTypesControl1);
            this.DocTypesTab.Location = new System.Drawing.Point(164, 4);
            this.DocTypesTab.Name = "DocTypesTab";
            this.DocTypesTab.Size = new System.Drawing.Size(403, 419);
            this.DocTypesTab.TabIndex = 5;
            this.DocTypesTab.Text = "Doc Types & IDs";
            this.DocTypesTab.UseVisualStyleBackColor = true;
            // 
            // ResultsList
            // 
            this.ResultsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ResultsList.FormattingEnabled = true;
            this.ResultsList.HorizontalScrollbar = true;
            this.ResultsList.Location = new System.Drawing.Point(589, 46);
            this.ResultsList.Name = "ResultsList";
            this.ResultsList.Size = new System.Drawing.Size(462, 355);
            this.ResultsList.TabIndex = 1;
            this.ResultsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ResultsList_DrawItem);
            this.ResultsList.DoubleClick += new System.EventHandler(this.ResultsList_DoubleClick);
            this.ResultsList.Resize += new System.EventHandler(this.ResultsList_Resize);
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgressLabel.Location = new System.Drawing.Point(589, 3);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(462, 42);
            this.ProgressLabel.TabIndex = 27;
            this.ProgressLabel.Text = "Results";
            this.ProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CopyAllButton
            // 
            this.CopyAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyAllButton.Enabled = false;
            this.CopyAllButton.Location = new System.Drawing.Point(783, 407);
            this.CopyAllButton.Name = "CopyAllButton";
            this.CopyAllButton.Size = new System.Drawing.Size(75, 23);
            this.CopyAllButton.TabIndex = 28;
            this.CopyAllButton.Text = "Copy All";
            this.CopyAllButton.UseVisualStyleBackColor = true;
            this.CopyAllButton.Click += new System.EventHandler(this.CopyAllButton_Click);
            // 
            // docTypesControl1
            // 
            this.docTypesControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.docTypesControl1.Location = new System.Drawing.Point(0, 0);
            this.docTypesControl1.Name = "docTypesControl1";
            this.docTypesControl1.Size = new System.Drawing.Size(403, 419);
            this.docTypesControl1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 435);
            this.Controls.Add(this.CopyAllButton);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.ResultsList);
            this.Controls.Add(this.Tabs);
            this.Name = "MainForm";
            this.Text = "Imaging Transfer File Builder";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Tabs.ResumeLayout(false);
            this.GeneratorTab.ResumeLayout(false);
            this.ImageScraperTab.ResumeLayout(false);
            this.PdfScraperTab.ResumeLayout(false);
            this.AggregatorTab.ResumeLayout(false);
            this.ValidatorTab.ResumeLayout(false);
            this.DocTypesTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage GeneratorTab;
        private System.Windows.Forms.TabPage ImageScraperTab;
        private System.Windows.Forms.TabPage PdfScraperTab;
        private System.Windows.Forms.TabPage AggregatorTab;
        private System.Windows.Forms.TabPage ValidatorTab;
        private GeneratorControl generatorControl1;
        private System.Windows.Forms.ListBox ResultsList;
        private AggregatorControl aggregatorControl1;
        private System.Windows.Forms.Label ProgressLabel;
        private ValidatorControl validatorControl1;
        private PdfScraperControl pdfScraperControl1;
        private ImageScraperControl imageScraperControl1;
        private System.Windows.Forms.Button CopyAllButton;
        private System.Windows.Forms.TabPage DocTypesTab;
        private UserControls.DocTypesControl docTypesControl1;
    }
}

