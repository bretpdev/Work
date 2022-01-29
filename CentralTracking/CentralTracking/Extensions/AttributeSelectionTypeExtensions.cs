using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class AttributeSelectionTypeExtensions
    {
        public static AttributeSelectionType AddByDescription(this DbSet<AttributeSelectionType> db, string description)
        {
            AttributeSelectionType selectType = new AttributeSelectionType();
            selectType.AttributeSelectionTypeDescription = description;
            selectType.CreatedBy = LoginHelper.CurrentUser.Id;
            db.Add(selectType);

            return selectType;
        }

        public static AttributeSelectionType FindByDescription(this DbSet<AttributeSelectionType> db, string description)
        {
            return db.Where(p => p.AttributeSelectionTypeDescription == description).SingleOrDefault();
        }
    }
}
