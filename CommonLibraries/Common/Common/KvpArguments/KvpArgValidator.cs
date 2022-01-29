using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Uheaa.Common
{
    public static class KvpArgValidator
    {
        public static KvpArgValidationResults ValidateArguments<T>(string[] arguments)
        {
            List<string> invalidKvps = new List<string>();
            List<string> invalidKeys = new List<string>();
            List<string> invalidValues = new List<string>();
            List<string> missingRequiredArgs = new List<string>();
            Dictionary<string, KvpValidArgAttribute> validArgs = new Dictionary<string, KvpValidArgAttribute>();
            Dictionary<string, bool> requiredArgs = new Dictionary<string, bool>();

            foreach (var prop in typeof(T).GetProperties())
            {
                var attr = prop.GetCustomAttribute<KvpValidArgAttribute>();
                if (attr != null)
                {
                    validArgs[prop.Name.ToLower()] = attr;
                    if (attr.GetType() == typeof(KvpRequiredArgAttribute))//Add all required args and set value to false
                        requiredArgs[prop.Name.ToLower()] = false;
                }
            }

            foreach (string kvp in arguments)
            {
                bool atLeastOneColon = kvp.Count(c => c == ':') >= 1;
                if (!atLeastOneColon)
                    invalidKvps.Add(kvp);
                else
                {
                    string[] split = kvp.Split(':');
                    string key = split[0];
                    string value = string.Join(":", split.Skip(1).ToArray());
                    bool hasTextBeforeAndAfterSemicolon = key.Length > 0 && value.Length > 0;
                    if (!hasTextBeforeAndAfterSemicolon)
                        invalidKvps.Add(kvp);
                    else
                    {
                        if (!validArgs.ContainsKey(key))
                            invalidKeys.Add(kvp);
                        else
                        {
                            var argAttr = validArgs[key];
                            if (!argAttr.Validate(value))
                                invalidValues.Add(kvp);
                            else
                            {
                                if (requiredArgs.Where(p => p.Key == key).Any())
                                    requiredArgs[key] = true;
                            }
                        }
                    }
                }
            }

            if(requiredArgs.Where(p => !p.Value).Any())
                missingRequiredArgs.AddRange(requiredArgs.Where(p => !p.Value).Select(q => q.Key));

            if (invalidKvps.Any() || invalidKeys.Any() || invalidValues.Any() || missingRequiredArgs.Any())
            {
                return KvpArgValidationResults.InvalidResult(
                    args: arguments,
                    invalidKvps: invalidKvps,
                    invalidKeys: invalidKeys,
                    invalidValues: invalidValues,
                    missingRequiredArgs: missingRequiredArgs);
            }
            return KvpArgValidationResults.ValidResult();
        }

    }
}
