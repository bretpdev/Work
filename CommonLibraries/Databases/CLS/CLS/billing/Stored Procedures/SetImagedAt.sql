CREATE PROCEDURE billing.[SetImagedAt]
	@PrintProcessingId int
AS
BEGIN
	DECLARE @Time DATETIME = GETDATE()
	UPDATE
		billing.PrintProcessing
	SET
		ImagedAt = @Time
	WHERE
		PrintProcessingId = @PrintProcessingId
END;

	SELECT
		@Time
RETURN 0