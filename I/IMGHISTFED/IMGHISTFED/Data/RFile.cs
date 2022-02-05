using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;

namespace IMGHISTFED
{
    public abstract class RFile
    {
        /// <summary>
        /// Parse the given SAS Line and populate this class' properties with the results.
        /// </summary>
        public ParseResults Parse(string sasLine)
        {
            var properties = this.GetType().GetProperties();
            int expectedFieldCount = properties.Count();

            List<string> fields = sasLine.SplitAndRemoveQuotes(",");
            if (fields.Count != expectedFieldCount)
            {
                string message = string.Format("A SAS line for borrower #{0} in the {3} file has the wrong number of fields. It should have {1}, but {2} were found.", fields[0], expectedFieldCount, fields.Count, this.GetType().Name);
                return new ParseResults(message);
            }

            for (int i = 0; i < properties.Length; i++)
            {
                var setter = properties[i].GetSetMethod();
                var convertedValue = Convert.ChangeType(fields[i], properties[i].PropertyType);
                setter.Invoke(this, new object[] { convertedValue });
            }

            AfterParse(fields);
            return new ParseResults();
        }

        protected virtual void AfterParse(List<string> fields) { }
    }

    public class ParseResults
    {
        public bool Successful { get { return string.IsNullOrEmpty(Error); } }
        public string Error { get; private set; }
        public ParseResults() { }
        public ParseResults(string error) { Error = error; }
    }
}
