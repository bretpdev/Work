using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Uheaa.Common
{
    public static class ReflectionExtensions
    {
        public static bool IsAnonymous(this object o)
        {
            if (o == null)
                return false;
            return o.GetType().IsAnonymous();
        }

        public static bool IsAnonymous(this Type type)
        {
            if (type == null)
                return false;
            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public static bool HasAttribute<T>(this PropertyInfo pi)
        {
            return pi.GetCustomAttributes(true).Any(o => o is T);
        }

        public static RET GetAttributeValue<ATT, RET>(this PropertyInfo pi, Func<ATT, RET> isNull, RET nullValue) where ATT : class
        {
            return pi.GetCustomAttributes(true).Filter<ATT>().SingleOrDefault().IsNull(isNull, nullValue);
        }

        public static RET GetAttributeValue<ATT, RET>(this MemberInfo mi, Func<ATT, RET> isNull, RET nullValue) where ATT : class
        {
            return mi.GetCustomAttributes(true).Filter<ATT>().SingleOrDefault().IsNull(isNull, nullValue);
        }
    }
}
