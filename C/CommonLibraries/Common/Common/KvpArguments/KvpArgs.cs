using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace Uheaa.Common
{
    /// <summary>
    /// Arguments should be given in key-value pairs, separated by colons.
    /// Each property of your derived class marked with RequiredKvpArg or ValidKvpArg will be parsed as a property.
    /// </summary>
    public abstract class KvpArgs
    {
        protected string[] Arguments { get; set; }
        public KvpArgs(string[] arguments)
        {
            Arguments = arguments;
            //populate all ValidArgs
            foreach (var prop in this.GetType().GetProperties())
            {
                var attr = prop.GetCustomAttribute<KvpValidArgAttribute>();
                if (attr != null)
                {
                    var objValue = Kvp(prop.Name.ToLower());
                    var propType = prop.PropertyType;
                    if (propType.Name.ToLower().Contains("nullable"))
                        propType = Nullable.GetUnderlyingType(propType);

                    if (objValue != null)
                    {
                        if (propType.IsEnum)
                            prop.SetValue(this, Enum.Parse(propType, objValue, true));
                        else if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
                        {   //KVP supports a list, split on comma and populate
                            var genType = propType.GetGenericArguments()[0];
                            var genListType = typeof(List<>).MakeGenericType(genType);
                            var ci = genListType.GetConstructor(new Type[] { });
                            var genericList = ci.Invoke(new object[] { });
                            List<object> results = new List<object>();
                            foreach (var val in objValue.Split(','))
                            {
                                (genericList as IList).Add(Convert.ChangeType(val, genType));
                            }
                            prop.SetValue(this, genericList);
                        }
                        else
                            prop.SetValue(this, Convert.ChangeType(objValue, propType));
                    }
                }
            }
        }
        private string Kvp(string key)
        {
            key += ":";
            string kvp = Arguments.SingleOrDefault(o => o.StartsWith(key));
            if (kvp == null) return null;
            return string.Join(":", kvp.Split(':').Skip(1));
        }
    }
}
