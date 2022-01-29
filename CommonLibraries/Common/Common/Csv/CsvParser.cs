using System;
using System.Collections.Generic;

namespace Uheaa.Common
{
    class CsvParser
    {
        private string line;
        private int curPos;
        private string CurChar { get { return curPos >= line.Length ? null : line[curPos].ToString(); } }
        const string DoubleQuote = "\"";
        const string Comma = ",";

        public CsvParser(string line)
        {
            this.line = line;
        }

        public string[] Parse()
        {
            List<string> values = new List<string>();
            while (curPos < line.Length)
            {
                if (string.IsNullOrEmpty(CurChar))
                    ParseWhitespace();
                else if (CurChar == DoubleQuote)
                    values.Add(ParseQuotedField());
                else
                    values.Add(ParseNormalField());
            }
            return values.ToArray();
        }

        private string ParseNormalField()
        {
            int nextComma = line.IndexOf(',', curPos);
            string value = "";
            if (nextComma == -1)
            {
                value = line.Substring(curPos);
                curPos = line.Length;
            }
            else
            {
                value = line.Substring(curPos, nextComma - curPos);
                curPos = nextComma + 1;
            }
            return value;
        }

        private string ParseQuotedField()
        {
            if (CurChar != DoubleQuote)
                throw new InvalidOperationException("ParseQuotedField must only be called when CurrentChar is a double quotation mark.");
            curPos++; //move past quotation
            int endQuote = line.IndexOf('"', curPos);
            string field = line.Substring(curPos, endQuote - curPos);
            int commaAferQuote = line.IndexOf(Comma, endQuote);
            if (commaAferQuote != -1)
                curPos = commaAferQuote + 1;
            else
                curPos = line.Length;
            return field.Trim();
        }

        private void ParseWhitespace()
        {
            while (string.IsNullOrEmpty(CurChar) && curPos < line.Length)
                curPos++;
        }
    }

}
