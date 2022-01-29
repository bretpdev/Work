using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLoggerRC;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace RCDIALER
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) => LDA = lda;

        [UsesSproc(Voyager, "rcdialer.GetSprocs")]
        public List<Sprocs> GetSprocs() =>
            LDA.ExecuteList<Sprocs>("rcdialer.GetSprocs", Voyager,
                SP("DAY", DateTime.Now.DayOfWeek.ToString())).CheckResult();

        [UsesSproc(Voyager, "rcdialer.Load30DayCalls")]
        [UsesSproc(Voyager, "rcdialer.Load60DayCalls")]
        [UsesSproc(Voyager, "rcdialer.Load90DayCalls")]
        [UsesSproc(Voyager, "rcdialer.Load270DayCalls")]
        public void LoadCalls(string sprocName)
        {
            int startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday).Day;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            if (startOfWeek > DateTime.Now.Day)
                --month; //Subtract 1 from the month if the Monday was in the last month
            if (month < 1)
            {
                --year; //Subtract one from the year if the first of the week was in the previous month of the previous year
                month = 1;
            }
            DateTime monday = new DateTime(year, month, startOfWeek, 9, 0, 0);
            LDA.Execute(sprocName, Voyager,
                  SP("MONDAY", monday));
        }

        [UsesSproc(Voyager, "rcdialer.GetUnprocessedRecords")]
        public List<DialerData> GetDialerData() =>
            LDA.ExecuteList<DialerData>("rcdialer.GetUnprocessedRecords", Voyager).Result;

        [UsesSproc(Voyager, "rcdialer.SetProcessedAt")]
        public void SetProcessedAt(int outboundCallsId) => 
            LDA.Execute("rcdialer.SetProcessedAt", Voyager,
                SqlParams.Single("OutboundCallsId", outboundCallsId));

        public SqlParameter SP(string name, object value) => SqlParams.Single(name, value);
    }
}