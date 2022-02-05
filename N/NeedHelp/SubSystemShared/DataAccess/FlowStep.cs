namespace SubSystemShared
{
    public class FlowStep
    {
        public string FlowID { get; set; }
        public int FlowStepSequenceNumber { get; set; }
        public bool AccessAlsoBasedOffBusinessUnit { get; set; }
        public string AccessKey { get; set; }
        public string NotificationType { get; set; }
        public int StaffAssignment { get; set; }
        public string StaffAssignmentCalculationID { get; set; }
        public string ControlDisplayText { get; set; }
        public string Description { get; set; }
        public string DataValidationID { get; set; }
        public string Status { get; set; }
        public string StaffAssignmentLegalName { get; set; }
    }
}
