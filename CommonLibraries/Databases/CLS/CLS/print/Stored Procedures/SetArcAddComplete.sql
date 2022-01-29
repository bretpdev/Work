CREATE PROCEDURE [print].[SetArcAddComplete]
(
	@PrintProcessingId int,
	@ArcAddProcessingId int
)
AS
BEGIN
	UPDATE [print].PrintProcessing
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		PrintProcessingId = @PrintProcessingId
END;