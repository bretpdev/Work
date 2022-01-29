
namespace Uheaa.Common
{
    public static class BoolExtensions
    {
        /// <summary>
        /// Short for Quaternary, this method takes the given bool? value and translates it to trueValue, falseValue, or noneValue.
        /// </summary>
        public static T Quat<T>(this bool? value, T trueValue, T falseValue, T noneValue)
        {
            if (!value.HasValue)
                return noneValue;
            if (value.Value)
                return trueValue;
            return falseValue;
        }
    }
}
