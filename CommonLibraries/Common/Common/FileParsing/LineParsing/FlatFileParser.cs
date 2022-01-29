using System.Collections.Generic;
using System.Linq;

namespace Uheaa.Common
{
    public class FlatFileParser : LineParserBase<FlatFileParser, FlatField[]>
    {
        public FlatFileDefinition FileDefinition { get; set; }
        public FlatFileParser(string location = null, FlatFileDefinition fileDefinition = null, PreValidateLineEventHandler preValidate = null, PostValidateLineEventHandler postValidate = null, ProcessLineEventHandler processLine = null)
            : base(location, preValidate, postValidate, processLine)
        {
            this.FileDefinition = fileDefinition;
        }

        protected override FlatField[] ParseLine(string line)
        {
            List<FlatField> fields = new List<FlatField>();
            foreach (FlatFieldDefinition def in FileDefinition)
                if (def.Start + def.Length <= line.Length)
                    fields.Add(new FlatField(def, line.Substring(def.Start, def.Length)));
                else
                    fields.Add(new FlatField(def, null));
            return fields.ToArray();
        }

        public static PreValidateLineEventHandler ValidateMatchesDefinition;
        static FlatFileParser()
        {
            ValidateMatchesDefinition = (line, parser) =>
            {
                bool success = true;
                foreach (FlatFieldDefinition def in parser.FileDefinition)
                    if (line.Length < def.Start + def.Length)
                    {
                        success = false;
                        break;
                    }
                return success;
            };
        }
    }

    public class FlatFieldDefinition
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public string Description { get; set; }
    }

    public class FlatFileDefinition : IEnumerable<FlatFieldDefinition>
    {
        private List<FlatFieldDefinition> Definitions { get; set; }
        public FlatFileDefinition()
        {
            Definitions = new List<FlatFieldDefinition>();
        }
        public void Add(string header, int length)
        {
            var last = this.Definitions.OrderByDescending(o => o.Start + o.Length).FirstOrDefault();
            int start = 0;
            if (last != null)
                start = last.Start + last.Length;

            FlatFieldDefinition definition = new FlatFieldDefinition();
            definition.Description = header;
            definition.Length = length;
            definition.Start = start;
            Add(definition);
        }

        public void Add(FlatFieldDefinition definition)
        {
            Definitions.Add(definition);
        }

        #region IEnumerable Members
        public IEnumerator<FlatFieldDefinition> GetEnumerator()
        {
            return Definitions.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Definitions.GetEnumerator();
        }
        #endregion

        public string Header
        {
            get { return CsvHelper.SimpleEncode(this.Select(o => o.Description)); }
        }

        public string HeaderWithLengths
        {
            get { return string.Join(",", this.Select(o => o.Description + "(" + o.Length + ")").ToArray()); }
        }
    }

    public class FlatField
    {
        public FlatFieldDefinition Definition { get; set; }
        public string Value { get; set; }
        public FlatField(FlatFieldDefinition definition, string value)
        {
            Definition = definition;
            Value = value;
        }
    }
}
