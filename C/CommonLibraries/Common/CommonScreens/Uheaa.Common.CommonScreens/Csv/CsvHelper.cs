using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Uheaa.Common.CommonScreens
{
    /// <summary>
    /// Helps encode and decode CSV (comma separated value) files.
    /// Rules as per http://tools.ietf.org/html/rfc4180 (please update this documentation and code as you find issues)
    /// 1.  All fields are separated by a comma (,)
    /// 2.  Fields containing line breaks, double quotes, and/or commas must be enclosed in double-quotes.
    /// 3.  If a field is enclosed in double-quotes, and double-quotes within the actual field must be escaped with another preceding double-quote.
    /// </summary>
    public static class CsvHelper
    {
        public static string SimpleEncode(IEnumerable values)
        {
            return string.Join(",", values.Cast<object>().Select(o => (o ?? "").ToString()).ToArray());
        }

        public static string[] Parse(string line)
        {
            return new CsvParser(line).Parse();
        }

        /// <summary>
        /// Validates the given header against the given type.  Any errors (missing headers, extra properties, etc) are listed in the
        /// results object returned.
        /// </summary>
        public static CsvHeaderValidationResults ValidateHeader<T>(string header)
        {
            List<string> remainingHeaders = Parse(header).ToList();
            List<string> remainingProperties = new List<string>();
            bool includeOnlyRequiredProperties = typeof(T).GetProperties().Any(o => o.GetCustomAttribute<CsvRequiredAttribute>() != null);
            foreach (PropertyInfo pi in typeof(T).GetProperties().Where(o => o.GetSetMethod() != null && o.GetGetMethod() != null))
            {
                if (includeOnlyRequiredProperties && pi.GetCustomAttribute<CsvRequiredAttribute>() == null)
                    continue;
                if (pi.GetCustomAttribute<CsvLineNumberAttribute>() != null || pi.GetCustomAttribute<CsvLineContentAttribute>() != null)
                    continue; //these are special properties and shouldn't be included in validation
                string name = pi.Name;
                var attr = pi.GetCustomAttribute<CsvHeaderNameAttribute>();
                if (attr != null)
                    name = attr.HeaderName;
                remainingProperties.Add(name);
                foreach (string headerName in remainingHeaders.ToArray())
                    if (PropertyHeaderMatch(name, headerName))
                    {
                        remainingHeaders.Remove(headerName);
                        remainingProperties.Remove(name);
                        break;
                    }

            }
            var results = new CsvHeaderValidationResults(header);
            results.ExtraFields.AddRange(remainingHeaders);
            results.MissingFields.AddRange(remainingProperties);
            return results;
        }
        /// <summary>
        /// Returns true if the property name matches the header name.  Takes extra spaces, camel-casing, etc into account.
        /// </summary>
        private static bool PropertyHeaderMatch(string propertyName, string header)
        {
            Func<bool> match = () => propertyName == header;

            if (match())
                return true;

            propertyName = propertyName.ToLower();
            header = header.ToLower();
            if (match())
                return true;

            header = header.Replace(" ", "");
            return match();
        }

        public static CsvParseResults<T> ParseTo<T>(string[] fileContentsIncludingHeaderLine) where T : new()
        {
            return ParseTo<T>(fileContentsIncludingHeaderLine.FirstOrDefault(), fileContentsIncludingHeaderLine.Skip(1).ToArray());
        }
        /// <summary>
        /// Parses a header and file contents into a collection of type T.
        /// Any errors will be listed in the results object.
        /// </summary>
        public static CsvParseResults<T> ParseTo<T>(string headerLine, string[] lines) where T : new()
        {
            var results = new CsvParseResults<T>();
            var properties = typeof(T).GetProperties().ToList();
            var headers = Parse(headerLine);
            results.HeaderValidationResults = ValidateHeader<T>(headerLine);
            if (!results.HeaderValidationResults.HasErrors)
            {
                //gather all property setters and arrange by column index
                Dictionary<int, MethodInfo> setterIndices = new Dictionary<int, MethodInfo>();
                for (int headerIndex = 0; headerIndex < headers.Length; headerIndex++)
                {
                    string header = headers[headerIndex];
                    foreach (var prop in properties.ToArray())
                    {
                        if (PropertyHeaderMatch(prop.Name, header))
                        {
                            properties.Remove(prop);
                            setterIndices[headerIndex] = prop.GetSetMethod();
                        }
                    }
                }
                //gather special setter properties
                MethodInfo lineNumberSetter = null;
                MethodInfo lineContentSetter = null;
                var lineNumberProp = properties.Where(o => o.GetCustomAttribute<CsvLineNumberAttribute>() != null).SingleOrDefault();
                var lineContentProp = properties.Where(o => o.GetCustomAttribute<CsvLineContentAttribute>() != null).SingleOrDefault();
                if (lineNumberProp != null)
                    lineNumberSetter = lineNumberProp.GetSetMethod();
                if (lineContentProp != null)
                    lineContentSetter = lineContentProp.GetSetMethod();
                //process each line in the file
                for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
                {
                    string line = lines[lineNumber];
                    try
                    {
                        var obj = new T();
                        string[] values = Parse(line);
                        for (int i = 0; i < values.Length; i++)
                        {
                            var setter = setterIndices[i];
                            setter.Invoke(obj, new object[] { Convert.ChangeType(values[i], setter.GetParameters().First().ParameterType) });
                        }
                        if (lineNumberSetter != null) //set special line number property
                            lineNumberSetter.Invoke(obj, new object[] { lineNumber + 2 });
                        if (lineContentSetter != null) //set special line content property
                            lineContentSetter.Invoke(obj, new object[] { line });
                        results.AddValidLine(lineNumber + 2, line, obj);
                    }
                    catch (Exception ex)
                    {
                        results.AddInvalidLine(lineNumber + 2, line, ex);
                    }
                }
            }
            return results;
        }
    }
}