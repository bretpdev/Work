using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEYIDCHNG
{
    public enum ApprovalResult
    {
        BorrowerNotFound,
        NoChangesFound,
        ChangesMadeNoArc,
        ChangedMadeArcSuccessful,
        ErrorMakingChanges,
        ErrorMakingChangesBadAccess
    }
}
