CREATE PROCEDURE [dbo].[MarkEbillArcAdded]
		@SSN CHAR(9),
	@BillingPreference CHAR(1)
AS
	UPDATE
		EBill
	SET
		ArcAdded = 1,
		ArcAddedAt = GETDATE()
	WHERE 
		SSN = @SSN
		AND BillingPreference = @BillingPreference
		AND UpdateSucceeded = 1
		AND ArcAdded = 0
RETURN 0
