using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace DEMUPDTFED
{
    class DataAccess
    {
        private string _loanServicingManagerId;

        [UsesSproc(DataAccessHelper.Database.Cls, "spDemographicUpdateGetArc")]
        public string GetArc(string queue, Demographics.Type demographicType, string rejectReason)
        {
            string typeString = "";
            switch (demographicType)
            {
                case Demographics.Type.Address:
                    typeString = "Address";
                    break;
                case Demographics.Type.Phone:
                    typeString = "Phone";
                    break;
                default:
                    Debug.Assert(false, string.Format("Unknown demographic type \"{0}\".", demographicType));
                    break;
            }

            return DataAccessHelper.ExecuteSingle<string>("spDemographicUpdateGetArc", DataAccessHelper.Database.Cls,
                queue.ToSqlParameter("Queue"), typeString.ToSqlParameter("DemographicType"), rejectReason.ToSqlParameter("RejectReason"));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "spDemographicUpdateGetComment")]
        public string GetComment(string rejectReason)
        {
            return DataAccessHelper.ExecuteSingle<string>("spDemographicUpdateGetComment", DataAccessHelper.Database.Cls, rejectReason.ToSqlParameter("RejectReason"));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "spDemographicUpdateGetSourceCode")]
        public string GetDemographicsSourceCode(string demographicsSource)
        {
            return DataAccessHelper.ExecuteSingle<string>("spDemographicUpdateGetSourceCode", DataAccessHelper.Database.Cls, demographicsSource.ToSqlParameter("SourceName"));
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "GetManagerOfBusinessUnit")]
        public string GetLoanServicingManagerId()
        {
            string buName = EnterpriseFileSystem.GetPath("DEMUPDTFED-BU");
            if (string.IsNullOrEmpty(_loanServicingManagerId))
            {
                _loanServicingManagerId = DataAccessHelper.ExecuteSingle<string>("GetManagerOfBusinessUnit", DataAccessHelper.Database.Bsys, buName.ToSqlParameter("BusinessUnit"));
            }
            return _loanServicingManagerId;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "spDemographicUpdateGetLocate")]
        public Locate GetLocate(string demographicsSource)
        {
            return DataAccessHelper.ExecuteSingle<Locate>("spDemographicUpdateGetLocate", DataAccessHelper.Database.Cls, demographicsSource.ToSqlParameter("SourceName"));
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "spDemographicUpdateGetQueues")]
        public List<string> GetQueues()
        {
            return DataAccessHelper.ExecuteList<string>("spDemographicUpdateGetQueues", DataAccessHelper.Database.Cls);
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "spDemographicUpdateGetSourceCodes")]
        internal List<string> GetSourceCodes()
        {
            return DataAccessHelper.ExecuteList<string>("spDemographicUpdateGetSourceCodes", DataAccessHelper.Database.Cls);
        }
    }
}