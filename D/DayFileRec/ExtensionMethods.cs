using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DayFileRec
{
	static class ExtensionMethods
	{
		/// <summary>
		/// Finds the first contiguous set of integers in a string and returns their value, or 0 if the string contains no numbers.
		/// </summary>
		/// <param name="s">The string from which to extract an integer.</param>
		public static int ExtractInteger(this string s)
		{
			StringBuilder integerBuilder = new StringBuilder();
			//Skip over any non-numeric characters.
			int i = 0;
			while (i < s.Length && !Regex.IsMatch(s[i].ToString(), @"\d")) { i++; }
			//Add to integerBuilder until we run out of numeric characters.
			for (; i < s.Length && Regex.IsMatch(s[i].ToString(), @"\d"); i++) { integerBuilder.Append(s[i].ToString()); }
			//Return what we got, or 0 if there were no numbers.
			return (integerBuilder.Length == 0 ? 0 : int.Parse(integerBuilder.ToString()));
		}//ExtractNumber()

		/// <summary>
		/// Determines whether a date is the second to last day of the month.
		/// </summary>
		/// <param name="d">The date to check.</param>
		public static bool IsDayBeforeEndOfMonth(this DateTime d)
		{
			//If the next day is in the same month but the day after that is in a different month,
			//d is the day before the end of the month.
			return (d.Month == d.AddDays(1).Month && d.Month != d.AddDays(2).Month);
		}//IsDayBeforeEndOfMonth()

		/// <summary>
		/// Determines whether a date is the last day of the month.
		/// </summary>
		/// <param name="d">The date to check.</param>
		public static bool IsEndOfMonth(this DateTime d)
		{
			//If the next day is in a different month, d is the end of the month.
			return (d.Month != d.AddDays(1).Month);
		}//IsEndOfMonth()

		/// <summary>
		/// Determines whether a date is a working holiday, according to UHEAA HR's published holidays.
		/// </summary>
		/// <param name="d">The date to check.</param>
		public static bool IsHoliday(this DateTime d)
		{
			//New Year's Day: January 1st.
			if (d.Month == 1 && d.Day == 1) { return true; }
			//Martin Luther King Day: 3rd Monday in January.
			if (d.Month == 1 && d.DayOfWeek == DayOfWeek.Monday && d.Day >= 15 && d.Day <= 21) { return true; }
			//Presidents' Day: 3rd Monday in February.
			if (d.Month == 2 && d.DayOfWeek == DayOfWeek.Monday && d.Day >= 15 && d.Day <= 21) { return true; }
			//Memorial Day: Last Monday in May.
			if (d.Month == 5 && d.DayOfWeek == DayOfWeek.Monday && d.Day >= 25) { return true; }
			//Independence Day: July 4th.
			if (d.Month == 7 && d.Day == 4) { return true; }
			//Pioneer Day: July 24th.
			if (d.Month == 7 && d.Day == 24) { return true; }
			//Labor Day: 1st Monday in September.
			if (d.Month == 9 && d.Day <= 7) { return true; }
			//Thanksgiving: 4th Thursday in November.
			if (d.Month == 11 && d.DayOfWeek == DayOfWeek.Thursday && d.Day >= 22 && d.Day <= 28) { return true; }
			//Black Friday: Day after 4th Thursday in November.
			if (d.Month == 11 && d.DayOfWeek == DayOfWeek.Friday && d.Day >= 23 && d.Day <= 29) { return true; }
			//Christmas: December 25th.
			if (d.Month == 12 && d.Day == 25) { return true; }
			//Any other date is not a holiday.
			return false;
		}//IsHoliday()

		/// <summary>
		/// Determines whether a date falls on a Saturday.
		/// </summary>
		/// <param name="d">The date to check.</param>
		public static bool IsSaturday(this DateTime d)
		{
			return (d.DayOfWeek == DayOfWeek.Saturday);
		}//IsSaturday()
	}//class
}//namespace
