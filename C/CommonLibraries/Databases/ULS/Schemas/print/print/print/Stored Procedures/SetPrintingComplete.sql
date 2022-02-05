CREATE PROCEDURE [print].[SetPrintingComplete]
(
	@PrintProcessingId int
)
AS
BEGIN
	UPDATE [print].PrintProcessing
	SET
		PrintedAt = getdate()
	WHERE
		PrintProcessingId = @PrintProcessingId
END;


