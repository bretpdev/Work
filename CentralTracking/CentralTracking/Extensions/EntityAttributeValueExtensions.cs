using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class EntityAttributeValueExtensions
    {
        public static EntityAttributeValue GetByAttributeDescription(this IEnumerable<EntityAttributeValue> collection, string description)
        {
            return collection.Where(FindByAttributeDescription(description)).SingleOrDefault();
        }

        public static Func<EntityAttributeValue, bool> FindByAttributeDescription(string description)
        {
            return p => p.Attribute.AttributeDescription == description;
        }
    }
}
