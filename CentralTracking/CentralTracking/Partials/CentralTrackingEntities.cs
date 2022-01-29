using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public partial class CentralTrackingEntities
    {
        public override int SaveChanges()
        {
            //properly set all created by attributes
            foreach (var eav in ChangeTracker.Entries<IHasCreationInfo>().Where(P => P.State != System.Data.EntityState.Unchanged))
                eav.Entity.CreatedBy = LoginHelper.CurrentUser.Id;
            //re-detect the changes we've made.
            ChangeTracker.DetectChanges();
            try
            {
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                var exception = ex as System.Data.Entity.Infrastructure.DbUpdateException;
                throw;    
            
            }
        }

        public new System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
