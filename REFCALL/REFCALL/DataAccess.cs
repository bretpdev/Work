using System;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace REFCALL
{
    public class DataAccess
    {
        public ProcessLogRun LogRun { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LogRun = logRun;
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "[spGENRGetLenderAffiliation]")]
        public bool IsAffiliatedLenderCode(string lenderCode)
        {
            return LogRun.LDA.ExecuteSingle<string>("[spGENRGetLenderAffiliation]", DataAccessHelper.Database.Bsys,
                SqlParams.Single("LenderId", lenderCode)).Result == "UHEAA";
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "Get_BU_Manager")]
        public string GetManagerId()
        {
            return LogRun.LDA.ExecuteSingle<Manager>("Get_BU_Manager", DataAccessHelper.Database.Csys,
                SqlParams.Single("BusinessUnitName", "Loan Management")).Result.AesUserId;
        }
    }

    public class Manager
    {
        public string AesUserId { get; set; }
    }
}