using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INCIDENTRP
{
    public class IncidentBase
    {
        public virtual bool IsComplete() => true; //default to true, overridable when necessary
    }
}
