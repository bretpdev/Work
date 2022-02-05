using System;

namespace BATCHESP
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Subtracts two DateTimes and provides the difference in days.
        /// Formula glossary: Minuend - subtrahend = difference
        /// </summary>
        public static int DateDiffInDays(this DateTime minuend, DateTime subtrahend)
        {
            return Math.Abs(((TimeSpan)(minuend - subtrahend)).Days);
        }

    }
}
