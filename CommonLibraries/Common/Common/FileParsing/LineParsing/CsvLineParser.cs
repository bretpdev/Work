using System;
using System.Linq;

namespace Uheaa.Common
{
    public class CsvLineParser : LineParserBase<CsvLineParser, string[]>
    {
        public string Delimiter { get; set; }
        public static readonly PostValidateLineEventHandler EmptyLineValidator;
        public static readonly PostValidateLineEventHandler EmptyValueValidator;
        public static readonly PostValidateLineEventHandler EmptyLineAndValueValidator;
        static CsvLineParser()
        {
            EmptyLineValidator = (line, parser) => line.Content.Length > 0;
            EmptyValueValidator = (line, parser) => !line.Content.Any(l => string.IsNullOrEmpty(l));
            EmptyLineAndValueValidator = Combine(CsvLineParser.EmptyLineValidator, CsvLineParser.EmptyValueValidator);
            
        }
        protected override string[] ParseLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return new string[] { };
            return line.Split(new string[] { Delimiter }, StringSplitOptions.None);
        }
        public CsvLineParser(string location = null, string delimiter = ",", PreValidateLineEventHandler preValidate = null, PostValidateLineEventHandler postValidate = null, ProcessLineEventHandler processLine = null)
            : base(location, preValidate, postValidate, processLine)
        {
            Delimiter = delimiter;
        }
    }
}
