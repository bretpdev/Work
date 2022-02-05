using System;
using System.Reflection;

namespace Uheaa.Common
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns def if o is null.  Otherwise returns the result of getter.
        /// </summary>
        public static Property IsNull<Source, Property>(this Source o, Func<Source, Property> getter, Property def = default(Property)) where Source : class
        {
            if (o == null)
                return def;
            else
                return getter(o);
        }
        public static void IfExists<Source>(this Source o, Action<Source> action)
        {
            if (o != null)
                action(o);
        }
        public static bool IsEqual<T>(this T obj1, T obj2)
        {
            foreach (PropertyInfo item1 in obj1.GetType().GetProperties())
            {
                foreach (PropertyInfo item2 in obj2.GetType().GetProperties())
                {
                    if (item1.Name == item2.Name)
                    {
                        var value1 = obj1.GetType().GetProperty(item1.Name).GetValue(obj1, null);
                        var value2 = obj2.GetType().GetProperty(item2.Name).GetValue(obj2, null);

                        if (value1 == null && value2 == null)
                            break;

                        if (value1 == null || value2 == null)
                            return false;
                        

                        if (!value1.Equals(value2))
                            return false;
                        else
                            break;
                    }
                }
            }
            return true;
        }
    }
}
