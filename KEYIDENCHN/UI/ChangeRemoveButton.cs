using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Uheaa.Common.WinForms;

namespace KEYIDENCHN
{
    public enum ChangeRemove
    {
        [Description("Keep Value")]
        Change,
        [Description("Remove Value")]
        Remove
    }
    public class ChangeRemoveButton : EnumCycleButton<ChangeRemove>
    {
    }
}
