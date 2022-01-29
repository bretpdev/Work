using System;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common.DataAccess;

namespace FileSeparator
{
    public class FileSeparator
    {
        public FileSeparator(int mode)
        {
            DataAccessHelper.CurrentMode = (DataAccessHelper.Mode)mode;
            Separate();
        }

        public void Separate()
        {
            Region reg = new Region();
            DataAccessHelper.CurrentRegion = reg.ShowDialog() == DialogResult.OK ? DataAccessHelper.Region.CornerStone : DataAccessHelper.Region.Uheaa;

            int numberOfFiles = 0;
            EOJReport report = new EOJReport();
            int rowsPerFile = 0;

            //Open a FileDialog and allow a user to either choose a file or cancel and end the script.
            while (true)
            {
                string fileName = OpenFile();
                if (fileName != string.Empty)
                {
                    SeparationSelection form = new SeparationSelection(fileName);
                    DialogResult result = form.ShowDialog();
                    if ((result == DialogResult.OK) && form.RowsPerFile > 0)
                    {
                        rowsPerFile = form.RowsPerFile;
                        numberOfFiles = form.NumberOfNewFiles;
                        report = FileSeparate(form, fileName);
                        break;
                    }
                    else if (result == DialogResult.Cancel)
                        Application.Exit();
                    else
                        continue;
                }
                else
                    if (MessageBox.Show("You did not choose a file to process. Do you want to end the script?", "Are you done?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        break;
            }

            //If there was an error separating the file, a null report will be created.
            if (!string.IsNullOrEmpty(report.FileName))
            {
                CreateEOJReport(report);

                if (numberOfFiles < report.NumberOfNewFiles)
                {
                    MessageBox.Show("Processing Complete!\r\n\r\nThis file had a borrower counter. The file could not be split at exactly " + rowsPerFile + " or else borrower data would be split up."
                        + " There are " + report.NumberOfNewFiles + " files instead of " + numberOfFiles + " files to keep borrower data together.", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("Process Complete!", "Process Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
        }

        /// <summary>
        /// Opens a FileDialog to allow a user to choose a SAS file
        /// </summary>
        /// <returns>The FileName of the chosen file</returns>
        private string OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileOk += (o, ea) =>
            {
                if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone && !FileSystemHelper.IsSecureLocation(dialog.FileName))
                {
                    MessageBox.Show("You must choose a file from a federal location when running this application in a federal mode.");
                    ea.Cancel = true;
                }
                else if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa && FileSystemHelper.IsSecureLocation(dialog.FileName))
                {
                    MessageBox.Show("You must choose a file from a commercial location when running this application in a commercial mode.");
                    ea.Cancel = true;
                }
            };
            dialog.InitialDirectory = EnterpriseFileSystem.FtpFolder;
            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.FileName;
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form">The SeparationSelection form and properties</param>
        private EOJReport FileSeparate(SeparationSelection form, string fileName)
        {
            LoadingProgress progress = new LoadingProgress("");
            EOJReport report = progress.Separate(form, fileName);
            progress.Close();
            return report;
        }//Separate()

        /// <summary>
        /// Creates a custom EOJ report
        /// </summary>
        /// <param name="report"></param>
        private void CreateEOJReport(EOJReport report)
        {
            try
            {
                string filePath = EnterpriseFileSystem.GetPath("File Separator");
                string name = filePath + "EOJ " + report.FileName + DateTime.Now.ToShortDateString().Replace("/", "_");
                using (StreamWriter sw = new StreamWriter(name + ".html"))
                {
                    sw.WriteLine("<html>");
                    sw.WriteLine("<body>");

                    sw.WriteLine("<h1>File Separator FED Report</h1>");
                    sw.WriteLine("<h2>End Of Job Report</h2>");
                    sw.WriteLine(filePath + report.FileName + ".html<br />");
                    sw.WriteLine("<br />");

                    sw.WriteLine("<table>");

                    //Title of file split
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<td style=\"width: 250px; border: solid 1px black;\">");
                    sw.WriteLine("Original File");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td style=\"border: solid 1px black;\">");
                    sw.WriteLine(report.FileName);
                    sw.WriteLine("</td>");
                    sw.WriteLine("</tr>");

                    //File location of new files
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<td style=\"border: solid 1px black;\">");
                    sw.WriteLine("New File Location");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td style=\"border: solid 1px black;\">");
                    sw.WriteLine(report.SaveLocation);
                    sw.WriteLine("</td>");
                    sw.WriteLine("</tr>");

                    //Number of rows in file
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<td style=\"border: solid 1px black;\">");
                    sw.WriteLine("Number of Rows in file");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td style=\"border: solid 1px black;\">");
                    sw.WriteLine(report.RowCount);
                    sw.WriteLine("</td>");
                    sw.WriteLine("</tr>");

                    //Number of rows per new file
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<td style=\"border: solid 1px black;\">");
                    sw.WriteLine("Number of Rows in each new file");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td style=\"border: solid 1px black;\">");
                    sw.WriteLine(report.RowsPerFile);
                    sw.WriteLine("</td>");
                    sw.WriteLine("</tr>");

                    //Number of new files
                    sw.WriteLine("<tr>");
                    sw.WriteLine("<td style=\"border: solid 1px black;\">");
                    sw.WriteLine("Number of new files");
                    sw.WriteLine("</td>");
                    sw.WriteLine("<td style=\"border: solid 1px black;\">");
                    sw.WriteLine(report.NumberOfNewFiles);
                    sw.WriteLine("</td>");
                    sw.WriteLine("</tr>");

                    sw.WriteLine("</table>");
                    sw.WriteLine("<h3>New File Names and Counts</h3>");
                    sw.WriteLine("<table>");

                    sw.WriteLine("<tr><td style=\"border: solid 1px black; font-style: bold;\"><strong>New File Name</strong></td><td style=\"border: solid 1px black; font-style: bold;\"><strong>File Count</strong</td></tr>");

                    //The new file names and counts
                    foreach (NewFiles file in report.NewFile)
                    {
                        sw.WriteLine("<tr>");
                        sw.WriteLine("<td style=\"padding-right: 30px; border: solid 1px black;\">");
                        sw.WriteLine(file.FileName);
                        sw.WriteLine("</td>");
                        sw.WriteLine("<td style=\"border: solid 1px black;\">");
                        sw.WriteLine(file.RowCount);
                        sw.WriteLine("</td>");
                        sw.WriteLine("</tr>");
                    }

                    sw.WriteLine("</table>");

                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");
                }
            }
            catch (Exception ex)
            {
                string message = "There was an error creating the End Of Job Report.\r\n\r\n" + ex.Message;
                MessageBox.Show(message, "Error Creating Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }//Class
}//Namespace