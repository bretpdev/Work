CREATE PROCEDURE [dbo].[DeleteApplication]
	@ApplicationId int,
	@RemovedBy int
AS
	UPDATE
		Applications
	SET
		RemovedBy = @RemovedBy,
		RemovedOn = GETDATE()
	WHERE
		ApplicationId = @ApplicationId
RETURN 0

GRANT EXECUTE ON DeleteApplication TO db_executor