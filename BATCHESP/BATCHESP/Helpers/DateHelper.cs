using System;

namespace BATCHESP
{
    public class DateHelper
    {
        public DateTime ImmutableStart { get; private set; }
        public DateTime ImmutableEnd { get; private set; }
        public DateHelper(DateTime immutableStart, DateTime immutableEnd)
        {
            this.ImmutableStart = immutableStart.Date;
            this.ImmutableEnd = immutableEnd.Date;
        }

        public Tuple<DateTime, DateTime> GetNewStartAndEnd(DateTime currentStart, DateTime currentEnd)
        {
            currentStart = currentStart.Date;
            currentEnd = currentEnd.Date;
            if (currentStart < ImmutableStart)
                currentEnd = ImmutableStart.AddDays(-1);
            else
                currentStart = ImmutableEnd.AddDays(1);
            return Tuple.Create(currentStart, currentEnd);
        }

        public bool Intersects(DateTime currentStart, DateTime currentEnd)
        {
            currentStart = currentStart.Date;
            currentEnd = currentEnd.Date;
            if (currentStart <= ImmutableEnd && currentEnd >= ImmutableStart)
                return true;
            return false;
        }

        public bool SurroundsCurrentRange(DateTime currentStart, DateTime currentEnd)
        {
            currentStart = currentStart.Date;
            currentEnd = currentEnd.Date;
            if (currentStart <= ImmutableStart && currentEnd >= ImmutableEnd)
                return true;
            return false;
        }

        public bool IsWithinCurrentRange(DateTime currentStart, DateTime currentEnd)
        {
            currentStart = currentStart.Date;
            currentEnd = currentEnd.Date;
            if (currentStart >= ImmutableStart && currentEnd <= ImmutableEnd)
                return true;
            return false;
        }

        /// <summary>
        /// Removes ending deferment if it is a D46.
        /// Per BA, this should be removed so that the system can automatically generate
        /// a new one with the proper date range.
        /// 
        /// Method will only be called if the deferment being inspected is a D46.
        /// </summary>
        public bool EndingDefNeedsRemoval(DateTime currentStart, DateTime currentEnd)
        {
            currentStart = currentStart.Date;
            currentEnd = currentEnd.Date;
            if (currentEnd >= ImmutableEnd)
                return true;
            return false;
        }
    }
}
