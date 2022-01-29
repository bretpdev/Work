using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.WinForms
{
    public interface IHasBehaviors
    {
        BehaviorInstallation InstalledBehaviors { get; set; }
    }
}
