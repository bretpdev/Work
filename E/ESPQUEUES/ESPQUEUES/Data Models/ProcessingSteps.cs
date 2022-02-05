
namespace ESPQUEUES
{
    /// <summary>
    /// This is used for the Enrollment Review portion of the code to demarcate 
    /// the various steps of the process. Tracking the steps that have been 
    /// completed for a given ESP task allows us to ensure that the session 
    /// screens are not double-worked, should the script be re-run in recovery
    /// mode on a task that is processed in the Enrollment Review section.
    /// </summary>
    public class ProcessingSteps
    {
       public enum ProcessingStep
        {
            UpdateSeparationDate = 1, //Update the separation date in COMPASS if needed.
            ReviewWglStatusLoans, //See if there are any loans with W, G, or L status to be reviewed.
            CompleteStTasks, //Complete all ST tasks for the borrower.
            AddPlusLoansComment, //Review the loans and add a comment for PLUS loans if any are found.
        }
    }
}
