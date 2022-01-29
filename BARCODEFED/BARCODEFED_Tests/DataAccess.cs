using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BARCODEFED_Tests
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        public string GetAccountInCA()
        {
            return LDA.ExecuteSingle<string>("SELECT TOP 1 PD10.DF_SPE_ACC_ID FROM UDW..PD10_PRS_NME PD10 INNER JOIN PD30_PRS_ADR PD30 ON PD10.DF_PRS_ID = PD30.DF_PRS_ID WHERE PD30.DC_DOM_ST = 'CA'", DataAccessHelper.Database.Udw).Result;
        }
    }
}