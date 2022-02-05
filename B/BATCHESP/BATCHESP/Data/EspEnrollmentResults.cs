namespace BATCHESP
{
    class EspEnrollmentResults
    {
        /// <summary>
        /// Result values for processing a given ESP task.
        /// </summary>
        public enum EspEnrollmentResult
        {
            Success,
            FailureContinueProcessing,
            FailureEndProcessing
        }
    }
}
