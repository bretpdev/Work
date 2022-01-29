using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace FileSeparator
{
    public partial class LoadingProgress : Form
    {
        string filePath;

        public LoadingProgress(string file)
        {
            InitializeComponent();
            filePath = file;
        }

        /// <summary>
        /// Loads the file selected into a List<string>
        /// </summary>
        /// <returns></returns>
        public List<string> GetDataCounts()
        {
            fileName.Text = "Loading " + filePath;
            progressBar.Maximum = File.ReadAllLines(filePath).Count();


            List<string> rows = new List<string>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    //Add the next line to the List<string>
                    rows.Add(sr.ReadLine());

                    //Do the events to get the progress bar to work
                    int percent = (int)(((double)progressBar.Value / (double)progressBar.Maximum) * 100);
                    Application.DoEvents();
                    progressBar.Refresh();
                    progressBar.CreateGraphics().DrawString(percent.ToString() + "%",
                        new Font("Arial", (float)12, FontStyle.Regular),
                        Brushes.Black,
                        new PointF(progressBar.Width / 2 - 10, progressBar.Height / 2 - 7));
                    progressBar.Increment(1);
                }
            }

            return rows;
        }

        /// <summary>
        /// Starts the progress bar
        /// </summary>
        /// <returns></returns>
        public List<string> Start()
        {
            this.Show();
            Application.DoEvents();
            return GetDataCounts();
        }

        /// <summary>
        /// Creates new documents and writes the rows to the documents
        /// </summary>
        /// <param name="form"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public EOJReport Separate(SeparationSelection form, string filePath)
        {

            EOJReport report = new EOJReport();

            try
            {
                this.Show();
                Application.DoEvents();

                NewFiles file = new NewFiles();

                //Get the paths for the new file locations
                string originalPath = EnterpriseFileSystem.GetPath("File Separator");
                string processingPath = EnterpriseFileSystem.GetPath("File Sep Processing");
                string reconciliationPath = EnterpriseFileSystem.GetPath("File Sep Reconciliation");

                //Copy the original file to the Archive folder and delete the original file
                string name = filePath.Substring(filePath.LastIndexOf("\\") + 1, (filePath.Length - filePath.LastIndexOf("\\") - 1));
                File.Copy(filePath, originalPath + name.Replace(".txt", "") + "." + DateTime.Now.ToShortDateString().Replace("/", "_") + ".txt", true);

                report.FileName = name;
                report.SaveLocation = processingPath;
                report.RowCount = form.Rows.Count;
                report.RowsPerFile = form.RowsPerFile;

                //Create a new file for each number of new files figured dividing the number of rows by the number of new rows
                for (int i = 1; i < form.NumberOfNewFiles + 1; i++)
                {
                    string newFilePath = processingPath + name + "." + DateTime.Now.ToShortDateString().Replace("/", "_") + "_" + i.ToString() + ".txt";
                    file = new NewFiles();
                    file.FileName = newFilePath;
                    using (StreamWriter sw = new StreamWriter(newFilePath))
                    {
                        if (form.HasHeaderRow)
                            sw.WriteLine(form.HeaderRow);

                        if (form.HasRecordCount)
                        {
                            WriteData(form, sw, file);
                        }
                        else
                        {
                            for (int j = 0; j < form.RowsPerFile; j++)
                            {
                                sw.WriteLine(form.Rows[0]);
                                form.Rows.RemoveAt(0);
                                ++file.RowCount;
                            }
                            if (form.Rows.Count < form.RowsPerFile)
                                form.RowsPerFile = form.Rows.Count;
                        }
                    }
                    if (File.Exists(newFilePath))
                        File.Copy(newFilePath, (reconciliationPath + name + "." + DateTime.Now.ToShortDateString().Replace("/", "_") + "_" + i.ToString() + ".txt"), true);

                    fileName.Text = "Created " + progressBar.Value + " of " + form.NumberOfNewFiles + " new files";
                    progressBar.Maximum = form.NumberOfNewFiles;
                    //Do the events to get the progress bar to work
                    int percent = (int)(((double)progressBar.Value / (double)progressBar.Maximum) * 100);
                    Application.DoEvents();
                    progressBar.Refresh();
                    progressBar.CreateGraphics().DrawString(percent.ToString() + "%",
                        new Font("Arial", (float)12, FontStyle.Regular),
                        Brushes.Black,
                        new PointF(progressBar.Width / 2 - 10, progressBar.Height / 2 - 7));
                    progressBar.Increment(1);
                    report.NewFile.Add(file);
                }

                //Check to see if there are still left over rows and procress them
                while (form.Rows.Count > 0)
                {
                    string path = processingPath + name + "." + DateTime.Now.ToShortDateString().Replace("/", "_") + "_" + ++form.NumberOfNewFiles + ".txt";
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        if (form.HasHeaderRow)
                            sw.WriteLine(form.HeaderRow);

                        if (form.HasRecordCount)
                        {
                            NewFiles extraFile = new NewFiles();
                            extraFile.FileName = path;
                            WriteData(form, sw, extraFile);
                            report.NewFile.Add(extraFile);
                        }
                    }
                }
                report.NumberOfNewFiles = form.NumberOfNewFiles;
#if !DEBUG
                File.Delete(filePath);
#endif
            }
            catch (Exception ex)
            {
                string message = "There was an error creating the new files. Please contact a programmer to debug the error\r\n\r\n" + ex.Message;
                MessageBox.Show(message, "Files Not Created", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new EOJReport();
            }
            return report;
        }

        /// <summary>
        /// Writes the user selected rows to a new file
        /// </summary>
        /// <param name="form"></param>
        /// <param name="sw"></param>
        /// <param name="file"></param>
        public void WriteData(SeparationSelection form, StreamWriter sw, NewFiles file)
        {
            int rowCount = form.Rows.Count > form.RowsPerFile ? form.RowsPerFile : form.Rows.Count - 1;
            int recordCount = int.Parse(form.Rows[rowCount].SplitAndRemoveQuotes(",")[0]);

            //If it is the final page, add 1 back to the record count to get the last record
            if (form.Rows.Count < form.RowsPerFile)
            {
                form.RowsPerFile = rowCount + 1;
                recordCount++;
            }

            for (int j = 0; j < form.RowsPerFile; j++)
            {
                if (int.Parse(form.Rows[0].SplitAndRemoveQuotes(",")[0]) < recordCount)
                {
                    sw.WriteLine(form.Rows[0]);
                    form.Rows.RemoveAt(0);
                    ++file.RowCount;
                }
            }
        }
    }
}
