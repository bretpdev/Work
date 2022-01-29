CREATE PROCEDURE [dbo].[ArcAdd_SetRecordAsProcessed]
	@ArcAddProcessingId int
AS
BEGIN

	SET NOCOUNT ON;
	
	UPDATE
		ArcAddProcessing
	SET
		ProcessedAt = GETDATE()
	WHERE
		ArcAddProcessingId = @ArcAddProcessingId

END
GO
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_SetRecordAsProcessed] TO [db_executor]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_SetRecordAsProcessed] TO [UHEAA\SystemAnalysts]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_SetRecordAsProcessed] TO [UHEAA\CornerStoneUsers]
    AS [dbo];

