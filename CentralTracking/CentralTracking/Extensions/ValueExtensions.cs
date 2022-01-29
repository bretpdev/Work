using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class ValueExtensions
    {
        public static Value AddOrFindByStringValue(this DbSet<Value> db, string stringValue)
        {
            Value val = FindByStringValue(db, stringValue);
            if (val == null)
            {
                val = new Value() { StringValue = stringValue };
                db.Add(val);
            }

            return val;
        }

        public static Value FindByStringValue(this DbSet<Value> db, string stringValue)
        {
            return db.Where(p => p.StringValue == stringValue).SingleOrDefault();
        }
    }
}
