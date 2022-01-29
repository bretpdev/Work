
namespace Uheaa.Common
{
    public class LineParser : LineParserBase<LineParser, string>
    {
        public static readonly PreValidateLineEventHandler EmptyLineValidator;
        static LineParser()
        {
            EmptyLineValidator = (line, parser) => !string.IsNullOrEmpty(line);
        }
        protected override string ParseLine(string line)
        {
            return line;
        }

        public LineParser(string location = null, PreValidateLineEventHandler preValidate = null, PostValidateLineEventHandler postValidate = null, ProcessLineEventHandler processLine = null)
            : base(location, preValidate ?? EmptyLineValidator, postValidate, processLine) { }
    }
}
