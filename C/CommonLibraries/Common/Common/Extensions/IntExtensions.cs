
namespace Uheaa.Common
{
    public static class IntExtensions
    {
        #region Between
        public static bool BetweenInc(this int value, int min, int max)
        {
            return Between(value, min, max, true);
        }
        public static bool Between(this int value, int min, int max)
        {
            return Between(value, min, max, false);
        }
        private static bool Between(this int value, int min, int max, bool inclusive)
        {
            if (min > max)
            {
                max = min ^ max;
                min = max ^ min;
                max = min ^ max;
            }
            if (inclusive) return value >= min && value <= max;
            return value > min && value < max;
        }
        #endregion

        #region Even/Odd
        /// <summary>
        /// Returns true if the value is an even number.
        /// </summary>
        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }
        /// <summary>
        /// Returns true if the value is an odd number.
        /// </summary>
        public static bool IsOdd(this int value)
        {
            return !value.IsEven();
        }
        /// <summary>
        /// Returns even if value is even, otherwise returns odd.
        /// </summary>
        public static T EveryOther<T>(this int value, T even, T odd)
        {
            return value.IsEven() ? even : odd;
        }
        #endregion
    }
}
