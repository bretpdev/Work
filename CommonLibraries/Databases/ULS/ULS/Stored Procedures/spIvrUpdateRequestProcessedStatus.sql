
CREATE PROCEDURE spIvrUpdateRequestProcessedStatus
	@RecNum		BigInt
AS
BEGIN
	UPDATE	IvrRequestTracking 
	SET		ProcessedDate = GETDATE()
	WHERE	RecNum = @RecNum
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrUpdateRequestProcessedStatus] TO [UHEAA\UHEAAUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrUpdateRequestProcessedStatus] TO [db_executor]
    AS [dbo];

