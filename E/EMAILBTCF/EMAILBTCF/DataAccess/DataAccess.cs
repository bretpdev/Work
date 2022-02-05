using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace EMAILBTCF
{
    public class DataAccess
    {
        private LogDataAccess lda;
        const DataAccessHelper.Database cls = DataAccessHelper.Database.Cls;
        public DataAccess(LogDataAccess lda)
        {
            this.lda = lda;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.GetEmailCampaigns")]
        public List<EmailCampaign> GetAllCampaigns()
        {
            return lda.ExecuteList<EmailCampaign>("emailbtcf.GetEmailCampaigns", cls).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.UploadCampaignData")]
        public void UploadCampaignData(int emailCampaignId, List<CampaignDataForUpload> borrowers)
        {
            lda.Execute("emailbtcf.UploadCampaignData", cls, Sp("EmailCampaignId", emailCampaignId), Sp("CampaignData", borrowers.ToDataTable()));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.GetCampaignData")]
        public List<CampaignData> GetCampaignData(int emailCampaignId)
        {
            return lda.ExecuteList<CampaignData>("emailbtcf.GetCampaignData", cls, Sp("EmailCampaignId", emailCampaignId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.MarkCampaignDataEmailSent")]
        public void MarkCampaignDataEmailSent(int campaignDataId)
        {
            lda.Execute("emailbtcf.MarkCampaignDataEmailSent", cls, Sp("CampaignDataId", campaignDataId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.MarkCampaignDataArcProcessed")]
        public void MarkCampaignDataArcProcessed(int campaignDataId, int? arcAddProcessingId)
        {
            lda.Execute("emailbtcf.MarkCampaignDataArcProcessed", cls, Sp("CampaignDataId", campaignDataId), Sp("ArcAddProcessingId", arcAddProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.MarkLineDataEmailSent")]
        public void MarkLineDataEmailSent(int lineDataId)
        {
            lda.Execute("emailbtcf.MarkLineDataEmailSent", cls, Sp("LineDataId", lineDataId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.MarkLineDataArcProcessed")]
        public void MarkLineDataArcProcessed(int lineDataId, int? arcAddProcessingId)
        {
            lda.Execute("emailbtcf.MarkLineDataArcProcessed", cls, Sp("LineDataId", lineDataId), Sp("ArcAddProcessingId", arcAddProcessingId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.GetLineData")]
        public List<LineData> GetLineData(int emailCampaignId)
        {
            return lda.ExecuteList<LineData>("emailbtcf.GetLineData", cls, Sp("EmailCampaignId", emailCampaignId)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "emailbtcf.GetMergeFieldMapping")]
        public List<MergeField> GetMergeFieldMapping(int emailCampaignId)
        {
            return lda.ExecuteList<MergeField>("emailbtcf.GetMergeFieldMapping", cls, Sp("EmailCampaignId", emailCampaignId)).Result;
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
    }
}
