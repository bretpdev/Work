using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace CSLSLTRFED
{
    public static class ScreenHelper
    {
        /// <summary>
        /// Accesses TXX1Y to get the borrower relationship to the endorser
        /// </summary>
        /// <param name="accountNumber">Borrower account number</param>
        public static bool AccessBorrowerRelationshipScreen(ReflectionInterface ri, string accountNumber, int pageNumber = 1)
        {
            ri.FastPath("TX3Z/ITS26" + accountNumber);
            //if on a selection screen select the first loan.
            if (ri.CheckForText(1, 72, "TSX28"))
                ri.PutText(21, 12, "01", ReflectionInterface.Key.Enter);

            //Goes to the borrower relationship screen.
            ri.Hit(ReflectionInterface.Key.F4);

            //Something went wrong and there were no results.
            if (!ri.CheckForText(1, 74, "TXX1Y"))
            {
                Dialog.Info.Ok("Unable to find a co-borrower for the spousal consolidation loan. Please contact systems support.");
                return false;
            }

            while (pageNumber-- > 1)
                ri.Hit(ReflectionInterface.Key.F8);

            return true;
        }
    }
}