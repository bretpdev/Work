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
    
    public partial class PermissionType
    {
        public PermissionType()
        {
            this.RoleAttributePermissions = new HashSet<RoleAttributePermission>();
        }
    
        public int PermissionTypeId { get; set; }
        public string PermissionTypeDescription { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> InactivatedAt { get; set; }
        public Nullable<int> InactivatedBy { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<RoleAttributePermission> RoleAttributePermissions { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Entity Entity1 { get; set; }
    }
}
