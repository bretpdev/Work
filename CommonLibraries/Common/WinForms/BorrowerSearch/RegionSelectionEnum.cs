using System.ComponentModel;

namespace Uheaa.Common.WinForms
{
    public enum RegionSelectionEnum
    {
        [Description("UHEAA Borrowers")]
        Uheaa,
        [Description("OneLINK Borrowers")]
        OneLINK,
        [Description("All Borrowers")]
        All
    }
}