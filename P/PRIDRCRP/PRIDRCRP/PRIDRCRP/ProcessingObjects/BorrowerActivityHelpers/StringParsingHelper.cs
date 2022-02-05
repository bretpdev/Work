using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace PRIDRCRP
{
    public static class StringParsingHelper
    {
        /// <summary>
        /// Gets the next numeric string in body after header with read characters allowed to be interspersed
        /// </summary>
        /// <param name="body">The string to be parsed</param>
        /// <param name="header">The string literal header that the numeric will be parsed after</param>
        /// <param name="read">Array of non-numeric characters to accept in the result(should be used with characters like ".$,/")</param>
        /// <returns>parsed numeric value with allowed characters</returns>
        public static string ReadNumericFromHeaderToString(string body, string header, char[] read)
        {
            if (!body.Contains(header))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = body.IndexOf(header) + header.Length; i < body.Length; i++)
            {
                int? ret = body[i].ToString().ToIntNullable();
                if (!ret.HasValue && !read.Contains(body[i]))
                {
                    break;
                }
                sb.Append(body[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Reads and returns the next string after header, stops when encountering a period
        /// </summary>
        public static string ReadNextStringToPeriod(string body, string header)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = body.IndexOf(header) + header.Length + 1; i < body.Length; i++)
            {
                if (body[i] == '.')
                {
                    return sb.ToString().Replace("|", "").TrimStart();
                }
                else
                {
                    sb.Append(body[i]);
                }
            }
            return sb.ToString().Replace("|", "").TrimStart();
        }

        /// <summary>
        /// Reads and returns the next string after header(will skip one space after the header), stops when encountering a space or 
        /// line end denoted by a vertical bar
        /// </summary>
        public static string ReadNextString(string body, string header)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = body.IndexOf(header) + header.Length + 1; i < body.Length; i++)
            {
                if (body[i] == ' ' || body[i] == '|')
                {
                    return sb.ToString();
                }
                else
                {
                    sb.Append(body[i]);
                }
            }
            return sb.ToString();
        }

        public static string SafeSubStringTrimmed(string str, int start, int length)
        {
            return str.SafeSubString(start, length).Trim();
        }

        public static string GetFlatString(string str)
        {
            return str.Replace(" ", "").Replace("|", "");
        }

        public static string GetCommaDelimitedList(List<int> records)
        {
            if(records.Count == 0)
            {
                return "";
            }

            string result = "";
            foreach(var rec in records)
            {
                result += rec.ToString() + ",";
            }
            return result.Remove(result.Length - 1);
        }
    }
}
