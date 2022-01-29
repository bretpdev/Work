using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace TEXTCOORD
{
    public class DataAccess
    {
        private LogDataAccess LDA { set; get; }


        public DataAccess(LogDataAccess lda)
        {
            LDA = lda;
        }

        [UsesSproc(Uls, "textcoord.InsertApp1")]
        public bool InsertApp1Data() => LDA.Execute("textcoord.InsertApp1", Uls);

        [UsesSproc(Uls, "textcoord.GetCampaigns")]
        public List<Campaigns> GetCampaigns()
        {
            var results = LDA.ExecuteList<Campaigns>("textcoord.GetCampaigns", Uls).Result;
            foreach (var campaign in results)
                campaign.DisabledUiFields = LDA.ExecuteList<CampaignDisabledUiField>("textcoord.GetCampaignDisabledUiFields", Uls, Sp("CampaignId", campaign.CampaignId)).Result;
            return results;
        }

        public List<ExcelData> Search(string sproc, int numberToSend, string campaign, int lowerRange = 0, int upperRange = 0, int lowerAge = 0, int upperAge = 0, List<Ranks> segment = null, List<Ranks> category = null)
        {
            return LDA.ExecuteList<ExcelData>(sproc, Uls,
                Sp("NumberToSend", numberToSend),
                Sp("DelqLower", lowerRange),
                Sp("DelqUpper", upperRange),
                Sp("AgeLower", lowerAge),
                Sp("AgeUpper", upperAge),
                Sp("ContentType", campaign),
                Sp("Segment", segment.ToDataTable()),
                Sp("PerformanceCategory", category.ToDataTable())).Result;
        }

        [UsesSproc(Uls, "textcoord.SetExclusions")]
        public void InsertExclusions(List<ExcelData> data)
        {
            if (DataAccessHelper.TestMode)
            {
                bool result = LDA.Execute("textcoord.SetExclusions", Uls, Sp("SearchResults", data.ToDataTable()));
                if (!result)
                {
                    Dialog.Error.Ok("These was an error adding the exclusion data to Noble.  Please contract Systems Support.");
                    Environment.Exit(1);
                }
            }
            else
                BuildAndExecuteNobleSQL(data.Select(p => p.AccountNumber).ToList());
        }

        [UsesSproc(Uls, "textcoord.ExportFile")]
        public List<FileData> InsertTrackingGetResults(List<ExcelData> data, string campaign) =>
            LDA.ExecuteList<FileData>("textcoord.ExportFile", Uls, Sp("SearchResults", data.ToDataTable()),
                Sp("ContentType", campaign)).Result;

        private string GetConnString()
        {
            return ConfigurationManager.ConnectionStrings["app1"].ConnectionString;
        }

        /// <summary>
        /// Connect to Noble's server and set a list of accounts to not call.  In line code is needed as we dont want to store the UN/PW on the database for this one
        /// </summary>
        /// <param name="accts"></param>
        private void BuildAndExecuteNobleSQL(List<string> accts)
        {
            int skipVal = 0;
            int takeVal = (accts.Count - skipVal > 1000 ? 1000 : accts.Count - skipVal);

            List<string> workingSet = new List<string>();
            for (; takeVal > 0 && skipVal < accts.Count; skipVal += takeVal)
            {
                workingSet.Clear();
                takeVal = accts.Count - skipVal > 1000 ? 1000 : accts.Count - skipVal;
                workingSet = accts.Skip(skipVal).Take(takeVal).ToList();
                StringBuilder sb = new StringBuilder("insert into cs_exclusion_acct (filler_2,add_date) values ");
                string currentDate = System.DateTime.Now.ToShortDateString();
                for (int index = 0; index <= workingSet.Count - 1; index++)
                {
                    if (index == workingSet.Count - 1 || workingSet.Count == 1)
                        sb.Append($"('{workingSet[index]}','{currentDate}');");
                    else
                        sb.Append($"('{workingSet[index]}','{currentDate}'),");
                }
                string sql = sb.ToString();
                ExecuteNobleSQL(sql);
            }
        }

        /// <summary>
        /// Uses a posgres connection to run a query on Nobles posgres database.
        /// </summary>
        /// <param name="sql"></param>
        [UsesSproc(BatchProcessing, "spGetDecrpytedPassword")]
        private void ExecuteNobleSQL(string sql)
        {
            if (DataAccessHelper.TestMode)
                return;
            string connectionString = GetConnString();
            connectionString = string.Format(connectionString, LDA.ExecuteSingle<string>("spGetDecrpytedPassword", BatchProcessing, Sp("UserId", "App1")).Result);
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteReader();
            conn.Close();
        }

        private SqlParameter Sp(string name, object val) => SqlParams.Single(name, val);
    }
}