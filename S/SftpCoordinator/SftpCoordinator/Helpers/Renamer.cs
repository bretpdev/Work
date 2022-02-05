using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uheaa.Common;

namespace SftpCoordinator
{
    public static class Renamer
    {
        /// <summary>
        /// Renames files using a custom format string (one more easily readable to users).
        /// Format is: [[token|formatString]]
        /// Supported keywords:
        /// [[date]]
        /// [[file]]
        /// [[ext]]
        /// </summary>
        /// <param name="format"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string Rename(string format, string filename, DateTime dateTime)
        {
            FormatFinder ff = new FormatFinder(format);
            List<object> values = new List<object>();
            while (!string.IsNullOrEmpty(ff.Token))
            {
                switch (ff.Token.ToLower())
                {
                    case "date":
                        values.Add(dateTime);
                        break;
                    case "file":
                        values.Add(Path.GetFileNameWithoutExtension(filename));
                        break;
                    case "ext":
                        values.Add(Path.GetExtension(filename));
                        break;
                    default:
                        ff = new FormatFinder(format, ff.EndBracket);
                        continue;
                }
                string parameter = "{" + (values.Count - 1);
                if (!ff.Format.IsNullOrEmpty())
                    parameter += ":" + ff.Format;
                parameter += "}";
                format = format.Substring(0, ff.StartBracket) + parameter + format.Substring(ff.EndBracket + 1);
                ff = new FormatFinder(format);
            }
            return string.Format(format, values.ToArray());
        }

        private class FormatFinder
        {
            public int StartBracket { get; private set; }
            public int EndBracket { get; private set; }
            public string Token { get; private set; }
            public string Format { get; private set; }
            public FormatFinder(string formatString, int start = 1)
            {
                if (formatString.Length < 2) return;
                int? startBracket = null;
                int? endBracket = null;
                for (int i = start; i < formatString.Length; i++)
                {
                    string prevCur = formatString[i - 1].ToString() + formatString[i];

                    if (!startBracket.HasValue)
                    {
                        if (prevCur == "[[")
                            startBracket = StartBracket = i - 1;
                    }
                    else
                    {
                        if (prevCur == "]]")
                        {
                            endBracket = EndBracket = i;
                            break;
                        }
                    }
                }
                if (endBracket.HasValue && startBracket.HasValue)
                {
                    string token = formatString.Substring(StartBracket + 2, (EndBracket - StartBracket) - 3);
                    string[] split = token.Split(':');
                    Token = split[0];
                    if (split.Count() == 2)
                        Format = split[1];
                }
            }
        }
    }
}
