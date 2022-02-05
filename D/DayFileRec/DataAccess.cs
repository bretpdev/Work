using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Text;
using Q;

namespace DayFileRec
{
	class DataAccess : DataAccessBase
	{
		/// <summary>
		/// Queries Sacker for schedule details of active, scheduled SAS jobs.
		/// </summary>
		public static List<SackerSchedule> GetActiveSchedules()
		{
			StringBuilder queryBuilder = new StringBuilder();
			queryBuilder.Append("SELECT B.ID AS JobId,");
			queryBuilder.Append(" CASE WHEN A.dMonAM = 1 THEN A.dMonAM ELSE A.wMonAM END AS WeeklySunday,");
			queryBuilder.Append(" CASE WHEN A.dMon = 1 THEN A.dMon ELSE A.wMon END AS WeeklyMonday,");
			queryBuilder.Append(" CASE WHEN A.dTue = 1 THEN A.dTue ELSE A.wTue END AS WeeklyTuesday,");
			queryBuilder.Append(" CASE WHEN A.dWed = 1 THEN A.dWed ELSE A.wWed END AS WeeklyWednesday,");
			queryBuilder.Append(" CASE WHEN A.dThu = 1 THEN A.dThu ELSE A.wThu END AS WeeklyThursday,");
			queryBuilder.Append(" CASE WHEN A.dFri = 1 THEN A.dFri ELSE A.wFri END AS WeeklyFriday,");
			queryBuilder.Append(" CASE WHEN A.dSat = 1 THEN A.dSat ELSE A.wSat END AS WeeklySaturday,");
			queryBuilder.Append(" A.mJan AS MonthlyJaunary,");
			queryBuilder.Append(" A.mFeb AS MonthlyFebruary,");
			queryBuilder.Append(" A.mMar AS MonthlyMarch,");
			queryBuilder.Append(" A.mApr AS MonthlyApril,");
			queryBuilder.Append(" A.mMay AS MonthlyMay,");
			queryBuilder.Append(" A.mJun AS MonthlyJune,");
			queryBuilder.Append(" A.mJul AS MonthlyJuly,");
			queryBuilder.Append(" A.mAug AS MonthlyAugust,");
			queryBuilder.Append(" A.mSep AS MonthlySeptember,");
			queryBuilder.Append(" A.mOct AS MonthlyOctober,");
			queryBuilder.Append(" A.mNov AS MonthlyNovember,");
			queryBuilder.Append(" A.mDec AS MonthlyDecember,");
			queryBuilder.Append(" A.mOn AS MonthlyDate,");
			queryBuilder.Append(" A.EOM AS MonthlyEndOfMonth,");
			queryBuilder.Append(" A.Other AS MonthlyOther,");
			queryBuilder.Append(" A.ExcSat AS ExcludeSaturday,");
			queryBuilder.Append(" A.ExcEOM AS ExcludeEndOfMonth,");
			queryBuilder.Append(" A.ExcBeforeEOM AS ExcludeDayBeforeEndOfMonth,");
			queryBuilder.Append(" A.ExcHoliday AS ExcludeHoliday,");
			queryBuilder.Append(" A.RollNot AS DoNotRoll,");
			queryBuilder.Append(" A.RollNext AS RollNextDay,");
			queryBuilder.Append(" A.RollPrevious AS RollPreviousDay,");
			queryBuilder.Append(" A.OnDemand");
			queryBuilder.Append(" FROM SCKR_DAT_SASSchedule A");
			queryBuilder.Append(" INNER JOIN SCKR_DAT_SAS B");
			queryBuilder.Append(" ON A.Job = B.Job");
			queryBuilder.Append(" WHERE B.Type = 'Scheduled'");
			queryBuilder.Append(" AND B.Status = 'Active'");
			return BsysDataContext(false).ExecuteQuery<SackerSchedule>(queryBuilder.ToString()).OrderBy(p => p.JobId).ToList().ToList();
		}//GetActiveSchedules()

		/// <summary>
		/// Returns a list of all SAS files (files whose names start with "ULW")
		/// in the specified location whose createion date/time stamp falls between
		/// 10 PM the night before runDate and 10 AM the morning of runDate.
		/// </summary>
		/// <param name="runDate">The date for which the script is being run.</param>
		/// <param name="location">The location to search for SAS files.</param>
		public static List<string> GetActualSasReportNames(DateTime runDate, string location)
		{
			DateTime previousDay = runDate.AddDays(-1);
			if (runDate.DayOfWeek == DayOfWeek.Monday) { previousDay = runDate.AddDays(-3); }
			DateTime downloadStart = new DateTime(previousDay.Year, previousDay.Month, previousDay.Day, 22, 0, 0);
			DateTime downloadEnd = new DateTime(runDate.Year, runDate.Month, runDate.Day, 10, 0, 0);

			//Get the path/name of all SAS reports in the specified location.
			string[] sasFiles = Directory.GetFiles(location, "ULW*", SearchOption.TopDirectoryOnly);

			//Compare their time stamps to runDate's download time range, and save any matches to a list.
			List<string> newSasFiles = new List<string>();
			foreach (string sasFileName in sasFiles.Where(p => !p.Contains("R1.")))
			{
				FileInfo sasFileInfo = new FileInfo(sasFileName);
				if (sasFileInfo.CreationTime >= downloadStart && sasFileInfo.CreationTime <= downloadEnd)
				{
					newSasFiles.Add(sasFileInfo.Name);
					string message = string.Format("Found {0}", sasFileInfo.Name);
					Reconciler.OnNoteworthyEvent(new Reconciler.NoteworthyEventArgs(message));
				}
			}//foreach

			return newSasFiles.OrderBy(p => p).ToList();
		}//GetActualSasReportNames()

