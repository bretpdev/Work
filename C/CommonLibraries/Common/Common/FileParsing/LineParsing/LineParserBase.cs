using System.IO;

namespace Uheaa.Common
{
    public abstract class LineParserBase<O, T> : ILineParser 
        where O : LineParserBase<O, T>
    {
        protected abstract T ParseLine(string line);
        public string Location { get; set; }

        public LineParserBase(string location = null, PreValidateLineEventHandler preValidate = null, PostValidateLineEventHandler postValidate = null, ProcessLineEventHandler processLine = null)
        {
            Location = location;

            if (processLine != null) ProcessLine += processLine;
            if (preValidate != null) PreValidateLine += preValidate;
            if (postValidate != null) PostValidateLine += postValidate;
        }

        public LineParserResult Parse(int? skip = 0)
        {
            if (skip == null) skip = 0;
            LineParserResult result = new LineParserResult();
            using (StreamReader sr = new StreamR(Location))
            {
                string line = null;
                int number = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (number < skip.Value)
                        continue;
                    result.TotalLines++;
                    if (PreValidateLine == null || PreValidateLine(line, (O)this))
                    {
                        Line<T> l = new Line<T>(line, ParseLine(line), number++);
                        if (PostValidateLine == null || PostValidateLine(l, (O)this))
                        {
                            result.ValidLines++;
                            ProcessLine(l, (O)this);
                        }
                        else
                            result.InvalidLines--;
                    }
                    else
                        result.InvalidLines--;
                }
            }
            return result;
        }

        public delegate void ProcessLineEventHandler(Line<T> value, O parser);
        public event ProcessLineEventHandler ProcessLine;

        public delegate bool PreValidateLineEventHandler(string line, O parser);
        public event PreValidateLineEventHandler PreValidateLine;

        public delegate bool PostValidateLineEventHandler(Line<T> value, O parser);
        public event PostValidateLineEventHandler PostValidateLine;

        public static LineParserBase<O, T>.PreValidateLineEventHandler Combine(LineParserBase<O, T>.PreValidateLineEventHandler handler, LineParserBase<O, T>.PreValidateLineEventHandler otherHandler) 
        {
            return (line, parser) => handler(line, parser) && otherHandler(line, parser);
        }

        public static LineParserBase<O, T>.PostValidateLineEventHandler Combine(LineParserBase<O, T>.PostValidateLineEventHandler handler, LineParserBase<O, T>.PostValidateLineEventHandler otherHandler)
        {
            return (line, parser) => handler(line, parser) && otherHandler(line, parser);
        }
    }
}
