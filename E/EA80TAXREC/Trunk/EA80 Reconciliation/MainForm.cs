using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.ProcessLogger;

namespace EA80Reconciliation
{
    public partial class MainForm : Form
    {
		ProcessLogData LogData;

        public MainForm(ProcessLogData logData)
        {
			LogData = logData;
            InitializeComponent();
            Tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        private void Tabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = Tabs.TabPages[e.Index];
            

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = Tabs.GetTabRect(e.Index);
            if (e.State == DrawItemState.Selected)
            {

                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                g.FillRectangle(Brushes.Gray, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            
            // Use our own font.
            Font _tabFont = new Font("Arial", (float)14.0, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, e.Font, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Results.RegisterListBox(ResultsList);
            Results.RegisterCopyButton(CopyAllButton);
        }

        private void ResultsList_DoubleClick(object sender, EventArgs e)
        {
            if (ResultsList.SelectedItem != null)
                MessageBox.Show(((Results.Result)ResultsList.SelectedItem).Text);
        }

        private void ResultsList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            var item = (Results.Result)ResultsList.Items[e.Index];
            e.DrawBackground();
            Brush b = null;
            if (item.Type == Results.Result.ResultType.Notification)
                b = Brushes.Black;
            else
                b = Brushes.Red;
            e.Graphics.DrawString(item.Text, e.Font, b, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Save();
			ProcessLogger.LogEnd(LogData.ProcessLogId);
        }

        private void ResultsList_Resize(object sender, EventArgs e)
        {
            ResultsList.Refresh();
        }

        private void CopyAllButton_Click(object sender, EventArgs e)
        {
            string results = Results.ToString();
            if (!string.IsNullOrEmpty(results))
            {
                Clipboard.SetText(Results.ToString());
                MessageBox.Show("Results Copied.");
            }
        }
    }
}
