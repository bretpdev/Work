CREATE PROCEDURE [aesrcvdial].[GetRecordsByDate]
	@AddedAt DATETIME
AS
	SELECT
		TargetsId,
		[FileName],
		QueueRegion,
		CriticalTaskIndicator,
		BorrowersName,
		BorrowersPaymentAmount,
		BorrowersOutstandingBalance,
		BorrowersAccountNumber,
		TargetsDateLastAttempt,
		TargetsDateLastContact,
		TargetsRelationshipToBorrower,
		TargetsName,
		TargetsZip,
		TargetsHomePhoneType,
		TargetsHomePhone,
		TargetsAltPhoneType,
		TargetsAltPhone,
		TargetsOtherPhoneType,
		TargetsOtherPhone,
		TargetsTCPAConsentForHomePhone,
		TargetsTCPAConsentForAltPhone,
		TargetsTCPAConsentForOtherPHone,
		RegardsToNumberOfDaysDelinquent,
		RegardsToName,
		RegardsToSkipStartDate,
		PreviouslyRehabbedIndicator
	FROM
		aesrcvdial.OnelinkDialFileInput
	WHERE
		CAST(AddedAt AS DATE) = CAST(@AddedAt AS DATE)
		AND DeletedAt IS NULL