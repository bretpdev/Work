using System;
using System.Text;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;


namespace ACS.Infrastructure
{
    public class Name_Spec
    {
        [DbName("DM_PRS_1")]
        public string FirstName { get; set; }
        [DbName("DM_PRS_LST")]
        public string LasName { get; set; }
        [DbName("DF_SPE_ACC_ID")]
        public string AccountNumber { get; set; }
    }
}
