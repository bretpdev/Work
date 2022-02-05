using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace LetterTrackingDataTransfer
{
    public class DataAccess
    {
        public List<CentralPrintingDocData> GetLiveLetterIds(DataAccessHelper.Mode mode = DataAccessHelper.Mode.Live)
        {
            DataAccessHelper.CurrentMode = mode;
            return DataAccessHelper.ExecuteList<CentralPrintingDocData>("GetCentralizedPrintingDocData", DataAccessHelper.Database.Bsys);
        }

        public CentralPrintingDocData GetCentralPrintingDocData(string id)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            return DataAccessHelper.ExecuteSingle<CentralPrintingDocData>("GetCentralPrintingDocData", DataAccessHelper.Database.Bsys,
                SqlParams.Single("ID", id));
        }

        public DocDetail GetDocDetail(string id)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            return DataAccessHelper.ExecuteSingle<DocDetail>("GetDocDetail", DataAccessHelper.Database.Bsys,
                SqlParams.Single("ID", id));
        }

        public int InsertCentralPrintingDocData(CentralPrintingDocData docData)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            try
            {
                return DataAccessHelper.ExecuteSingle<int>("InsertCentralPrintingDocData", DataAccessHelper.Database.Bsys,
                    SqlParams.Single("ID", docData.ID),
                    SqlParams.Single("Pages", docData.Pages),
                    SqlParams.Single("Instructions", docData.Instructions ?? ""),
                    SqlParams.Single("Duplex", docData.Duplex),
                    SqlParams.Single("UHEAACostCenter", docData.UHEAACostCenter ?? ""),
                    SqlParams.Single("Path", docData.Path ?? ""),
                    SqlParams.Single("ResendMail", docData.ResendMail));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return 0;
        }

        public void InsertDocDetail(DocDetail docDetail)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            try
            {
                DataAccessHelper.Execute("InsertDocDetail", DataAccessHelper.Database.Bsys,
                    SqlParams.Single("DocName", docDetail.DocName),
                    SqlParams.Single("DocSeqNo", docDetail.DocSeqNo),
                    SqlParams.Single("DocTyp", docDetail.DocTyp ?? ""),
                    SqlParams.Single("Status", docDetail.Status ?? ""),
                    SqlParams.Single("ID", docDetail.ID ?? ""),
                    SqlParams.Single("Code", docDetail.Code ?? ""),
                    SqlParams.Single("Description", docDetail.Description ?? ""),
                    SqlParams.Single("Letterhead", docDetail.Letterhead ?? ""),
                    SqlParams.Single("Model", docDetail.Model ?? ""),
                    SqlParams.Single("ActCd", docDetail.ActCd ?? ""),
                    SqlParams.Single("ARC", docDetail.ARC ?? ""),
                    SqlParams.Single("DocID", docDetail.DocID ?? ""),
                    SqlParams.Single("CostCd", docDetail.CostCd ?? ""),
                    SqlParams.Single("Addressee", docDetail.Addressee ?? ""),
                    SqlParams.Single("Regarding", docDetail.Regarding ?? ""),
                    SqlParams.Single("Recip", docDetail.Recip ?? ""),
                    SqlParams.Single("AltRecip", docDetail.AltRecip ?? ""),
                    SqlParams.Single("Unit", docDetail.Unit ?? ""),
                    SqlParams.Single("ACSParticipant", docDetail.ACSParticipant ?? ""),
                    SqlParams.Single("LPD", docDetail.LPD ?? ""),
                    SqlParams.Single("Addresse", docDetail.Addresse ?? ""),
                    SqlParams.Single("OtherAddressee", docDetail.OtherAddresse ?? ""),
                    SqlParams.Single("Citation", docDetail.Citation ?? ""),
                    SqlParams.Single("ReqLang", docDetail.ReqLang ?? ""),
                    SqlParams.Single("Path_old", docDetail.Path_old ?? ""),
                    SqlParams.Single("BCPCriticality", docDetail.BCPCriticality ?? ""),
                    SqlParams.Single("Compliance", docDetail.Compliance),
                    SqlParams.Single("ClosingParagraph", docDetail.ClosingParagraph ?? ""));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void InsertEcorrLetter(string letterId)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            //Check if the letter is already in the table before trying to insert it
            if (DataAccessHelper.ExecuteList<string>("CheckIfLetterExists", DataAccessHelper.Database.ECorrFed,
                SqlParams.Single("Letter", letterId)).Count == 0)
            {
                DataAccessHelper.Execute("InsertLetter", DataAccessHelper.Database.ECorrFed,
                    SqlParams.Single("Letter", letterId),
                    SqlParams.Single("LetterTypeId", 1),
                    SqlParams.Single("DocId", "XELT"),
                    SqlParams.Single("Viewable", "Y"),
                    SqlParams.Single("ReportDescription", "Description"),
                    SqlParams.Single("ReportName", "Name"),
                    SqlParams.Single("Viewed", "N"),
                    SqlParams.Single("MainframeRegion", "VUK1"),
                    SqlParams.Single("SubjectLine", "Important Information Regarding Your Account"),
                    SqlParams.Single("DocSource", "IMPORT"),
                    SqlParams.Single("DocComment", "Important Update from CornerStone!"),
                    SqlParams.Single("WorkFlow", "N"),
                    SqlParams.Single("DocDelete", "N"),
                    SqlParams.Single("Active", 1));
            }
        }

        public void InsertLetterTrackingData(string p)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
        }
    }
}