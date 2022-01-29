using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class EntityExtensions
    {
        public static Entity FindUser(this DbSet<Entity> db, string userName)
        {
            //TODO look at this more we may have to use a stored procedure.
            return db.Where(p => p.EntityAttributeValues.Where(EntityAttributeValueExtensions.FindByAttributeDescription("Windows User Name")).Single().Value.StringValue == userName).Single();
        }

        public static Entity AddByEntityNameAndType(this DbSet<Entity> db, string entityName, int entityTypeId)
        {
            Entity entity = new Entity();
            entity.EntityName = entityName;
            entity.EntityTypeId = entityTypeId;
            entity.CreatedBy = LoginHelper.CurrentUser.Id;
            db.Add(entity);

            return entity;
        }

        public static List<Entity> FindByEntityName(this DbSet<Entity> db, string entityName)
        {
            return db.Where(p => p.EntityName == entityName).ToList();
        }

        public static Entity FindByEntityId(this DbSet<Entity> db, int entityId)
        {
            return db.Where(p => p.EntityId == entityId).Single();
        }
    }
}
