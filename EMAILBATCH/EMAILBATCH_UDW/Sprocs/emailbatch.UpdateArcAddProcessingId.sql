CREATE PROCEDURE [emailbatch].[UpdateArcAddProcessingId]
	@ArcAddProcessingId INT,
	@EmailProcessingId int
AS
		UPDATE
		[emailbatch].EmailProcessing
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		EmailProcessingId = @EmailProcessingId
RETURN 0
