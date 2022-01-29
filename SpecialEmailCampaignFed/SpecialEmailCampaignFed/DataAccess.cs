using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Windows.Forms;
using Q;

namespace SpecialEmailCampaignFed
{

    class DataAccess : DataAccessBase
    {
        private readonly DataContext _cls;
        private readonly DataContext _cdw;

        public DataAccess(bool testMode)
        {
            _cls = ClsDataContext(testMode);
            _cdw = CDWDataContext(testMode);
        }

        public IEnumerable<CampaignData> GetEmailCampigns()
        {
            return _cls.ExecuteQuery<CampaignData>(@"EXEC spEMCPGetEmailCampaigns").ToList();
        }

        public IEnumerable<CampaignData> GetCampaignData(int campId)
        {
            return _cls.ExecuteQuery<CampaignData>(@"EXEC spEMCPGetCampaignData {0}", campId).ToList();
        }

        public bool Save(bool newCampaign, CampaignData cData, bool CornerStone)
        {
            if (newCampaign)
            {
                _cls.ExecuteCommand(@"EXEC spEMCPSaveSpecialEmailCampaign {0},{1},{2},{3},{4},{5},{6},{7},{8}", cData.EmailSubjectLine, cData.CornerStone, cData.IncludeAccountNumber, cData.Arc, cData.CommentText, cData.DataFile, cData.HTMLFile, cData.EmailFrom, DateTime.Now);
                MessageBox.Show("Campaign has been saved.");
            }
            else
            {
                _cls.ExecuteCommand(@"EXEC spEMCPUpdateSpecialEmailCampaign {0},{1},{2},{3},{4},{5},{6},{7},{8}", cData.CampID, cData.EmailSubjectLine, cData.CornerStone, cData.IncludeAccountNumber, cData.Arc, cData.CommentText, cData.DataFile, cData.HTMLFile, cData.EmailFrom);
                MessageBox.Show("Campaign has been updated.");
            }

            return true;

        }

        public long CreateAndReturnRecipId(BorrowerDetails fileData, CampaignData cData)
        {
            _cls.ExecuteCommand(@"EXEC spEMCPUpdateEmailId {0},{1},{2}", fileData.AccountNumber, cData.CampID, fileData.EmailAddress);
            return _cls.ExecuteQuery<long>(@"EXEC spEMCPGetEmailId {0},{1},{2}", fileData.AccountNumber, cData.CampID, fileData.EmailAddress).Last();
        }

        public string GetSsnFromAcctNum(string accountNUmber)
        {
            return _cdw.ExecuteQuery<string>(@"EXEC spGetSSNFromAcctNumber {0}", accountNUmber).SingleOrDefault();
        }

        public long GetCampaignId(CampaignData cData)
        {
            return _cls.ExecuteQuery<long>("EXEC spEMCPGetCampId {0},{1},{2},{3},{4},{5},{6},{7}", cData.EmailSubjectLine, cData.CornerStone, cData.IncludeAccountNumber, cData.Arc, cData.CommentText, cData.DataFile, cData.HTMLFile, cData.EmailFrom).SingleOrDefault();
        }
    }
}
