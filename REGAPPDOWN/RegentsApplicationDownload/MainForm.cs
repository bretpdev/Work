using System;
using System.Text;
using System.Windows.Forms;
using RegentsApplicationDownload.DataAccess;
using Q;

namespace RegentsApplicationDownload
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Entry point for the assembly.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                Downloader downloader = new Downloader();
                downloader.Log += new EventHandler<LogEventArgs>(downloader_Log);
                downloader.Run();
            }
            catch (Exception ex)
            {
                AppendToLog(ex.Message);
                //E-mail an error message to the appropriate people.
				bool testMode = RegentsScholarshipBackEnd.Constants.TEST_MODE;
                const string SUBJECT = "Regents' Scholarship DL Script Abort Error";
                string distributionList = BsysDataAccess.GetDistributionList(BsysDataAccess.ErrorType.RuntimeError);
                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.Append(string.Format("The Regents' Scholarship Download script did not run to completion on {0}. Please investigate.", DateTime.Now.ToString("MM/dd/yyyy")));
                messageBuilder.Append(Environment.NewLine);
                messageBuilder.Append(Environment.NewLine);
                messageBuilder.Append("The following stack trace may help the programmers determine the cause:");
                messageBuilder.Append(Environment.NewLine);
                messageBuilder.Append(ex.ToString());
                if (ex.InnerException != null)
                {
                    messageBuilder.Append(Environment.NewLine);
                    messageBuilder.Append(Environment.NewLine);
                    messageBuilder.Append("Inner Exception:");
                    messageBuilder.Append(ex.InnerException.ToString());
                }
                Common.SendMail(testMode, distributionList, "", SUBJECT, messageBuilder.ToString(), "", "", "", Common.EmailImportanceLevel.Normal, testMode);
            }
            finally
            {
                this.Close();
            }
        }//MainForm_Shown()

        private void AppendToLog(string message)
        {
            txtLog.AppendText(message);
            txtLog.AppendText(Environment.NewLine);
        }//AppendToLog()

        private void downloader_Log(object sender, LogEventArgs e)
        {
            AppendToLog(e.Message);
            Application.DoEvents();
        }//downloader_Log()
    }//class
}//namespace
