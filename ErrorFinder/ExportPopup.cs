using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ErrorFinder
{
    public partial class ExportPopup : Form
    {
        string exportPath;
        IEnumerable<BorrowerLine> lines;
        string label;
        public ExportPopup(string exportPath, IEnumerable<BorrowerLine> lines, string label = null)
        {
            InitializeComponent();
            this.exportPath = exportPath;
            this.lines = lines;
            this.label = label;
            GeneratingFile = new Action<string>(fileName =>
            {
                this.Invoke(new Action(() =>
                {
                    ProgressBox.Items.Insert(0, "Generating " + fileName);
                }));
            });
            Success = new Action(() => this.Invoke(new Action(() =>
            {
                OpenLocationButton.Visible = true;
                ProgressBox.Items.Insert(0, "Finished. ");
            })));
            Failure = new Action(() => this.Invoke(new Action(() => this.Close())));

        }

        Action<string> GeneratingFile;
        Action Success;
        Action Failure;

        Thread t;
        private void Popup_Shown(object sender, EventArgs e)
        {
            t = new Thread(() =>
            {
                if (label != null) //only one file
                {
                    string fileName = label + "_" + Util.TimeStamp();
                    GeneratingFile(fileName);
                    bool success = ExportHelper.Export(lines, Path.Combine(exportPath, fileName));
                    if (success)
                        Success();
                    else
                        Failure();
                }
                else //trigger export of all views
                {
                    ExportHelper.ExportAll(lines, exportPath, GeneratingFile, Success, Failure);
                }
            });
            t.Start();
        }

        private void OpenLocationButton_Click(object sender, EventArgs e)
        {
            Process.Start(exportPath);
        }

        private void Popup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t != null)
                t.Abort();
        }
    }
}
