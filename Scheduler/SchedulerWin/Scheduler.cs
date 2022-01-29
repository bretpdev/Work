using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SchedulerWeb
{
	class Scheduler
	{
        DataAccess da;
        ProcessLogRun plr;
        public Scheduler(ProcessLogRun plr, DataAccess da)
        {
            this.da = da;
            this.plr = plr;
        }
        public void Process()
        { 
			da.RemoveCompletedRequests(); //deletes completed requests from the scheduler table

            CacheSacker();

			List<SackerData> requests = da.GetSackerRequests(); //returns all incomplete sacker requests along with default estimates if they are null


			Productivity productivityValue = da.GetProductivityValues();

			foreach (SackerData request in requests)
			{
				request.RequestPriority = da.GetRequestPriority(request);
				if (request.RequestPriority == null)
					request.RequestPriority = da.SetDefaultPriority(request);
			}
			
			foreach (SackerData request in requests.OrderBy(r => r.RequestPriority)) //The orderby will be key in getting estimates correct
			{
                if (request.AssignedDeveloper.IsNullOrEmpty())
                    request.AssignedDeveloper = da.GetDefaultProgrammer();
                if (request.AssignedTester.IsNullOrEmpty())
                    request.AssignedTester = da.GetDefaultTester();

				if (request.CurrentStatus.Contains("Test") || request.CurrentStatus.Contains("Train"))
				{
					if (request.DevStartDate == null)
						request.DevStartDate = new DateTime(1900, 01, 01);
					if (request.DevEndDate == null)
						request.DevEndDate = new DateTime(1900, 01, 01);
				}
				else
				{
					DateTime? maxDate = requests.Where(p => p.AssignedDeveloper == request.AssignedDeveloper).Max(q => q.DevEndDate);
					request.DevStartDate = maxDate == null ? DateTime.Now : maxDate;
					request.DevEndDate = SetEndDate(request.DevEstimate, request.DevStartDate.Value, productivityValue.DevHours, request.AssignedDeveloper);
					request.DevEndDate = AdjustedEndDate(request.DevEndDate);
				}

				request.TesterStartDate = requests.Where(p => p.AssignedTester == request.AssignedTester).Max(q => q.TesterEndDate) ?? DateTime.Now;
				request.TesterStartDate = request.DevEndDate.Value.AddHours(32 - request.DevEndDate.Value.Hour).AddMinutes(-request.DevEndDate.Value.Minute) > request.TesterStartDate ? request.DevEndDate.Value.AddHours(32 - request.DevEndDate.Value.Hour).AddMinutes(-request.DevEndDate.Value.Minute) : request.TesterStartDate;
				request.TesterEndDate = SetEndDate(request.TesterEstimate, request.TesterStartDate.Value, productivityValue.TestHours, request.AssignedTester);
				request.TesterEndDate = AdjustedEndDate(request.TesterEndDate);

				da.SetEstimatedCompletionDates(request);
			}
		}

		/// <summary>
		/// Adjusts the end date to account for night hours, late completion, and weekends.
		/// </summary>
		/// <param name="requestEndDate">End date</param>
		/// <returns>New dev end date for the request</returns>
		private DateTime? AdjustedEndDate(DateTime? requestEndDate)
		{
			DateTime? endDate = requestEndDate;
			if (endDate < DateTime.Now)
				endDate = DateTime.Now.AddHours(2);
			if (endDate.Value.Hour >= 16)
				endDate = endDate.Value.AddHours(8 + (24 - endDate.Value.Hour) + (endDate.Value.Hour - 16));//gets me to 8 am the next day + whatever # of hours it was going to take that day
			if (endDate.Value.DayOfWeek == DayOfWeek.Saturday)
				endDate = endDate.Value.AddDays(2);
			if (endDate.Value.DayOfWeek == DayOfWeek.Sunday)
				endDate = endDate.Value.AddDays(1);

			return endDate;
		}

		/// <summary>
		/// Sets the end date for the request based on the estimate, startdate, productivehours and assignee
		/// </summary>
		/// <param name="requestEstimate">Estimate in hours of how long the project will take</param>
		/// <param name="requestStartDate">start date for either the dev or tester</param>
		/// <param name="productiveHours">productivity value</param>
		/// <param name="assignee">Assigned dev/tester</param>
		/// <returns>Date</returns>
		private DateTime SetEndDate(double requestEstimate, DateTime? requestStartDate, double productiveHours, string assignee)
		{
			bool startDateOff = false;
			List<DateTime> timeOffList = da.GetTimeOff(requestStartDate.Value, assignee);
			startDateOff = timeOffList.Any(r => r.Date == requestStartDate.Value.Date);

			double timeOff = TimeOffAdjustment(timeOffList, requestStartDate.Value, productiveHours, requestEstimate);

			double remainingHours = GetTodaysRemainingHours(requestStartDate.Value, productiveHours, startDateOff);
			double remainingDays = (requestEstimate - remainingHours) / productiveHours < 0 ? 0 : (requestEstimate - remainingHours) / productiveHours;

			double weekendDays = da.GetWeekendDays(requestStartDate.Value, Math.Ceiling(remainingDays + timeOff));
			double holidays = da.GetHolidays(requestStartDate.Value, Math.Ceiling(remainingDays + timeOff + weekendDays));

			remainingDays += weekendDays + holidays + timeOff;
			double partialDayHours = (remainingDays - Math.Floor(remainingDays)) * productiveHours;

			return requestStartDate.Value.AddHours(remainingHours + partialDayHours).AddDays(Math.Floor(remainingDays));
		}

		/// <summary>
		/// Gets a value for the hours remaining in the day for work to be performed.
		/// </summary>
		/// <param name="requestStartDate">date for work to begin on the erquest</param>
		/// <param name="productiveHours">static value for productive hours in a day.</param>
		/// <param name="startDateOff">true of the person has the starting day for the request off</param>
		/// <returns></returns>
		private double GetTodaysRemainingHours(DateTime requestStartDate, double productiveHours, bool startDateOff)
		{
			double remainingHours = 0; 
			if (!startDateOff)
			{
				if (requestStartDate.Date == DateTime.Now.Date && requestStartDate.Hour < 16) //16 represents 4PM
					remainingHours = 16 - requestStartDate.Hour - ((double)requestStartDate.Minute / 60);
				if (remainingHours > productiveHours)
					remainingHours = productiveHours;
			}
			return remainingHours;
		}

		/// <summary>
		/// Adjust time off
		/// </summary>
		/// <param name="timeOffList">List of days off for the requests assigned party</param>
		/// <param name="requestStartDate">Start date for the request</param>
		/// <param name="productiveHours"># of productive hours in the day</param>
		/// <param name="estimate">estimate of hours needed to complete the project</param>
		/// <returns>double of number of days to add to the total request time</returns>
		private double TimeOffAdjustment(List<DateTime> timeOffList,  DateTime requestStartDate, double productiveHours, double estimate)
		{
			double timeOff = 0.0;
			var endDate = requestStartDate.AddDays(estimate / productiveHours);
			foreach (DateTime dayOff in timeOffList)
			{
				if (requestStartDate.Date <= dayOff.Date && dayOff.Date <= endDate.Date)
				{
					endDate = endDate.AddDays(1);
					timeOff += 1;
				}
			}
			return timeOff;
		}

        private void CacheSacker()
        {
            int addedCount = 0;
            int updatedCount = 0;
            int removeCount = 0;
            List<SackerCache> allSourceItems = new List<SackerCache>();
            allSourceItems.AddRange(da.GetSasData());
            allSourceItems.AddRange(da.GetScriptData());
            allSourceItems.AddRange(da.GetLetterData());
            List<SackerCache> allCachedItems = da.GetSackerCache();
            foreach (var source in allSourceItems)
            {
                var existingCachedItem = allCachedItems.FirstOrDefault(o => o.RequestTypeId == source.RequestTypeId && o.Id == source.Id);
                if (existingCachedItem != null)
                {
                    source.SackerCacheId = existingCachedItem.SackerCacheId;
                    if (!source.CacheValuesMatch(existingCachedItem))
                    {
                        da.UpdateSackerCache(source);
                        updatedCount++;
                    }
                    allCachedItems.Remove(existingCachedItem);
                }
                else
                {
                    da.AddSackerCache(source);
                    addedCount++;
                }
            }

            foreach (var remaining in allCachedItems)
            {
                da.DeleteSackerCache(remaining);
                removeCount++;
            }

            plr.AddNotification(string.Format("Finished Caching Sacker Data: Added {0}, Removed {1}, Updated {2}", addedCount, removeCount, updatedCount), NotificationType.EndOfJob, NotificationSeverityType.Informational);
        }
	}
}
