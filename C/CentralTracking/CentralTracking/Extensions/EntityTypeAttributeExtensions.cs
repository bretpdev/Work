using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class EntityTypeAttributeExtensions
    {
        public static EntityTypeAttribute FindByEntityTypeAndAttribute(this DbSet<EntityTypeAttribute> entityTypeAttributes, int entityTypeId, int attributeId)
        {
            return entityTypeAttributes.Where(p => (p.EntityTypeId == entityTypeId && p.AttributeId == attributeId)).SingleOrDefault();
        }

        public static List<EntityTypeAttribute> FindByEntityType(this DbSet<EntityTypeAttribute> entityTypeAttributes, int entityTypeId)
        {
            return entityTypeAttributes.Where(p => p.EntityTypeId == entityTypeId ).ToList();
        }

        public static List<EntityTypeAttribute> FindByAttrubite(this DbSet<EntityTypeAttribute> entityTypeAttributes, int attributeId)
        {
            return entityTypeAttributes.Where(p => p.AttributeId == attributeId).ToList();
        }
    }
}
