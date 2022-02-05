using System;
using System.Collections.Generic;
using System.Linq;

namespace Uheaa.Common.CommonScreens
{
    public class CsvParseResults<T>
    {
        public CsvParseResults() { LineResults = new List<Line>(); }
        public bool HasErrors { get { return HeaderValidationResults.HasErrors || InvalidLines.Any(); } }
        public CsvHeaderValidationResults HeaderValidationResults { get; set; }
        public List<Line> LineResults { get; set; }
        public IEnumerable<InvalidLine> InvalidLines { get { return LineResults.Where(o => o is InvalidLine).Cast<InvalidLine>(); } }
        public IEnumerable<ValidLine> ValidLines { get { return LineResults.Where(o => o is ValidLine).Cast<ValidLine>(); } }

        public abstract class Line
        {
            public int LineNumber { get; set; }
            public string LineContent { get; set; }
        }
        public class InvalidLine : Line
        {
            public Exception Exception { get; set; }
        }
        public class ValidLine : Line
        {
            public T ParsedEntity { get; set; }
        }

        internal void AddInvalidLine(int lineNumber, string lineContent, Exception ex)
        {
            LineResults.Add(new InvalidLine() { LineNumber = lineNumber, LineContent = lineContent, Exception = ex });
        }

        internal void AddValidLine(int lineNumber, string lineContent, T entity)
        {
            LineResults.Add(new ValidLine() { LineNumber = lineNumber, LineContent = lineContent, ParsedEntity = entity });
        }

        public string GenerateErrorMessage()
        {
            List<string> lines = new List<string>();
            if (HeaderValidationResults.HasErrors)
                lines.Add(HeaderValidationResults.GenerateErrorMessage());
            foreach (var line in InvalidLines)
                lines.Add(string.Format("Error on line {0} [{1}]: {2}", line.LineNumber, line.LineContent, line.Exception.Message));
            return string.Join(Environment.NewLine, lines);
        }
    }
}
