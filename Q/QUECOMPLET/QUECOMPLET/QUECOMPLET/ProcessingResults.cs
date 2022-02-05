namespace QUECOMPLET
{
    public class ProcessingResults
    {
        /// <summary>
        /// Result values for processing the closure of a task.  
        /// There are several known issues that are handled by the
        /// code, so the UnknownIssue is the one that will cause
        /// a script run to be considered a failure
        /// </summary>
        public enum ProcessingResult
        {
            Success, 
            KnownIssue,
            UnknownIssue
        }

    }
}