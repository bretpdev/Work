using regex = System.Text.RegularExpressions.Regex;

namespace Uheaa.Common
{
    public class Regex : regex
    {
        public Regex() : base() { }
        public Regex(string pattern) : base(pattern) { }
        public Regex(string pattern, System.Text.RegularExpressions.RegexOptions options) : base(pattern, options) { }
        public Regex(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Checks for a match against the given regex.  Adds Start of Line and End of Line anchors to the pattern.
        /// </summary>
        public static bool IsExactMatch(string input, string pattern)
        {
            return regex.IsMatch(input, "^" + pattern + "$");
        }

        public static bool IsSsn(string input)
        {
            return regex.Match(input, @"(\d{3}-\d{2}-\d{4}|XXX-XX-XXXX|\d{9}|\d{3} ?\d{2} ?\d{4}|XXX XX XXXX)").Success;
        }
    }
}
