using System;
using System.Collections.Generic;
using System.Linq;

namespace Uheaa.Common.CommonScreens
{
    public class CsvHeaderValidationResults
    {
        public string Header { get; private set; }
        public bool HasErrors { get { return MissingFields.Any() || ExtraFields.Any(); } }
        public List<string> MissingFields { get; private set; }
        public List<string> ExtraFields { get; private set; }
        public CsvHeaderValidationResults(string header)
        {
            MissingFields = new List<string>();
            ExtraFields = new List<string>();
            this.Header = header;
        }

        public string GenerateErrorMessage()
        {
            List<string> sentences = new List<string>();
            Func<string, string> format = new Func<string, string>(s => s.Contains(' ') ? s : s + "(or " + UnCamelCase(s) + ")");
            if (MissingFields.Any())
                sentences.Add("These fields weren't found in the header file: " + string.Join("; ", MissingFields.Select(o => format(o))));
            if (ExtraFields.Any())
                sentences.Add("These fields weren't expected: " + string.Join("; ", ExtraFields));
            sentences.Add("Header Contents: " + Header);
            return string.Join(Environment.NewLine + Environment.NewLine, sentences);
        }

        private static string UnCamelCase(string camelCased)
        {
            if (camelCased.Length == 0)
                return camelCased;
            List<string> words = new List<string>();
            string curWord = camelCased.First().ToString().ToUpper(); //fix for lowerCamelCasing
            for (int i = 1; i < camelCased.Length; i++)
            {
                if (char.IsUpper(camelCased[i]))
                {
                    words.Add(curWord);
                    curWord = "";
                }
                curWord += camelCased[i].ToString();
            }
            words.Add(curWord);
            return string.Join(" ", words.ToArray());
        }
    }
}
