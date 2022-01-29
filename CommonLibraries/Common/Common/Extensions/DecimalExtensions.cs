namespace Uheaa.Common
{
    public static class DecimalExtensions
    {
        public  static decimal FormatDecimal(this string text)
        {
            if (text.Contains("."))
                return text.ToDecimal();
            else
                return string.Format("{0}.00", text).ToDecimal();
        }
    }
}