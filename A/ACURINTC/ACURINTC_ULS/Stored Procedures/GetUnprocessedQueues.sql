CREATE PROCEDURE [acurintc].[GetUnprocessedQueues]
AS
	
	SELECT
		[ProcessQueueId],
		[Queue],
		[SubQueue],
		[TaskControlNumber],
		[DemographicsSourceId],
		[SystemSourceID],
		[Ssn],
		[AccountNumber],
		[Address1],
		[Address2],
		[Address3],
		[City],
		[State],
		[Zipcode],
		[Country],
		[ForeignState],
		[OriginalAddressText],
		[OriginalAddressIsValid],
		[PrimaryPhone],
		[AdditionalInfo],
		[PendingVerificationDate],
		[CurrentVerificationDate],
		[OriginalHomePhone],
		[HomePhoneVerificationDate],
		[OriginalHomePhoneIsValid],
		[OriginalAltPhone],
		[AltPhoneVerificationDate],
		[OriginalAltPhoneIsValid],
		[OriginalWorkPhone],
		[WorkPhoneVerificationDate],
		[OriginalWorkPhoneIsValid],
		[OriginalMobilePhone],
		[MobilePhoneVerificationDate],
		[OriginalMobilePhoneIsValid],
		[CreatedAt],
		[CreatedBy],
		[ProcessedAt],
		[DeletedAt],
		[DeletedBy]
	FROM
		acurintc.ProcessQueue
	WHERE
		ProcessedAt IS NULL
		AND 
		DeletedAt IS NULL

RETURN 0