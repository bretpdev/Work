using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common
{
    public class KvpArgValidationResults
    {
        public bool IsValid { get; set; }
        public string ValidationMesssage { get; set; }

        private KvpArgValidationResults(bool isValid, string message)
        {
            IsValid = isValid;
            ValidationMesssage = message;
        }
        public static KvpArgValidationResults ValidResult()
        {
            return new KvpArgValidationResults(true, null);
        }
        public static KvpArgValidationResults InvalidResult(string[] args, IEnumerable<string> invalidKvps, IEnumerable<string> invalidKeys, IEnumerable<string> invalidValues, IEnumerable<string> missingRequiredArgs)
        {
            List<string> lines = new List<string>();
            lines.Add("ARGS: " + string.Join(" ", args));
            if (invalidKvps.Any())
                lines.Add("These args should be formatted as KVPs (key:value) --> " + string.Join(", ", invalidKvps));
            if (invalidKeys.Any())
                lines.Add("These args don't exist --> " + string.Join(", ", invalidKeys));
            if (invalidValues.Any())
                lines.Add("These args have invalid values --> " + string.Join(", ", invalidValues));
            if (missingRequiredArgs.Any())
                lines.Add("These args are required but were not provided --> " + string.Join(", ", missingRequiredArgs));
            return new KvpArgValidationResults(false, string.Join(Environment.NewLine, lines));
        }
    }

}
