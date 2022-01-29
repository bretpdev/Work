using System;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace INCCOLREV
{
    class DataAccess
    {
        public static string DUE_DILIGENCE_1 = "DUE_DIL_1";
        public static string DUE_DILIGENCE_2 = "DUE_DIL_2";
        public static string DUE_DILIGENCE_3 = "DUE_DIL_3";

        private static string dueDiligenceDefaultedText1;
        public static string DueDiligenceDefaultedText1
        {
            get
            {
                if (dueDiligenceDefaultedText1.IsNullOrEmpty())
                    dueDiligenceDefaultedText1 = GetText(DUE_DILIGENCE_1, "Defaulted");
                return dueDiligenceDefaultedText1;
            }
        }

        private static string dueDiligenceDefaultedText2;
        public static string DueDiligenceDefaultedText2
        {
            get
            {
                if (dueDiligenceDefaultedText2.IsNullOrEmpty())
                    dueDiligenceDefaultedText2 = GetText(DUE_DILIGENCE_2, "Defaulted");
                return dueDiligenceDefaultedText2;
            }
        }

        private static string dueDiligenceDefaultedText3;
        public static string DueDiligenceDefaultedText3
        {
            get
            {
                if (dueDiligenceDefaultedText3.IsNullOrEmpty())
                    dueDiligenceDefaultedText3 = GetText(DUE_DILIGENCE_3, "Defaulted");
                return dueDiligenceDefaultedText3;
            }
        }
        
        private static string dueDiligenceRepaymentText1;
        public static string DueDiligenceRepaymentText1
        {
            get 
            {
                if (dueDiligenceRepaymentText1.IsNullOrEmpty())
                    dueDiligenceRepaymentText1 = GetText(DUE_DILIGENCE_1, "Repayment");
                return dueDiligenceRepaymentText1;
            }
        }

        private static string dueDiligenceRepaymentText2;
        public static string DueDiligenceRepaymentText2
        {
            get
            {
                if (dueDiligenceRepaymentText2.IsNullOrEmpty())
                    dueDiligenceRepaymentText2 = GetText(DUE_DILIGENCE_2, "Repayment");
                return dueDiligenceRepaymentText2;
            }
        }

        private static string dueDiligenceRepaymentText3;
        public static string DueDiligenceRepaymentText3
        {
            get
            {
                if (dueDiligenceRepaymentText3.IsNullOrEmpty())
                    dueDiligenceRepaymentText3 = GetText(DUE_DILIGENCE_3, "Repayment");
                return dueDiligenceRepaymentText3;
            }
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "GetLetterText")]
        private static string GetText(string mergeField, string condition)
        {
            return DataAccessHelper.ExecuteSingle<string>("GetLetterText", DataAccessHelper.Database.Bsys,
                new SqlParameter("MergeFieldName", mergeField), new SqlParameter("Condition", condition));
        }

    }//class
}//namespace
