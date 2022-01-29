using System.Collections.Generic;
using System.Linq;

namespace BATCHESP
{
    public class NonSelectionHelper
    {
        public List<NonSelectionReason> Reasons { get; private set; }
        public NonSelectionHelper(DataAccess da)
        {
            Reasons = da.GetNonSelectionReasons();
        }

        /// <summary>
        /// Returns true if the given reason and course exist in the database.
        /// </summary>
        public bool ReasonExists(string reason, string course)
        {
            return Reasons.Any(r => reason.StartsWith(r.Reason) && r.Course == course);
        }
    }
}
