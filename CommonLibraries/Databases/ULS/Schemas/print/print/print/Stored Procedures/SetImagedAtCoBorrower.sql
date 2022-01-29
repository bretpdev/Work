CREATE PROCEDURE [print].[SetImagedAtCoBorrower]
	@PrintProcessingId int
AS
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		[print].PrintProcessingCoBorrower
	SET
		ImagedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId

	SELECT
		@Time
RETURN 0