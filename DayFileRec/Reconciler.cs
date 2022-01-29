using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DayFileRec
{
	class Reconciler
	{
		public static event EventHandler<NoteworthyEventArgs> NoteworthyEvent;
		public static void OnNoteworthyEvent(NoteworthyEventArgs e)
		{
			if (NoteworthyEvent != null) { NoteworthyEvent(null, e); }
		}

		private static readonly DateTime INVALID_DATE = DateTime.MinValue;

		public static void Reconcile(DateTime downloadDate, string sasFolder)
		{
			//Find all SAS files for the given download date.
			string searchingMessage = string.Format("Searching {0} for SAS files...", sasFolder);
			OnNoteworthyEvent(new NoteworthyEventArgs(searchingMessage));
			List<string> foundFiles = DataAccess.GetActualSasReportNames(downloadDate, sasFolder);

			//Check the active schedules from Sacker to see which reports we should have received.
			List<DataAccess.SackerSchedule> activeSchedules = DataAccess.GetActiveSchedules();
			string totalSchedulesMessage = string.Format("Checking {0} active SAS schedules from Sacker...", activeSchedules.Count);
			OnNoteworthyEvent(new NoteworthyEventArgs(""));
			OnNoteworthyEvent(new NoteworthyEventArgs(totalSchedulesMessage));
			List<ScheduledReport> expectedReports = DetermineExpectedReports(activeSchedules, downloadDate);

			//Check the reports we received against what we expected, and write out the reconciliation reports.
			string reconcileMessage = "Reconciling the received files with the expected files...";
			OnNoteworthyEvent(new NoteworthyEventArgs(""));
			OnNoteworthyEvent(new NoteworthyEventArgs(reconcileMessage));
			Finnish(foundFiles, expectedReports);
			OnNoteworthyEvent(new NoteworthyEventArgs("Finished!"));
		}//Reconcile()

		private static List<ScheduledReport> DetermineExpectedReports(List<DataAccess.SackerSchedule> activeSchedules, DateTime downloadDate)
		{
			List<ScheduledReport> expectedReports = new List<ScheduledReport>();
			foreach (DataAccess.SackerSchedule schedule in activeSchedules)
			{
				//Check for scheduling options that make it impossible to determine whether to expect reports.
				//Mark such jobs as expected, but with an appropriate caveat.
				if (schedule.OnDemand)
				{
					string scheduleMessage = string.Format("{0} runs on demand, so reports may be expected.", schedule.JobId);
					OnNoteworthyEvent(new NoteworthyEventArgs(scheduleMessage));
					foreach (string reportNumber in DataAccess.GetReportNumbers(schedule.JobId))
					{
						string caveat = "Job runs on demand, so may or may not be expected.";
						string reportName = GetReportName(schedule.JobId, reportNumber);
						expectedReports.Add(new ScheduledReport(reportName, caveat));
					}
				}
				else if (!string.IsNullOrEmpty(schedule.Other))
				{
					string scheduleMessage = string.Format("{0} has \"Other\" scheduling instructions, so reports may be expected.", schedule.JobId);
					OnNoteworthyEvent(new NoteworthyEventArgs(scheduleMessage));
					string caveat = string.Format("Job runs on the following schedule, so may or may not be expected: {0}", schedule.Other);
					foreach (string reportNumber in DataAccess.GetReportNumbers(schedule.JobId))
					{
						string reportName = GetReportName(schedule.JobId, reportNumber);
						expectedReports.Add(new ScheduledReport(reportName, caveat));
					}
				}
				//Check the remaining scheduling options that definitively determine whether to expect reports.
				else if (JobMatchesWeeklySchedule(schedule, downloadDate) || JobMatchesMonthlySchedule(schedule, downloadDate))
				{
					string scheduleMessage = string.Format("{0} is scheduled for today.", schedule.JobId);
					OnNoteworthyEvent(new NoteworthyEventArgs(scheduleMessage));
					foreach (string reportNumber in DataAccess.GetReportNumbers(schedule.JobId))
					{
						string reportName = GetReportName(schedule.JobId, reportNumber);
						expectedReports.Add(new ScheduledReport(reportName, null));
					}
				}
			}//foreach
			return expectedReports;
		}//DetermineExpectedReports()

		private static void Finnish(List<string> foundFiles, List<ScheduledReport> expectedReports)
		{
			//Set up three lists for matching, extra, and missing reports.
			List<string> matchingReports = new List<string>();
			List<string> extraReports = new List<string>();
			List<ScheduledReport> missingReports = new List<ScheduledReport>();

			//Sort the passed-in lists and compare their elements.
			foundFiles.Sort();
			expectedReports = expectedReports.OrderBy(p => p.Name).ToList();
			while (foundFiles.Count > 0 && expectedReports.Count > 0)
			{
				//Get the found file name up to the second dot.
				int secondDotIndex = foundFiles[0].IndexOf('.', foundFiles[0].IndexOf('.') + 1);
				string foundFile = foundFiles[0].Substring(0, secondDotIndex + 1);

				//Append a dot to the expected report name.
				string expectedReportName = expectedReports[0].Name + ".";

				//Compare the two strings and augment the appropriate results list.
				int comparisonResult = string.Compare(foundFile, expectedReportName);
				if (comparisonResult < 0)
				{
					//Found a file we didn't expect.
					//Augment the extra list and remove the found file name
					//so we can check the next one against what we expected.
					string eventMessage = string.Format("Did not expect to receive {0}", foundFiles[0]);
					OnNoteworthyEvent(new NoteworthyEventArgs(eventMessage));
					extraReports.Add(foundFiles[0]);
					foundFiles.RemoveAt(0);
				}
				else if (comparisonResult > 0)
				{
					//Expected a file that wasn't found.
					//Augment the missing list and remove the expected report name
					//so we can check the next one against what we found.
					string eventMessage = string.Format("Expected to receive {0}, but it was not found", expectedReports[0].Name);
					OnNoteworthyEvent(new NoteworthyEventArgs(eventMessage));
					missingReports.Add(expectedReports[0]);
					expectedReports.RemoveAt(0);
				}
				else
				{
					//Names match.
					//Augment the match list and remove from both comparison lists.
					string eventMessage = string.Format("Received {0} as expected", expectedReports[0].Name);
					OnNoteworthyEvent(new NoteworthyEventArgs(eventMessage));
					matchingReports.Add(expectedReports[0].Name);
					foundFiles.RemoveAt(0);
					expectedReports.RemoveAt(0);
				}
			}//while

			//Add any remaining elements into their respective lists.
			foreach (string foundFile in foundFiles)
			{
				string eventMessage = string.Format("Did not expect to receive {0}", foundFile);
				OnNoteworthyEvent(new NoteworthyEventArgs(eventMessage));
				extraReports.Add(foundFile);
			}
			foreach (ScheduledReport expectedReport in expectedReports)
			{
				string eventMessage = string.Format("Expected to receive {0}, but it was not found", expectedReport.Name);
				OnNoteworthyEvent(new NoteworthyEventArgs(eventMessage));
				missingReports.Add(expectedReport);
			}

			//Write the reconciliation reports.
			OnNoteworthyEvent(new NoteworthyEventArgs(""));
			OnNoteworthyEvent(new NoteworthyEventArgs("Writing the reconciliation reports to the T drive."));
			DataAccess.WriteMatchingReportsFile(matchingReports);
			DataAccess.WriteExtraReportsFile(extraReports);
			DataAccess.WriteMissingReportsFile(missingReports);
		}//Finnish()

		private static DateTime GetDataDate(DataAccess.SackerSchedule schedule, DateTime downloadDate)
		{
			//Adjust the date to match when the data went to the warehouse, which is what the SAS schedule is based on.
			DateTime dataDate = downloadDate.AddDays(-1).Date;

			//Check for exclusions and roll options to see if we need to adjust the date,
			//or if we shouldn't have a file at all.
			bool needToRoll = false;
			if (dataDate.IsDayBeforeEndOfMonth() && schedule.ExcludeDayBeforeEndOfMonth) { needToRoll = true; }
			if (dataDate.IsEndOfMonth() && schedule.ExcludeEndOfMonth) { needToRoll = true; }
			if (dataDate.IsHoliday() && schedule.ExcludeHoliday) { needToRoll = true; }
			if (dataDate.IsSaturday() && schedule.ExcludeSaturday) { needToRoll = true; }
			while (needToRoll)
			{
				if (schedule.RollNextDay)
				{
					dataDate = dataDate.AddDays(1);
				}
				else if (schedule.RollPreviousDay)
				{
					dataDate = dataDate.AddDays(-1);
				}
				else if (schedule.DoNotRoll)
				{
					//No reports should be generated for this job.
					return INVALID_DATE;
				}
				else
				{
					//An exclusion was hit, but no roll option is specified.
					string rollMessage = string.Format("The schedule for {0} has exclusions set, but no roll option is selected. Please set a roll option and re-run the script.", schedule.JobId);
					throw new Exception(rollMessage);
				}
				//Keep rolling if we landed on another date that's excluded by the schedule.
				needToRoll = false;
				if (dataDate.IsDayBeforeEndOfMonth() && schedule.ExcludeDayBeforeEndOfMonth) { needToRoll = true; }
				if (dataDate.IsEndOfMonth() && schedule.ExcludeEndOfMonth) { needToRoll = true; }
				if (dataDate.IsHoliday() && schedule.ExcludeHoliday) { needToRoll = true; }
				if (dataDate.IsSaturday() && schedule.ExcludeSaturday) { needToRoll = true; }
			}//while

			return dataDate;
		}//GetDataDate()

		private static string GetReportName(string jobId, string reportNumber)
		{
			//Take out the 'T' at index 1.
			string prefix = jobId.Remove(1, 1);
			//Take out the 'U' at index 0.
			string suffix = prefix.Substring(1);
			//Make sure the report number starts with R.
			if (!reportNumber.StartsWith("R")) { reportNumber = "R" + reportNumber; }
			//String it all together.
			return string.Format("{0}.{1}{2}", prefix, suffix, reportNumber);
		}//GetReportName()

		private static bool JobMatchesMonthlySchedule(DataAccess.SackerSchedule schedule, DateTime downloadDate)
		{
			//Make sure the day of month is specified.
			if (string.IsNullOrEmpty(schedule.MonthlyDate)) { return false; }

			//Adjust the date to match when the data went to the warehouse, which is what the SAS schedule is based on.
			DateTime dataDate = GetDataDate(schedule, downloadDate);
			//If we got INVALID_DATE, it means we hit an exclusion that says don't generate reports.
			if (dataDate == INVALID_DATE) { return false; }

			//Check whether the adjusted date is the same as the scheduled date.
			int scheduledDayOfMonth = schedule.MonthlyDate.ExtractInteger();
			if (scheduledDayOfMonth == dataDate.Day)
			{
				if (schedule.MonthlyJanuary && dataDate.Month == 1) { return true; }
				if (schedule.MonthlyFebruary && dataDate.Month == 2) { return true; }
				if (schedule.MonthlyMarch && dataDate.Month == 3) { return true; }
				if (schedule.MonthlyApril && dataDate.Month == 4) { return true; }
				if (schedule.MonthlyMay && dataDate.Month == 5) { return true; }
				if (schedule.MonthlyJune && dataDate.Month == 6) { return true; }
				if (schedule.MonthlyJuly && dataDate.Month == 7) { return true; }
				if (schedule.MonthlyAugust && dataDate.Month == 8) { return true; }
				if (schedule.MonthlySeptember && dataDate.Month == 9) { return true; }
				if (schedule.MonthlyOctober && dataDate.Month == 0) { return true; }
				if (schedule.MonthlyNovember && dataDate.Month == 11) { return true; }
				if (schedule.MonthlyDecember && dataDate.Month == 12) { return true; }
			}

			//If none of the above checks returned true, we don't expect reports for this job.
			return false;
		}//JobMatchesMonthlySchedule()

		private static bool JobMatchesWeeklySchedule(DataAccess.SackerSchedule schedule, DateTime downloadDate)
		{
			//Adjust the date to match when the data went to the warehouse, which is what the SAS schedule is based on.
			DateTime dataDate = GetDataDate(schedule, downloadDate);
			//If we got INVALID_DATE, it means we hit an exclusion that says don't generate reports.
			if (dataDate == INVALID_DATE) { return false; }

			//Check whether the adjusted date is the same as the scheduled date.
			if (schedule.WeeklySunday && dataDate.DayOfWeek == DayOfWeek.Sunday) { return true; }
			if (schedule.WeeklyMonday && dataDate.DayOfWeek == DayOfWeek.Monday) { return true; }
			if (schedule.WeeklyTuesday && dataDate.DayOfWeek == DayOfWeek.Tuesday) { return true; }
			if (schedule.WeeklyWednesday && dataDate.DayOfWeek == DayOfWeek.Wednesday) { return true; }
			if (schedule.WeeklyThursday && dataDate.DayOfWeek == DayOfWeek.Thursday) { return true; }
			if (schedule.WeeklyFriday && dataDate.DayOfWeek == DayOfWeek.Friday) { return true; }
			if (schedule.WeeklySaturday && dataDate.DayOfWeek == DayOfWeek.Saturday) { return true; }

			//On Mondays, check whether we should have something from Friday, Saturday, or Sunday.
			if (dataDate.DayOfWeek == DayOfWeek.Sunday && (schedule.WeeklyFriday || schedule.WeeklySaturday || schedule.WeeklySunday)) { return true; }

			//If none of the above checks returned true, we don't expect reports for this job.
			return false;
		}//JobMatchesWeeklySchedule()

		public class NoteworthyEventArgs : EventArgs
		{
			public readonly string Message;

			public NoteworthyEventArgs(string message)
			{
				Message = message;
			}
		}//NoteworthyEventArgs

		public class ScheduledReport
		{
			public readonly string Name;
			public readonly string Caveat;

			public ScheduledReport(string name, string caveat)
			{
				Name = name;
				Caveat = caveat;
			}
		}//ScheduledReport
	}//class
}//namespace
