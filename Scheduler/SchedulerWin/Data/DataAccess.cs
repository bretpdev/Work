using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace SchedulerWeb
{
	public class DataAccess
	{
        private LogDataAccess lda;
        public DataAccess(LogDataAccess lda)
        {
            this.lda = lda;
        }
		/// <summary>
		/// This should pull back all sacker/letterTracking requests that have not yet been completed
		/// </summary>
		/// <returns>List of Sacker request objects with information about the time estimates and projected start and end dates of the task</returns>
		[UsesSproc(DB.Scheduler, "GetSackerRequests")]
		public List<SackerData> GetSackerRequests()
		{
			return lda.ExecuteList<SackerData>("GetSackerRequests", DB.Scheduler).Result;
		}

		/// <summary>
		/// Hits the database, returning the priority of the specific request.
		/// </summary>
		/// <param name="request">SackerData object to search for its priority</param>
		/// <returns>integer representing the priority of the request.</returns>
		[UsesSproc(DB.Scheduler, "GetRequestPriority")]
		public int? GetRequestPriority(SackerData request)
		{
			return lda.ExecuteSingle<int>("GetRequestPriority", DB.Scheduler, Sp("RequestType", request.RequestType), Sp("RequestId", request.RequestId)).Result;
		}

		/// <summary>
		/// Hits the Database to get the productivity constant of either a developer or tester
		/// </summary>
		/// <returns>constant value for productive hours in the day</returns>
		[UsesSproc(DB.Scheduler, "GetProductivityValues")]
		public Productivity GetProductivityValues()
		{
			return lda.ExecuteSingle<Productivity>("GetProductivityValues", DB.Scheduler).Result;
		}

		[UsesSproc(DB.Scheduler, "GetDefaultProgrammer")]
		public string GetDefaultProgrammer()
		{
			return lda.ExecuteSingle<string>("GetDefaultProgrammer", DB.Scheduler).Result;
		}

        [UsesSproc(DB.Scheduler, "GetDefaultTester")]
        public string GetDefaultTester()
        {
            return lda.ExecuteSingle<string>("GetDefaultTester", DB.Scheduler).Result;
        }

        /// <summary>
        /// Hits the database and returns the number of off work holidays between now and (convertedDays) days from now.
        /// </summary>
        /// <param name="requestStartDate">gives start date</param>
        /// <param name="remainingDays"># of days to look ahead for holidays</param>
        /// <returns># of off work holidays between start date and (remainingDays) days from now</returns>
        [UsesSproc(DB.Scheduler, "GetHolidays")]
		public int GetHolidays(DateTime requestStartDate, double remainingDays)
		{
			return lda.ExecuteSingle<int>("GetHolidays", DB.Scheduler, Sp("StartDate", requestStartDate), Sp("Days", remainingDays)).Result;
		}

		/// <summary>
		/// Hits the database and returns the number of weekend days between now and (convertedDays) days from now.
		/// </summary>
		/// <param name="requestStartDate">gives start date</param>
		/// <param name="remainingDays"># of days to look ahead for weekends</param>
		/// <returns># of weekend days between now and (remainingDays) days from now</returns>
		[UsesSproc(DB.Scheduler, "GetWeekendDays")]
		public int GetWeekendDays(DateTime requestStartDate, double remainingDays)
		{
			return lda.ExecuteSingle<int>("GetWeekendDays", DB.Scheduler, Sp("StartDate", requestStartDate), Sp("Days", remainingDays)).Result;
		}

		/// <summary>
		/// Takes an employee name and determines how many days worth of time off that employee has during the time frame.
		/// </summary>
		/// <param name="requestStartDate">gives start date</param>
		/// <param name="assignee">Employee name</param>
		/// <returns># of days off the person has during the next (remainingDays) number of days</returns>
		[UsesSproc(DB.Scheduler, "GetTimeOff")]
		public List<DateTime> GetTimeOff(DateTime requestStartDate, string assignee)
		{
			return lda.ExecuteList<DateTime>("GetTimeOff", DB.Scheduler, Sp("StartDate", requestStartDate), Sp("Employee", assignee)).Result;
		}

		/// <summary>
		/// Hits the database and updates a row into our table that is refreshed hourly 
		/// </summary>
		/// <param name="request">Request to look up</param>
		[UsesSproc(DB.Scheduler, "SetEstimatedCompletionDates")]
		public void SetEstimatedCompletionDates(SackerData request)
		{
			lda.Execute("SetEstimatedCompletionDates", DB.Scheduler, 
                Sp("DevStartDate", request.DevStartDate), Sp("DevEndDate", request.DevEndDate), Sp("TestStartDate", request.TesterStartDate), Sp("TestEndDate", request.TesterEndDate), 
                Sp("RequestId", request.RequestId), Sp("RequestTypeId", request.RequestTypeId));			
		}

		/// <summary>
		/// Hits the database and adds a row to the requestPriority table with the request for requests that dont have an entry.
		/// </summary>
		/// <param name="request">request to update values for</param>
		/// <returns>request priority of inserted request</returns>
		[UsesSproc(DB.Scheduler, "SetDefaultPriority")]
		public int SetDefaultPriority(SackerData request)
		{
			return lda.ExecuteSingle<int>("SetDefaultPriority", DB.Scheduler, Sp("Request", request.RequestId), Sp("RequestType", request.RequestType)).Result;			
		}

		/// <summary>
		/// Removes completed requests from the priority scheduler
		/// </summary>
		[UsesSproc(DB.Scheduler, "RemoveCompletedRequests")]
		public void RemoveCompletedRequests()
		{
			lda.Execute("RemoveCompletedRequests", DB.Scheduler);
		}

        [UsesSproc(DB.Bsys, "scheduler.GetSasData")]
        public List<SackerCache> GetSasData()
        {
            return lda.ExecuteList<SackerCache>("scheduler.GetSasData", DB.Bsys).Result;
        }

        [UsesSproc(DB.Bsys, "scheduler.GetScriptData")]
        public List<SackerCache> GetScriptData()
        {
            return lda.ExecuteList<SackerCache>("scheduler.GetScriptData", DB.Bsys).Result;
        }


        [UsesSproc(DB.Bsys, "scheduler.GetLetterData")]
        public List<SackerCache> GetLetterData()
        {
            return lda.ExecuteList<SackerCache>("scheduler.GetLetterData", DB.Bsys).Result;
        }

        [UsesSproc(DB.Scheduler, "GetSackerCache")]
        public List<SackerCache> GetSackerCache()
        {
            return lda.ExecuteList<SackerCache>("GetSackerCache", DB.Scheduler).Result;
        }

        [UsesSproc(DB.Scheduler, "AddSackerCache")]
        public void AddSackerCache(SackerCache record)
        {
            lda.Execute("AddSackerCache", DB.Scheduler,
                Sp("RequestTypeId", record.RequestTypeId),
                Sp("Name", record.Name),
                Sp("Id", record.Id),
                Sp("Status", record.Status),
                Sp("Priority", record.Priority),
                Sp("Court", record.Court),
                Sp("AssignedProgrammer", record.AssignedProgrammer),
                Sp("AssignedTester", record.AssignedTester),
                Sp("DevEstimate", record.DevEstimate),
                Sp("TestEstimate", record.TestEstimate)
            );
        }

        [UsesSproc(DB.Scheduler, "UpdateSackerCache")]
        public void UpdateSackerCache(SackerCache record)
        {
            lda.Execute("UdpateSackerCache", DB.Scheduler,
                Sp("SackerCacheId", record.SackerCacheId),
                Sp("RequestTypeId", record.RequestTypeId),
                Sp("Name", record.Name),
                Sp("Id", record.Id),
                Sp("Status", record.Status),
                Sp("Priority", record.Priority),
                Sp("Court", record.Court),
                Sp("AssignedProgrammer", record.AssignedProgrammer),
                Sp("AssignedTester", record.AssignedTester),
                Sp("DevEstimate", record.DevEstimate),
                Sp("TestEstimate", record.TestEstimate)
            );
        }

        [UsesSproc(DB.Scheduler, "DeleteSackerCache")]
        public void DeleteSackerCache(SackerCache record)
        {
            lda.Execute("DeleteSackerCache", DB.Scheduler, Sp("SackerCacheId", record.SackerCacheId));
        }

        private SqlParameter Sp(string name, object val)
        {
            return SqlParams.Single(name, val);
        }
	}
}
