
CREATE PROCEDURE spIvrUpdatePaymentProcessedStatus
	@RecNum		BigInt
AS
BEGIN
	UPDATE	IvrCheckByPhone 
	SET		ProcessedDate = GETDATE()
	WHERE	RecNum = @RecNum
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrUpdatePaymentProcessedStatus] TO [UHEAA\UHEAAUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrUpdatePaymentProcessedStatus] TO [db_executor]
    AS [dbo];

