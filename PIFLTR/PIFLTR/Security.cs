using System;
using System.Text.RegularExpressions;

namespace PIFLTR
{
    public class Security
    {
        /// <summary>
        /// Removes potentially harmful characters from a given string.
        /// 
        /// Allows alphanumeric characters, periods, @ signs, hyphens, and colons.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SanitizeString(string str)
        {
            // Replace bad chars with empty strings
            try
            {
                return Regex.Replace(str, @"[^\w\.@:-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)); // Matches any char that's not a word char, a period, an @ symbol, a hyphen, or a colon
            }
            // Use timeout exception to catch malicious input that might be intended to overload Regex or otherwise provide a denial-of-service attack
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }
    }
}
