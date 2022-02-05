CREATE PROCEDURE [print].[SetImagedAt]
	@PrintProcessingId int
AS
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		[print].PrintProcessing
	SET
		ImagedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId

	SELECT
		@Time
RETURN 0