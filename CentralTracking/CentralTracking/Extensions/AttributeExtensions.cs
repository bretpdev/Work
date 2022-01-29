using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class AttributeExtensions
    {
        //TODO decide if this is really needed.
        //public static Attribute AddOrFind(this DbSet<Attribute> db, string description)
        //{
        //    Attribute attribute = db.Where(p => p.AttributeDescription == description).SingleOrDefault();
        //    if (attribute == null)
        //    {
        //        //TODO: add the correct data
        //        attribute = new Attribute();
        //        attribute.AttributeDescription = description;
        //        attribute.AttributeDataTypeId = 0;
        //        attribute.AttributeSelectionTypeId = 0;
        //        attribute.CreatedBy = LoginHelper.CurrentUser.Id;
        //        db.Add(attribute);
        //    }

        //    return attribute;
        //}

        public static Attribute FindByDescription(this IEnumerable<Attribute> attributes, string description)
        {
            return attributes.Where(p => p.AttributeDescription == description).SingleOrDefault();
        }
    }
}
