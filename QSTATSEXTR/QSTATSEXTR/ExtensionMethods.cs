using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace QSTATSEXTR
{
    public static class ExtensionMethods
    {
        public static TimeSpan? ToTimeSpanNullable(this string span)
        {
            span = span.Trim();
            if (span.Length == 6)
            {
                int? h = span.Substring(0, 2).ToIntNullable();
                int? m = span.Substring(2, 2).ToIntNullable();
                int? s = span.Substring(4, 2).ToIntNullable();
                if (h.HasValue && m.HasValue && s.HasValue)
                    return new TimeSpan(h.Value, m.Value, s.Value);
            }
            return null;
        }

        public static DateTime? ToDateNullableYearFirst(this string date)
        {
            try
            {
                int year = date.Substring(0, 4).ToInt();
                int month = date.Substring(4, 2).ToInt();
                int day = date.Substring(6, 2).ToInt();
                return new DateTime(year, month, day);
            }
            catch
            {
                return null;
            }
        }
    }
}
