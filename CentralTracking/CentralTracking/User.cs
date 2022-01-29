using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Entity Entity { get; set; }

        public User(Entity entity)
        {
            //TODO: Remove the hard coded text
            Entity = entity;
            UserName = "DCRUser";// entity.EntityAttributeValues.GetByAttributeDescription("WindowsUserName").Value.StringValue;
            Id = entity.EntityId;
        }
    }
}
