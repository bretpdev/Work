using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class EntityTypeExtensions
    {
        public static EntityType AddOrFind(this DbSet<EntityType> db, string description)
        {
            EntityType entityType = EntityTypeExtensions.FindByDescription(db, description);
            if (entityType == null)
            {
                entityType = new EntityType();
                entityType.EntityTypeDescription = description;
                entityType.CreatedBy = LoginHelper.CurrentUser.Id;
                db.Add(entityType);
            }

            return entityType;
        }

        public static EntityType FindByDescription(this DbSet<EntityType> db, string description)
        {
            return db.Where(p => p.EntityTypeDescription == description).SingleOrDefault();
        }
    } 
}
