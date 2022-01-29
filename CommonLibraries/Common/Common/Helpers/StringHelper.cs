using System;
using System.Reflection;

namespace Uheaa.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// Sanitizes each string property of the given object, converting empty or whitespace strings to nulls.
        /// </summary>
        public static void Sanitize(object o, bool trimStrings = true, bool nullifyEmptyString = true)
        {
            foreach (PropertyInfo pi in o.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    try
                    {
                        string origValue = (string)pi.GetValue(o, null);
                        string value = origValue;
                        if (value == null)
                            continue;
                        if (trimStrings)
                            value = value.Trim();
                        if (nullifyEmptyString && string.IsNullOrEmpty(value))
                            value = null;
                        if (value != origValue)
                            pi.SetValue(o, value, null);
                    }
                    catch (Exception)
                    {
                        continue; //getter or setter threw an error, probably a property we weren't supposed to access.
                    }

                }
            }
        }
    }
}
