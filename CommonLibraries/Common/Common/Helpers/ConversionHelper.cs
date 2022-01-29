using System;

namespace Uheaa.Common
{
    public static class ConversionHelper
    {
        public static DateTime ToDate(this string text)
        {
            return ToDateNullable(text).Value;
        }
        public static DateTime? ToDateNullable(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            DateTime temp = DateTime.Now;
            if (DateTime.TryParse(text, out temp))
                return temp;

            if(text.Replace(" ", "").Length == 6)
                if(DateTime.TryParse(text.Replace(" ", @"/"), out temp))
                    return temp;
            if (text.Length == 8)
                if (DateTime.TryParse(text.Insert(2, @"/").Insert(5, @"/"), out temp))
                    return temp;

            return null;
        }
        public static string ToLongDateNoDOW(this string date)
        {
            return date.ToDate().ToLongDateString().Replace(date.ToDate().DayOfWeek.ToString() + ", ", "");
        }

        public static int ToInt(this string text)
        {
            return text.ToIntNullable() ?? 0;
        }

        public static int? ToIntNullable(this string text)
        {
            int value;
            if (int.TryParse(text, out value))
                return value;
            return null;
        }

        public static double ToDouble(this string text)
        {
            double value;
            if (double.TryParse(text, out value))
                return value;
            return 0;
        }

        public static decimal ToDecimal(this string text)
        {
            return text.ToDecimalNullable() ?? 0m;
        }

        public static decimal? ToDecimalNullable(this string text)
        {
            decimal value;
            if (decimal.TryParse(text.Replace("$", "").Replace("%", ""), out value))
                return value;
            return null;
        }
        public static string ToMoney(this decimal money) { return money.ToMoney(0); }
        public static string ToMoney(this decimal money, int significantDigits)
        {
            return string.Format("{0:C" + significantDigits + "}", money);
        }

        public static string ToPercent(this decimal fractionalPercentage) { return fractionalPercentage.ToPercent(0); }
        public static string ToPercent(this decimal fractionalPercentage, int significantDigits)
        {
            return string.Format("{0:P" + significantDigits + "}", fractionalPercentage).Replace(" ", string.Empty);
        }

        public static long ToLong(this string text)
        {
            return text.ToLongNullable() ?? 0;
        }

        public static long? ToLongNullable(this string text)
        {
            long value;
            if (long.TryParse(text, out value))
                return value;
            return 0;
        }
    }
}