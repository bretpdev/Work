using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CentralTracking
{
    interface IHasCreationInfo 
    {
        int CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
