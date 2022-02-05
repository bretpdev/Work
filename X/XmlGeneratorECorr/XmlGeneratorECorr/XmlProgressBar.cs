using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace XmlGeneratorECorr
{
    public partial class XmlProgressBar : Form
    {
        private const string XmlPrepFolder = "ECORR_XML_PREP";
        private const string XmlFolder = "ECORR_XML";
        private int NumberOfFilesProcessed { get; set; }
        private bool OnlyRunBilling { get; set; }
        private string RecordCountSproc
        {
            get
            {
                return OnlyRunBilling ? "GetUnprocessedRecordsCountBills" : "GetUnprocessedRecordsCount";
            }
        }

        private string GetNextRecordSproc
        {
            get
            {
                return OnlyRunBilling ? "GetNextUnprocessedRecordBills" : "GetNextUnprocessedRecord";
            }
        }

        private string ProgressCount
        {
            get
            {
                return string.Format("Number of files processed: {0}", NumberOfFilesProcessed);
            }
        }


        public XmlProgressBar(bool onlyRunBilling)
        {
            InitializeComponent();
            OnlyRunBilling = onlyRunBilling;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (DataAccessHelper.ExecuteSingle<int>(RecordCountSproc, XmlGenerator.EcorrFedCon) == 0)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            List<Task> tasks = new List<Task>();
            ReaderWriterLockSlim pLock = new ReaderWriterLockSlim();
            for (int count = 0; count < 9; count++)
            {
                tasks.Add(Task.Factory.StartNew(() => Process(pLock, e), TaskCreationOptions.LongRunning));
            }

            Task.WhenAll(tasks).Wait();

            DialogResult = DialogResult.OK;
        }

        private DocumentProperties GetNextUnprocessedRecord()
        {
            try
            {
                return DataAccessHelper.ExecuteSingle<DocumentProperties>(GetNextRecordSproc, XmlGenerator.EcorrFedCon);
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "UpdateZipFIleName")]
        private void Process(ReaderWriterLockSlim pLock, DoWorkEventArgs e)
        {
            string dir = EnterpriseFileSystem.GetPath(XmlPrepFolder);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!Directory.Exists(EnterpriseFileSystem.GetPath(XmlFolder)))
                Directory.CreateDirectory(EnterpriseFileSystem.GetPath(XmlFolder));

            dir = dir + Guid.NewGuid().ToBase64String();
            Directory.CreateDirectory(dir);
            string zipFileName = string.Format("{2}KU_IN_ZIP_{0:MMddyyhhmmss}_{1}.zip", DateTime.Now, Guid.NewGuid().ToBase64String(), DataAccessHelper.TestMode ? "TEST_" : "");

            DocumentProperties document = GetNextUnprocessedRecord();

            List<int> documentDetailIds = new List<int>();
            while (document != null)
            {
                new XmlGenerator().WriteXmlDoc(document, dir, zipFileName);
                backgroundWorker1.ReportProgress(1);
                int fileCount = Directory.GetFiles(dir).Count();
                documentDetailIds.Add(document.DocumentDetailsId);
                if (fileCount == 1000) //500 PDF 500 XML files
                {
                    string moveDir = dir.Substring(0, (dir.LastIndexOf(@"\") + 1));
                    Repeater.TryRepeatedly(() => ZipFile.CreateFromDirectory(dir, Path.Combine(EnterpriseFileSystem.GetPath(XmlPrepFolder), zipFileName), CompressionLevel.Optimal, false));
                    File.Move(Path.Combine(moveDir , zipFileName), Path.Combine(EnterpriseFileSystem.GetPath(XmlFolder), zipFileName));
                    Repeater.TryRepeatedly(() => Directory.Delete(dir, true));
                    DataAccessHelper.Execute("UpdateZipFIleName", DataAccessHelper.Database.ECorrFed, SqlParams.Single("DDIds", documentDetailIds.Select(p => new { Id = p }).ToList().ToDataTable()), SqlParams.Single("ZipFileName", zipFileName));
                    documentDetailIds = new List<int>();
                    dir = EnterpriseFileSystem.GetPath(XmlPrepFolder) + Guid.NewGuid().ToBase64String();
                    Directory.CreateDirectory(dir);
                    zipFileName = string.Format("KU_IN_ZIP_{0:MMddyyhhmmss}_{1}.zip", DateTime.Now, Guid.NewGuid().ToBase64String());
                }

                document = GetNextUnprocessedRecord();
            }

            if (Directory.GetFiles(dir).Count() > 1)
            {
                string moveDir = dir.Substring(0, (dir.LastIndexOf(@"\") + 1));
                Repeater.TryRepeatedly(() => ZipFile.CreateFromDirectory(dir, Path.Combine(EnterpriseFileSystem.GetPath(XmlPrepFolder), zipFileName), CompressionLevel.Optimal, false));
                File.Move(Path.Combine(moveDir, zipFileName), Path.Combine(EnterpriseFileSystem.GetPath(XmlFolder), zipFileName));
                DataAccessHelper.Execute("UpdateZipFIleName", DataAccessHelper.Database.ECorrFed, SqlParams.Single("DDIds", documentDetailIds.Select(p => new { Id = p }).ToList().ToDataTable()), SqlParams.Single("ZipFileName", zipFileName));
            }

            Repeater.TryRepeatedly(() => Directory.Delete(dir, true));

            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            NumberOfFilesProcessed++;
            this.Count.Text = ProgressCount;
            this.Refresh();
        }

        private void XmlProgressBar_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel the script?", "Cancel?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                backgroundWorker1.CancelAsync();
                backgroundWorker1.Dispose();
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
