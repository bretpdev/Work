using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class AttributeDataTypeExtensions
    {
        public static AttributeDataType AddByDescription(this DbSet<AttributeDataType> db, string description)
        {
            AttributeDataType dataType = new AttributeDataType();
            dataType.AttributeDataTypeDescription = description;
            dataType.CreatedBy = LoginHelper.CurrentUser.Id;
            db.Add(dataType);
            return dataType;
        }

        public static AttributeDataType FindByDescription(this DbSet<AttributeDataType> db, string description)
        {
            return db.Where(p => p.AttributeDataTypeDescription == description).SingleOrDefault();
        }
    }
}
