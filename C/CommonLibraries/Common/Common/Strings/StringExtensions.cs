using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Uheaa.Common
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsPopulated(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static bool AreEqualAndNotEmpty(this string s, params string[] fields)
        {
            foreach (string t in fields)
            {
                if (s == t && !s.IsNullOrEmpty())
                    return true;
            }

            return false;
        }

        public static string ToDateFormat(this string s)
        {
            return s.Insert(2, "/").Insert(5, "/");
        }

        public static string To6DigitDate(this DateTime d)
        {
            return $"{d.Month.ToString().PadLeft(2, '0')}{d.Day.ToString().PadLeft(2, '0')}{d.Year.ToString().Substring(2, 2)}";
        }

        /// <summary>
        /// Converts a date, 7/1/19 or 7/1/2019, to a string of 10 digits Ex: 07/01/2019
        /// </summary>
        public static string ToTenDigitDate(this DateTime s)
        {
            string d = s.ToString().Contains("/") ? s.ToShortDateString() : s.ToShortDateString().ToDateFormat();
            List<string> l = d.Split('/').ToList();
            if (l[0].Length == 1)
                l[0] = "0" + l[0];
            if (l[1].Length == 1)
                l[1] = "0" + l[1];
            if (l[2].Length < 4)
            {
                if (l[2].ToInt() > DateTime.Now.Date.Year.ToString().Substring(2, 2).ToInt())
                    l[2] = "19" + l[2];
                else
                    l[2] = DateTime.Now.Date.Year.ToString().Substring(0, 2) + l[2];
            }
            return string.Join("/", l);
        }

        public static bool IsNumeric(this string s)
        {
            double num;
            return Double.TryParse(s, out num);
        }

        public static string SafeChar(this string s, int index)
        {
            return s.SafeSubString(index, 1);
        }

        public static string SafeSubString(this string originalString, int startIndex, int length)
        {
            if (originalString.IsNullOrEmpty())
                return string.Empty;
            if (originalString.Length >= startIndex + length)
            {
                return originalString.Substring(startIndex, length);
            }
            else
            {
                return originalString.Substring(startIndex);
            }
        }
        public static List<string> SplitAndPreserveQuotes(this string originalValue, string delimiter)
        {
            return SplitAndPreserveQuotes(originalValue, delimiter, true);
        }

        public static List<string> SplitAndPreserveQuotes(this string originalValue, string delimiter, bool trim)
        {
            List<string> fields = new List<string>();
            bool withinQuotes = false;
            int startIndex = 0;

            for (int currentIndex = 0; currentIndex <= originalValue.Length - 1; currentIndex++)
            {
                if (originalValue.Substring(currentIndex, 1) == @"""")
                {
                    withinQuotes = !withinQuotes;
                    continue;
                }

                if (!withinQuotes && originalValue.Substring(currentIndex, 1) == delimiter)
                {
                    if (trim)
                        fields.Add(originalValue.Substring(startIndex, currentIndex - startIndex).Trim());
                    else
                        fields.Add(originalValue.Substring(startIndex, currentIndex - startIndex));

                    startIndex = currentIndex + 1;
                }
            }

            if (trim)
                fields.Add(originalValue.Substring(startIndex).Trim());
            else
                fields.Add(originalValue.Substring(startIndex));
            return fields;
        }

        public static List<string> SplitAndRemoveQuotes(this string originalValue, string delimiter)
        {
            return SplitAndRemoveQuotes(originalValue, delimiter, true);
        }

        public static List<string> SplitAndRemoveQuotes(this string originalValue, string delimiter, bool trim)
        {
            List<string> fields = new List<string>();
            bool withinQuotes = false;
            int startIndex = 0;

            for (int currentIndex = 0; currentIndex <= originalValue.Length - 1; currentIndex++)
            {
                if (originalValue.Substring(currentIndex, 1) == @"""")
                {
                    withinQuotes = !withinQuotes;
                    continue;
                }

                if (!withinQuotes && originalValue.Substring(currentIndex, 1) == delimiter)
                {
                    if (trim)
                        fields.Add(originalValue.Substring(startIndex, currentIndex - startIndex).Replace(@"""", string.Empty).Trim());
                    else
                        fields.Add(originalValue.Substring(startIndex, currentIndex - startIndex).Replace(@"""", string.Empty));

                    startIndex = currentIndex + 1;
                }
            }

            if (trim)
                fields.Add(originalValue.Substring(startIndex).Replace(@"""", string.Empty).Trim());
            else
                fields.Add(originalValue.Substring(startIndex).Replace(@"""", string.Empty));
            return fields;
        }

        public static string ConcatList<T>(this List<T> list)
        {
            string value = string.Empty;
            foreach (T s in list)
            {
                value += s.ToString();
            }

            return value;
        }

        public static IEnumerable<string> ToHtmlLines(this List<string> table, string indent)
        {
            List<string> htmlLines = new List<string>();
            //Start the table.
            htmlLines.Add("<table>");
            //Write out the header row from the column names.
            htmlLines.Add(String.Format("{0}<tr>", indent));

            //add the headers
            foreach (string item in table[0].SplitAndRemoveQuotes(","))
            {
                htmlLines.Add(string.Format("{0}{0}<th>{1}</th>", indent, item));
            }

            bool oddRow = table.Count % 2 == 0; //always end with a gray row
            foreach (string row in table.Skip(1))//skip 1 since we have already added the header
            {
                htmlLines.Add(string.Format("{0}<tr class='{1}'>", indent, oddRow ? "oddrow" : "evenrow"));
                oddRow = !oddRow;
                foreach (string column in row.SplitAndRemoveQuotes(","))
                {
                    htmlLines.Add(string.Format("{0}{0}<td>{1}</td>", indent, column));
                }
                htmlLines.Add(string.Format("{0}</tr>", indent));
            }

            //End the table and return the final product.
            htmlLines.Add("</table>");

            return htmlLines;
        }

        /// <summary>
        /// Appends all the fields with parantheses and commas and writes them out to the stream
        /// </summary>
        /// <param name="writer">StreamWriter used to write the new line to</param>
        /// <param name="fields">The fields to be appended into a new string</param>
        public static void WriteCommaDelimitedLine(this StreamWriter writer, params string[] fields)
        {
            StringBuilder lineBuilder = new StringBuilder();
            for (int i = 0; i <= fields.GetUpperBound(0); i++)
            {
                lineBuilder.Append(string.Format(@",""{0}{1}{2}""", "{", i.ToString(), "}"));
            }

            writer.WriteLine(string.Format(lineBuilder.ToString(), fields).Substring(1));
        }

        /// <summary>
        /// Appends all the fields with commas and writes them out to the stream without parantheses
        /// </summary>
        /// <param name="writer">StreamWriter used to srite the new line to</param>
        /// <param name="removeQuotes">True to remove parantheses, leave blank to include them</param>
        /// <param name="fields">The fields to be appended into a new string</param>
        public static void WriteCommaDelimitedLine(this StreamWriter writer, bool removeQuotes, params string[] fields)
        {
            StringBuilder lineBuilder = new StringBuilder();
            for (int i = 0; i <= fields.GetUpperBound(0); i++)
            {
                if (removeQuotes)
                    lineBuilder.Append(string.Format(@",{0}{1}{2}", "{", i.ToString(), "}"));
                else
                    WriteCommaDelimitedLine(writer, fields);
            }

            writer.WriteLine(string.Format(lineBuilder.ToString(), fields).Substring(1));
        }

        public static bool IsIn<T>(this T value, params T[] possibles)
        {
            return possibles.Contains(value);
        }

        public static bool Contains(this string value, params string[] possibles)
        {
            foreach (string item in possibles)
            {
                if (item.Equals(value))
                    return true;
            }

            return false;
        }



        public static bool ContainsToUpper(this List<string> list, string value)
        {
            foreach (string item in list)
            {
                if (item.ToUpper().Equals(value.ToUpper()))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Takes a list of strings and places them with the new character
        /// </summary>
        /// <param name="value">String to be changed</param>
        /// <param name="newChar">New character</param>
        /// <param name="oldChars">array of characters to change</param>
        /// <returns></returns>
        public static string Replace(this string value, string newChar, params string[] oldChars)
        {
            foreach (string item in oldChars)
            {
                value = value.Replace(item, newChar);
            }
            return value;
        }

        public static int NthIndexOf(this string value, string seek, int n, int start = 0)
        {
            int count = 0;
            int index = start;
            while (count <= n)
            {
                int next = index + seek.Length;
                if (next > value.Length) return -1;
                index = value.IndexOf(seek, next);
                if (index == -1)
                    return index;
                count++;
            }
            return index;
        }

        public static SqlParameter ToSqlParameter(this string value, string name)
        {
            return new SqlParameter(name, value);
        }

        public static string PascalToWords(this string value)
        {
            if (value != null && value.Length >= 2)
            {
                //attempt to extrapolate words from PascalCasing
                for (int i = 1; i < value.Length; i++)
                {
                    bool newWord = Char.IsLower(value[i - 1]) && Char.IsUpper(value[i]);
                    if (newWord)
                        value = value.Insert(i++, " ");
                }
            }
            return value;
        }

        public static string TrimLeft(this string value, string trim)
        {
            if (value == null || trim == null) throw new NullReferenceException();
            while (value.StartsWith(trim))
                value = value.Substring(trim.Length);
            return value;
        }

        public static string TrimRight(this string value, string trim)
        {
            if (value == null || trim == null) throw new NullReferenceException();
            while (value.EndsWith(trim))
                value = value.Substring(0, value.Length - trim.Length);
            return value;
        }

        public static string Trim(this string value, string trim)
        {
            return value.TrimLeft(trim).TrimRight(trim);
        }

        public static string FormatWith(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        /// <summary>
        /// Given a start and an end string, finds the first instance of both within the given string and returns the contents between 
        /// </summary>
        public static string Between(this string value, string begin, string end)
        {
            int start = value.IndexOf(begin) + begin.Length;
            int stop = value.IndexOf(end, start);
            return value.Substring(start, stop - start);
        }

        /// <summary>
        /// Removes the characters added by a masked textbox and trims the string.  Chars:  (  )  -  _
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemovePhoneMask(this string text)
        {
            return text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "").Trim();
        }

        public static string MaskSsn(this string text)
        {
            return string.Format("XXX-XX-{0}", text.SafeSubString(text.Length - 4, 4));
        }

        public static bool IsAlpha(this string text)
        {
            foreach(char c in text)
            {
                if(!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))
                {
                    return false;
                }
            }
            return true;
        }
    }
}