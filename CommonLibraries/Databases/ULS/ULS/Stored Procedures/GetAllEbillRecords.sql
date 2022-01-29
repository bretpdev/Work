CREATE PROCEDURE [dbo].[GetAllEbillRecords]
	
AS
	SELECT 
		EbillId,
		SSN,
		LoanSequence,
		BillingPreference,
		EmailAddress AS Email,
		UpdateSucceeded,
		UpdatedAt,
		ArcAdded,
		ArcAddedAt
	FROM
		EBill
	WHERE
		(UpdateSucceeded = 0 OR ArcAdded = 0) AND HadError = 0
RETURN 0
