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
    
    public partial class RoleAttributePermission
    {
        public int RoleAttributePermissionId { get; set; }
        public int RoleId { get; set; }
        public int AttributeId { get; set; }
        public int PermissionTypeId { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> InactivatedAt { get; set; }
        public Nullable<int> InactivatedBy { get; set; }
        public bool Active { get; set; }
    
        public virtual Attribute Attribute { get; set; }
        public virtual PermissionType PermissionType { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Entity Entity1 { get; set; }
    }
}