		/// <summary>
		/// Returns a list of report numbers from Sacker for the given job ID.
		/// Reports 1 and Z are not included in the return results.
		/// </summary>
		/// <param name="jobId">The job ID for which to fetch report numbers.</param>
		public static List<string> GetReportNumbers(string jobId)
		{
			StringBuilder queryBuilder = new StringBuilder();
			queryBuilder.Append("SELECT LTRIM(RTRIM(B.ReportNo)) AS ReportNumber");
			queryBuilder.Append(" FROM SCKR_DAT_SAS A");
			queryBuilder.Append(" INNER JOIN SCKR_REF_PageCenter B");
			queryBuilder.Append(" ON A.Job = B.Job");
			queryBuilder.Append(string.Format(" WHERE A.ID = '{0}'", jobId));
			queryBuilder.Append(" AND B.PrintOpt = 'FT'");
			queryBuilder.Append(" AND B.ReportNo NOT IN ('1', 'Z')");
			return BsysDataContext(false).ExecuteQuery<string>(queryBuilder.ToString()).OrderBy(p => p).ToList();
		}//GetReportNumbers()

		public static void WriteExtraReportsFile(List<string> reportNames)
		{
			string extraFile = PersonalDataDirectory + "DailyFileReconciliationExtraReports.txt";
			using (StreamWriter fileWriter = new StreamWriter(extraFile, false))
			{
				foreach (string reportName in reportNames)
				{
					fileWriter.WriteLine(reportName);
				}
				fileWriter.Close();
			}//using
		}//WriteExtraReportsFile()

		public static void WriteMatchingReportsFile(List<string> reportNames)
		{
			string matchFile = PersonalDataDirectory + "DailyFileReconciliationMatches.txt";
			using (StreamWriter fileWriter = new StreamWriter(matchFile, false))
			{
				foreach (string reportName in reportNames)
				{
					fileWriter.WriteLine(reportName);
				}
				fileWriter.Close();
			}//using
		}//WriteMatchingReportsFile()

		public static void WriteMissingReportsFile(List<Reconciler.ScheduledReport> reports)
		{
			string missingFile = PersonalDataDirectory + "DailyFileReconciliationMissingReports.txt";
			using (StreamWriter fileWriter = new StreamWriter(missingFile, false))
			{
				foreach (Reconciler.ScheduledReport report in reports)
				{
					fileWriter.Write(report.Name);
					if (!string.IsNullOrEmpty(report.Caveat))
					{
						fileWriter.Write(string.Format(" ({0})", report.Caveat));
					}
					fileWriter.WriteLine();
				}
				fileWriter.Close();
			}//using
		}//WriteMissingReportsFile()

		#region Nested classes
		public class SackerSchedule
		{
			public string JobId { get; set; }
			public bool WeeklySunday { get; set; }
			public bool WeeklyMonday { get; set; }
			public bool WeeklyTuesday { get; set; }
			public bool WeeklyWednesday { get; set; }
			public bool WeeklyThursday { get; set; }
			public bool WeeklyFriday { get; set; }
			public bool WeeklySaturday { get; set; }
			public bool MonthlyJanuary { get; set; }
			public bool MonthlyFebruary { get; set; }
			public bool MonthlyMarch { get; set; }
			public bool MonthlyApril { get; set; }
			public bool MonthlyMay { get; set; }
			public bool MonthlyJune { get; set; }
			public bool MonthlyJuly { get; set; }
			public bool MonthlyAugust { get; set; }
			public bool MonthlySeptember { get; set; }
			public bool MonthlyOctober { get; set; }
			public bool MonthlyNovember { get; set; }
			public bool MonthlyDecember { get; set; }
			public string MonthlyDate { get; set; }
			public bool MonthlyEndOfMonth { get; set; }
			public string Other { get; set; }
			public bool ExcludeSaturday { get; set; }
			public bool ExcludeEndOfMonth { get; set; }
			public bool ExcludeDayBeforeEndOfMonth { get; set; }
			public bool ExcludeHoliday { get; set; }
			public bool DoNotRoll { get; set; }
			public bool RollNextDay { get; set; }
			public bool RollPreviousDay { get; set; }
			public bool OnDemand { get; set; }
		}//SackerSchedule

		/// <summary>
		/// String constants representing valid SAS file locations.
		/// </summary>
		public class SasFolder
		{
			public const string ARCHIVE = @"X:\Archive\SAS\";
			public const string FTP = @"X:\PADD\FTP\";
		}//SasFolder

		private class SasReport
		{
			public string JobId { get; set; }
			public string ReportNumber { get; set; }
		}//JobReport
		#endregion Nested classes
	}//class
}//namespace
