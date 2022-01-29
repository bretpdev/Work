CREATE PROCEDURE [aesrcvdial].[InsertSingleRecord]
	@TargetsId VARCHAR(9),
	@FileName VARCHAR(250),
	@QueueRegion VARCHAR(3),
	@CriticalTaskIndicator CHAR(1),
	@BorrowersName VARCHAR(50),
	@BorrowersPaymentAmount VARCHAR(9),
	@BorrowersOutstandingBalance VARCHAR(9),
	@BorrowersAccountNumber VARCHAR(10),
	@TargetsDateLastAttempt VARCHAR(10),
	@TargetsDateLastContact VARCHAR(10),
	@TargetsRelationshipToBorrower CHAR(1),
	@TargetsName VARCHAR(50),
	@TargetsZip VARCHAR(14),
	@TargetsHomePhoneType CHAR(1),
	@TargetsHomePhone VARCHAR(17),
	@TargetsAltPhoneType CHAR(1),
	@TargetsAltPhone VARCHAR(17),
	@TargetsOtherPhoneType CHAR(1),
	@TargetsOtherPhone VARCHAR(17),
	@TargetsTCPAConsentForHomePhone CHAR(1),
	@TargetsTCPAConsentForAltPhone CHAR(1),
	@TargetsTCPAConsentForOtherPhone CHAR(1),
	@RegardsToNumberOfDaysDelinquent VARCHAR(5),
	@RegardsToName VARCHAR(50),
	@RegardsToSkipStartDate VARCHAR(10),
	@PreviouslyRehabbedIndicator CHAR(1)
AS
	IF NOT EXISTS (SELECT * FROM aesrcvdial.OnelinkDialFileInput WHERE TargetsId = @TargetsId AND [FileName] = @FileName AND QueueRegion = @QueueRegion AND BorrowersAccountNumber = @BorrowersAccountNumber AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE) AND DeletedAt IS NULL)
		BEGIN
			INSERT INTO aesrcvdial.OnelinkDialFileInput(TargetsId, [FileName], QueueRegion, CriticalTaskIndicator, BorrowersName, BorrowersPaymentAmount, BorrowersOutstandingBalance, BorrowersAccountNumber, TargetsDateLastAttempt, TargetsDateLastContact, TargetsRelationshipToBorrower, TargetsName, TargetsZip, TargetsHomePhoneType, TargetsHomePhone, TargetsAltPhoneType, TargetsAltPhone, TargetsOtherPhoneType, TargetsOtherPhone, TargetsTCPAConsentForHomePhone, TargetsTCPAConsentForAltPhone, TargetsTCPAConsentForOtherPhone, RegardsToNumberOfDaysDelinquent, RegardsToName, RegardsToSkipStartDate, PreviouslyRehabbedIndicator)
			VALUES(@TargetsId, @FileName, @QueueRegion, @CriticalTaskIndicator, @BorrowersName, @BorrowersPaymentAmount, @BorrowersOutstandingBalance, @BorrowersAccountNumber, @TargetsDateLastAttempt, @TargetsDateLastContact, @TargetsRelationshipToBorrower, @TargetsName, @TargetsZip, @TargetsHomePhoneType, @TargetsHomePhone, @TargetsAltPhoneType, @TargetsAltPhone, @TargetsOtherPhoneType, @TargetsOtherPhone, @TargetsTCPAConsentForHomePhone, @TargetsTCPAConsentForAltPhone, @TargetsTCPAConsentForOtherPhone, @RegardsToNumberOfDaysDelinquent, @RegardsToName, @RegardsToSkipStartDate, @PreviouslyRehabbedIndicator)

			SELECT SCOPE_IDENTITY()
		END
	ELSE
		SELECT 0;