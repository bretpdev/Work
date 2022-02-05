using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace LetterTrackingDataTransfer
{
    public class Process
    {
        public DataAccess DA { get; set; }
        public TransferData TD { get; set; }
        public string EcorrFedSproc { get; set; }
        public string SystemLetterSproc { get; set; }

        public Process()
        {
            DA = new DataAccess();
            TD = new TransferData(this);
        }

        public void Start()
        {
            EcorrFedSproc = string.Format(@"{0}\{1}_EcorrFed.sql", EnterpriseFileSystem.TempFolder, TD.LetterId.Text);
            SystemLetterSproc = string.Format(@"{0}\{1}_SystemLetter.sql", EnterpriseFileSystem.TempFolder, TD.LetterId.Text);
            if (TD.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
        }

        public bool AddLetterData()
        {
            string id = ((CentralPrintingDocData)TD.LetterId.SelectedItem).ID;
            CentralPrintingDocData docData = DA.GetCentralPrintingDocData(id);
            DocDetail docDetail = DA.GetDocDetail(id);
            docDetail.DocSeqNo = DA.InsertCentralPrintingDocData(docData);
            if (docDetail.DocSeqNo == 0)
                return false;
            DA.InsertDocDetail(docDetail);
            return true;
        }

        public void AddLetterToEcorr()
        {
            if (TD.AddEcorr.Checked)
            {
                DA.InsertEcorrLetter(TD.LetterId.Text);
                if (TD.GenerateEcorr.Checked)
                    CreateEcorrSproc();
            }

            if (TD.AddLtdb.Checked)
            {
                DA.InsertLetterTrackingData(TD.LetterId.Text);
                if (TD.GenerateLtdb.Checked)
                    CreateLetterTrackingSproc();
            }
        }

        public void CreateEcorrSproc()
        {
            using (StreamWriter sw = new StreamWriter(EcorrFedSproc, false))
            {
                sw.WriteLine("USE ECorrFed");
                sw.WriteLine("GO");
                sw.WriteLine("");
                sw.Write(string.Format("IF NOT EXISTS(SELECT Letter FROM Letters WHERE Letter = '{0}')", TD.LetterId.Text));
                sw.WriteLine("INSERT INTO ECorrFed.dbo.letters(Letter, LetterTypeId, DocId, Viewable, ReportDescription, ReportName, Viewed, MainframeRegion, SubjectLine, DocSource, DocComment, WorkFlow, DocDelete, Active))");
                sw.WriteLine(string.Format("VALUES('{0}',1,'XELT','Y','Description','Name','N','VUK1','Important Information Regarding Your Account','IMPORT','Important Update from CornerStone!','N','N',1)", TD.LetterId.Text));
            }
        }

        private void CreateLetterTrackingSproc()
        {
            using (StreamWriter sw = new StreamWriter(SystemLetterSproc, false))
            {
                sw.WriteLine("USE BSYS");
                sw.WriteLine("GO");
                sw.WriteLine("");
                sw.WriteLine(string.Format("DECLARE @ID VARCHAR(10) = '{0}'", TD.LetterId.Text));
                sw.WriteLine("DECLARE @LetterId INT = (SELECT [DocDetailId] FROM [dbo].[LTDB_DAT_DocDetail] WHERE ID = @ID)");
                if (TD.HasLoanDetail.Checked)
                {
                    sw.WriteLine("DECLARE @RET_STRING VARCHAR(255)");
                sw.WriteLine("");
                    sw.WriteLine("EXEC xp_sprintf @RET_STRING OUTPUT, 'LT_%s_Loans', @ID");
                }
                sw.WriteLine("");
                sw.WriteLine("IF NOT EXISTS(SELECT SystemLettersStoredProcedureId FROM LTDB_SystemLettersStoredProcedures where LetterId = @LetterId)");
                sw.WriteLine("INSERT INTO LTDB_SystemLettersStoredProcedures(LetterId, StoredProcedureName, ReturnTypeId)");
                sw.WriteLine("VALUES(@LetterId, 'LT_Header', 1)");
                if (TD.HasLoanDetail.Checked)
                    sw.WriteLine(",\r\n(@LetterId, @RET_STRING, 2)");
            }
        }
    }
}