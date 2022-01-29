using System;

namespace TimeTracking.Processing
{
    public static class DateTimeExtension
    {
        public static DateTime GetCurrentWeekSunday(this DateTime now)
        {
            int diff = (7 + (now.DayOfWeek - DayOfWeek.Sunday)) % 7;
            int startOfWeek = now.AddDays(-1 * diff).Date.Day;
            int month = now.Month;
            int year = now.Year;
            if (startOfWeek > now.Day)
                --month; //Subtract 1 from the month if the Monday was in the last month
            if (month < 1)
            {
                --year; //Subtract one from the year if the first of the week was in the previous month of the previous year
                month = 1;
            }
            return new DateTime(year, month, startOfWeek, 0, 0, 0);
        }

        public static DateTime GetNextSaturday(this DateTime now)
        {
            while (now.DayOfWeek != DayOfWeek.Saturday)
                now = now.AddDays(1);
            return new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
        }
    }
}