using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public static class LoginHelper
    {
        public static User CurrentUser { get; set; }

        public static void Login()
        {
            using (CentralTrackingEntities ct = new CentralTrackingEntities())
            {
                //TODO: finish login logic
                CurrentUser = new User(ct.Entities.Where(p => p.EntityId == 0).Single());
                //CurrentUser = new User(ct.Entities.FindUser(Environment.UserName));
            }
        }

    }
}
