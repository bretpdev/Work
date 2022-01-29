namespace AESRCVDIAL
{
    public class FileData
    {
        // (starting position in the file, how many characters to read)
        public string FileName { get; set; }
        public string TargetsId { get; set; } //(1,9)
        public string QueueRegion { get; set; } //(10,3)
        public string CriticalTaskIndicator { get; set; } //(29,1)
        public string BorrowersName { get; set; } //(76,50)
        public string BorrowersPaymentAmount { get; set; } //(141,9)
        public string BorrowersOutstandingBalance { get; set; } //(150,9)
        public string BorrowersAccountNumber { get; set; } //(203,10)
        public string TargetsDateLastAttempt { get; set; } //(220,8)
        public string TargetsDateLastContact { get; set; } //(228,8)
        public string TargetsRelationshipToBorrower { get; set; } //(224,1)
        public string TargetsName { get; set; } //(245,50)
        public string TargetsZip { get; set; } //(397,14)
        public string TargetsHomePhoneType { get; set; } //(441,1)
        public string TargetsHomePhone { get; set; } //(442,17)
        public string TargetsAltPhoneType { get; set; } //(459,1)
        public string TargetsAltPhone { get; set; } //(460,17)
        public string TargetsOtherPhoneType { get; set; } //(477,1)
        public string TargetsOtherPhone { get; set; } //(478,17)
        public string TargetsTCPAConsentForHomePhone { get; set; } //(495,1)
        public string TargetsTCPAConsentForAltPhone { get; set; } //(496,1)
        public string TargetsTCPAConsentForOtherPhone { get; set; } //(497,1)
        public string RegardsToNumberOfDaysDelinquent { get; set; } //(510,5)
        public string RegardsToName { get; set; } //(525,50)
        public string RegardsToSkipStartDate { get; set; } //(724,8)
        public string PreviouslyRehabbedIndicator { get; set; } //(761,1)
    }
}