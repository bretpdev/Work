using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    public partial class AttributeDataType : IHasCreationInfo
    {
        public void Inactivate()
        {
            InactivatedBy = LoginHelper.CurrentUser.Id;
        }

        public void Activate()
        {
            InactivatedBy = null;
        }

        public bool Active
        {
            get
            {
                return InternalActive.Value;
            }
            set
            {
                if (value)
                    Activate();
                else
                    Inactivate();
            }
        }
    }
}
