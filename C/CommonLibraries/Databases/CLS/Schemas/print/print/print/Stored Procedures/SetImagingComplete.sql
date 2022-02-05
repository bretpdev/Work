CREATE PROCEDURE [print].[SetImagingComplete]
(
	@PrintProcessingId int
)
AS
BEGIN
	UPDATE [print].PrintProcessing
	SET
		ImagedAt = getdate()
	WHERE
		PrintProcessingId = @PrintProcessingId
END;