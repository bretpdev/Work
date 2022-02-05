CREATE PROCEDURE dbo.spBORG_AddRecordToOneLINKCheckByPhoneData 
	@UserID						VARCHAR(50),
	@ContactType				VARCHAR(2),
	@PaymentAmount				NUMERIC(18,2)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.BORG_DAT_OneLINKCheckByPhoneData
	(UserID, ContactType, PaymentAmount)
	VALUES
	(@UserID, @ContactType, @PaymentAmount)
	
END