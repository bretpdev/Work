using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Q;

namespace FUTRQUEFED
{
    class DataAccess : DataAccessBase
    {
        //check for duplicate requests
        public static bool HasDuplicateRequest(bool testMode, QueueInfo queInfo)
        {
            SprocCommandBuilder sp = new SprocCommandBuilder("spArcAddCheckForDuplicates");
            sp.AddParameter("@AccountNumber", queInfo.AccountNumber);
            sp.AddParameter("@RecipientId", queInfo.RecipientId);
            sp.AddParameter("@Arc", queInfo.Arc);
            sp.AddParameter("@ArcAddDate", queInfo.ArcAddDate);
            if (ClsDataContext(testMode).ExecuteQuery<int>(sp.Command, sp.ParameterValues).SingleOrDefault() > 0)
            {
                return true;
            }
            return false;
        }// public static bool HasDuplicateRequest

        //add record
        public static string AddRequest(bool testMode, QueueInfo queInfo)
        {
            try
            {
                SprocCommandBuilder sp = new SprocCommandBuilder("spArcAddAddRecord");
                sp.AddParameter("AccountNumber", queInfo.AccountNumber);
                sp.AddParameter("RecipientId", queInfo.RecipientId);
                sp.AddParameter("Arc", queInfo.Arc);
                sp.AddParameter("ArcAddDate", queInfo.ArcAddDate);
                sp.AddParameter("Comment", queInfo.Comment);
                sp.AddParameter("RequestedDate", DateTime.Today);
                sp.AddParameter("UserId", Environment.UserName);

                ClsDataContext(testMode).ExecuteCommand(sp.Command, sp.ParameterValues);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }//public static string AddRequest
    }
}
