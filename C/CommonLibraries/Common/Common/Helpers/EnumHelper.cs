using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Uheaa.Common
{
    public static class EnumHelper
    {
        public static A GetAttribute<E, A>(this E e)
            where A : Attribute
            where E : struct
        {
            FieldInfo f = e.GetType().GetField(e.ToString());
            object[] attributes = f.GetCustomAttributes(false);
            foreach (object a in attributes)
                if (a is A)
                    return (A)a;
            return null;
        }
        public static DescriptionAttribute GetDescription<E>(this E e) where E : struct
        {
            return e.GetAttribute<E, DescriptionAttribute>();
        }
        public static Dictionary<E, DescriptionAttribute> Enums<E>()
            where E : struct
        {
            return EnumsWithAttributes<E, DescriptionAttribute>();
        }
        public static Dictionary<E, A> EnumsWithAttributes<E, A>()
            where A : Attribute
            where E : struct
        {
            Dictionary<E, A> enums = new Dictionary<E, A>();
            FieldInfo[] fields = typeof(E).GetFields();
            foreach (FieldInfo f in fields)
            {
                if (f.Name == "value__")
                    continue;
                object[] attributes = f.GetCustomAttributes(false);
                foreach (object a in attributes)
                    if (a is A)
                        enums[(E)Enum.Parse(typeof(E), f.Name)] = (A)a;
            }
            return enums;
        }
        public static E ToEnum<E>(this string s) where E : struct
        {
            return EnumsWithAttributes<E, DescriptionAttribute>().Where(o => o.Value.Description == s).Single().Key;
        }
        public static E? ToEnumNullable<E>(this string s) where E : struct
        {
            var results = EnumsWithAttributes<E, DescriptionAttribute>().Where(o => o.Value.Description == s);
            if (results.Count() == 1)
                return results.Single().Key;
            return null;
        }
        public static E? ToEnumNullable<E, A>(this string s, Func<A, string> func)
            where E : struct
            where A : Attribute
        {
            var result = EnumsWithAttributes<E, A>().Where(o => func(o.Value) == s);
            if (result.Count() > 0)
                return result.Single().Key;
            return null;
        }

        public static E ToEnum<E, A>(this string s, Func<A, string> func) where E : struct where A : Attribute
        {
            return s.ToEnumNullable<E, A>(func).Value;
        }

        /// <summary>
        /// Check to see if a flags enumeration has a specific flag set.
        /// </summary>
        /// <param name="variable">Flags enumeration to check</param>
        /// <param name="value">Flag to check for</param>
        /// <returns></returns>
        public static bool HasFlag(this Enum variable, Enum value)
        {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException("value");

            // Not as good as the .NET 4 version of this function, but should be good enough
            if (!Enum.IsDefined(variable.GetType(), value))
            {
                throw new ArgumentException(string.Format(
                    "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
                    value.GetType(), variable.GetType()));
            }

            ulong num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(variable) & num) == num);

        }

        public static bool InFlag(this Enum variable, Enum flag)
        {
            return flag.HasFlag(variable);
        }
    }
}
