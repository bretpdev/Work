//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CentralTracking
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntityType
    {
        public EntityType()
        {
            this.EntityTypeAttributes = new HashSet<EntityTypeAttribute>();
            this.Entities = new HashSet<Entity>();
        }
    
        public int EntityTypeId { get; set; }
        public string EntityTypeDescription { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> InactivatedAt { get; set; }
        public Nullable<int> InactivatedBy { get; set; }
        private Nullable<bool> InternalActive { get; set; }
    
        public virtual ICollection<EntityTypeAttribute> EntityTypeAttributes { get; set; }
        public virtual ICollection<Entity> Entities { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Entity Entity1 { get; set; }
    }
}
