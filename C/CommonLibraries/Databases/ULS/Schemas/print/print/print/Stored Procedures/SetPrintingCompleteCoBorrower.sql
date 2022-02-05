CREATE PROCEDURE [print].[SetPrintingCompleteCoBorrower]
(
	@PrintProcessingId int
)
AS
BEGIN
	UPDATE [print].PrintProcessingCoBorrower
	SET
		PrintedAt = getdate()
	WHERE
		PrintProcessingId = @PrintProcessingId
END;


