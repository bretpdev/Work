using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ionic.Zip;
using Uheaa.Common;

namespace CommercialArchiveImport
{
    public partial class MainForm : Form
    {
        OpenFileDialog file = new OpenFileDialog();
        OpenFolderDialog folder = new OpenFolderDialog();
        public MainForm()
        {
            InitializeComponent();

            file.Filter = "Zip Files (*.zip)|*.zip";
            ResultsHelper.RegisterListBox(ResultsList);
            ProgressHelper.RegisterLabel(ResultsLabel);
            ProgressHelper.RegisterDisablee(SetupGroup);
            ProgressHelper.RegisterDisablee(ImportButton);

#if DEBUG
            ZipLocationText.Text = @"Q:\CS Systems Support\Melanie\Archive Testing\COLL_700577_700577_U0005_20120209_20120210165112-DONE.ZIP";
            ResultsLocationText.Text = @"C:\Users\ewalker\Desktop\resu";
#endif

        }

        private void ZipBrowseButton_Click(object sender, EventArgs e)
        {
            if (file.ShowDialog() == DialogResult.OK)
            {
                ZipLocationText.Text = file.FileName;
            }
        }

        private void ResultsBrowseButton_Click(object sender, EventArgs e)
        {
            if (folder.ShowDialog(this.Handle, false) == DialogResult.OK)
            {
                ResultsLocationText.Text = folder.Folder;
            }
        }

        List<Thread> resultWindows = new List<Thread>();
        private void ImportButton_Click(object sender, EventArgs e)
        {
            Thread main = new Thread(() =>
            {
                if (FileSystemHelper.CheckDirectory(ResultsLocationText.Text, secure: false) && FileSystemHelper.CheckFile(ZipLocationText.Text, secure: false) && IsInDocServices(ResultsLocationText.Text))
                {
                    try
                    {
                        EojResults results = MainProcessor.Process(ZipLocationText.Text, ResultsLocationText.Text, BatchNumberText.Text);
                        ResultsForm rf = new ResultsForm(results);
                        Application.Run(rf);
                    }
                    catch (MultipleIndexFilesException)
                    {
                        MessageBox.Show("Multiple index files were found in the zip file.  Cannot process until there is only one index file.");
                        ProgressHelper.Failure();
                    }
                    catch (NoIndexFileException)
                    {
                        MessageBox.Show("No index files were found in the zip file.  Cannot process without an index file to work from.");
                        ProgressHelper.Failure();
                    }
                    catch (NoEOJAccessException)
                    {
                        MessageBox.Show(string.Format("You do not have access to the EOJ report location ({0})", Eoj.EojFolder));
                        ProgressHelper.Failure();
                    }
                    catch (ThreadAbortException)
                    {
                        return; //we are closing the form
                    }
                    #if !DEBUG
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    #endif
                }
            });
            main.Start();
            resultWindows.Add(main);
        }

        private bool IsInDocServices(string path)
        {
#if DEBUG 
            return true;
#endif
            bool result = path.ToLower().StartsWith(@"q:\document services\");
            if (!result)
                MessageBox.Show("Results must be in Q:\\Document Services\\");
            return result;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainProcessor.Abort();
            foreach (Thread t in resultWindows)
                t.Abort();
        }
    }
}
