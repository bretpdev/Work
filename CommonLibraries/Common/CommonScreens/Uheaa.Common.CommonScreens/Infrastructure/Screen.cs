using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.CommonScreens
{
    public abstract class Screen
    {
        protected IReflectionSession session;
        public Screen(IReflectionSession session)
        {
            this.session = session;
            AccessLog.RegisterAccess(this.GetType().Name);
        }
        public abstract void Navigate();
    }
}
