using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class AttributeAllowedValueExtensions
    {
        public static AttributeAllowedValue AddByValue(this DbSet<AttributeAllowedValue> db, string stringValue, string attributeDescription)
        {
            AttributeAllowedValue aav = new AttributeAllowedValue();
            using (CentralTrackingEntities ct = new CentralTrackingEntities())
            {
                Value v = ct.Values.AddOrFindByStringValue(stringValue);
                Attribute a = ct.Attributes.FindByDescription(attributeDescription);

                aav.ValueId = v.ValueId;
                aav.AttributeId = a.AttributeId;
                aav.CreatedBy = LoginHelper.CurrentUser.Id;

                ct.AttributeAllowedValues.Add(aav);
            }

            return aav;
        }

        public static AttributeAllowedValue FindByValue(this DbSet<AttributeAllowedValue> db, string stringValue, string attributeDescription)
        {
            AttributeAllowedValue aav = new AttributeAllowedValue();
            using (CentralTrackingEntities ct = new CentralTrackingEntities())
            {
                Value v = ct.Values.FindByStringValue(stringValue);
                Attribute a = ct.Attributes.FindByDescription(attributeDescription);
            }

            return aav;
        }


    }
}
