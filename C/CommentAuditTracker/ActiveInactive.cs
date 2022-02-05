using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.WinForms;

///This file contains helper methods, controls, and enums for dealing with Active/Inactive statuses.
namespace CommentAuditTracker
{
    public enum ActiveInactive
    {
        Active = 1,  //true-bit
        Inactive = 0 //false-bit
    }
    public enum ActiveInactiveAll
    {
        All = 2,
        Active = 1,
        Inactive = 0,
    }

    [System.ComponentModel.DesignerCategory("")] //stop visual studio from trying to open this file in designer
    public class ActiveInactiveCycleButton : EnumCycleButton<ActiveInactive> { }
    public class ActiveInactiveAllCycleButton : EnumCycleButton<ActiveInactiveAll> { }

    public static class ActiveInactiveExtensions
    {
        public static bool ToBool(this ActiveInactive ai)
        {
            return ai == ActiveInactive.Active ? true : false;
        }
        public static bool? ToBool(this ActiveInactiveAll aia)
        {
            if (aia == ActiveInactiveAll.Active)
                return true;
            else if (aia == ActiveInactiveAll.Inactive)
                return false;
            else
                return null;
        }
    }
}
