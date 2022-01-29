using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    public static class EnumParser
    {
        public static T? Parse<T>(string value) where T : struct
        {
            value = value.ToLower();
            var type = typeof(T);
            foreach (var member in type.GetMembers())
            {
                string name = member.Name;
                var description = member.GetCustomAttributes(typeof(EnumDescriptionAttribute), false).SingleOrDefault() as EnumDescriptionAttribute;
                if (description != null)
                    name = description.Description;
                if (name.ToLower() == value)
                    return (T)Enum.Parse(type, member.Name);
            }
            return null;
        }

        public static string GetDescription<T>(T enumValue) where T : struct
        {
            var type = typeof(T);
            var member = type.GetMember(enumValue.ToString()).Single();
            var description = member.GetCustomAttributes(typeof(EnumDescriptionAttribute), false).SingleOrDefault() as EnumDescriptionAttribute;
            if (description != null)
                return description.Description;
            return member.Name;
        }
    }
}
