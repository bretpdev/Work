using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace PEPSFED
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        public DataAccess()
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, Program.PLR.ProcessLogId, false, true);
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].GetAffiliationData")]
        public List<AffiliationData> GetAffiliationData()
        {
            return LDA.ExecuteList<AffiliationData>("[peps].GetAffiliationData", DataAccessHelper.Database.Cls).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].GetClosureData")]
        public List<ClosureData> GetClosureData()
        {
            return LDA.ExecuteList<ClosureData>("[peps].GetClosureData", DataAccessHelper.Database.Cls).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].GetContactData")]
        public List<ContactData> GetContactData()
        {
            return LDA.ExecuteList<ContactData>("[peps].GetContactData", DataAccessHelper.Database.Cls).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].GetDetailData")]
        public List<DetailData> GetDetailData()
        {
            return LDA.ExecuteList<DetailData>("[peps].GetDetailData", DataAccessHelper.Database.Cls).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].GetIdentiferData")]
        public List<IdentiferData> GetIdentiferData()
        {
            return LDA.ExecuteList<IdentiferData>("[peps].GetIdentiferData", DataAccessHelper.Database.Cls).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].GetOtherAddressData")]
        public List<OtherAddressData> GetOtherAddressData()
        {
            return LDA.ExecuteList<OtherAddressData>("[peps].GetOtherAddressData", DataAccessHelper.Database.Cls).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].GetProgramData")]
        public List<ProgramData> GetProgramData()
        {
            return LDA.ExecuteList<ProgramData>("[peps].GetProgramData", DataAccessHelper.Database.Cls).CheckResult();
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].UpdateAffiliationProcessed")]
        public void UpdateAffiliationProcessed(long recordId)
        {
            LDA.Execute("[peps].UpdateAffiliationProcessed", DataAccessHelper.Database.Cls, SqlParams.Single("RecordId", recordId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].UpdateClosureProcessed")]
        public void UpdateClosureProcessed(long recordId)
        {
            LDA.Execute("[peps].UpdateClosureProcessed", DataAccessHelper.Database.Cls, SqlParams.Single("RecordId", recordId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].UpdateContactProcessed")]
        public void UpdateContactProcessed(long recordId)
        {
            LDA.Execute("[peps].UpdateContactProcessed", DataAccessHelper.Database.Cls, SqlParams.Single("RecordId", recordId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].UpdateDetailProcessed")]
        public void UpdateDetailProcessed(long recordId)
        {
            LDA.Execute("[peps].UpdateDetailProcessed", DataAccessHelper.Database.Cls, SqlParams.Single("RecordId", recordId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].UpdateIdentifierProcessed")]
        public void UpdateIdentifierProcessed(long recordId)
        {
            LDA.Execute("[peps].UpdateIdentifierProcessed", DataAccessHelper.Database.Cls, SqlParams.Single("RecordId", recordId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].UpdateOtherAddressProcessed")]
        public void UpdateOtherAddressProcessed(long recordId)
        {
            LDA.Execute("[peps].UpdateOtherAddressProcessed", DataAccessHelper.Database.Cls, SqlParams.Single("RecordId", recordId));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "[peps].UpdateProgramProcessed")]
        public void UpdateProgramProcessed(long recordId)
        {
            LDA.Execute("[peps].UpdateProgramProcessed", DataAccessHelper.Database.Cls, SqlParams.Single("RecordId", recordId));
        }
    }
}
