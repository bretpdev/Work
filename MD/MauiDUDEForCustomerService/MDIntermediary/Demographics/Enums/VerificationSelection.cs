using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public enum VerificationSelection
    {
        NoChange,
        ValidNoChange,
        ValidWithChange,
        InvalidNoChange,
        RefusedNoChange,
        ValidWithChangeAndInvalidateFirst
    }
    public static class VerificationSelectionExtensions
    {
        public static bool IsValid(this VerificationSelection selection)
        {
            return selection == VerificationSelection.ValidNoChange
                || selection.IsValidWithChanges();
        }
        public static bool IsValidWithChanges(this VerificationSelection selection)
        {
            return selection == VerificationSelection.ValidWithChange
                || selection == VerificationSelection.ValidWithChangeAndInvalidateFirst;
        }
        public static string ToVRN(this VerificationSelection selection)
        {
            if (selection.IsValid())
                return "V";
            else if (selection == VerificationSelection.RefusedNoChange)
                return "R";
            return "N";
        }
    }
}
