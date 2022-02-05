using System;
using System.Text.RegularExpressions;

namespace SftpCoordinator
{
    public static class SearchPattern
    {
        public static bool IsMatch(string input, string pattern, string antiPattern)
        {
            Func<string, string> normalize = new Func<string, string>((s) =>
            {
                s = Regex.Escape(s ?? ""); //mostly to take care of periods (.) in the search pattern
                s = s.Replace(@"\*", ".*"); //replace escaped asterisks with equivalent regex
                s = "^" + s + "$"; //must match exact string
                return s;
            });
            pattern = normalize(pattern);
            antiPattern = normalize(antiPattern);
            
            //return if input matches the pattern and doesn't match the antipattern
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase) && !Regex.IsMatch(input, antiPattern, RegexOptions.IgnoreCase);
        }
    }
}
