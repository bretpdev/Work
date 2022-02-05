CREATE PROCEDURE [emailbatch].[UpdateEmailSentAt]
	@EmailProcessingId int
AS
	UPDATE
		[emailbatch].EmailProcessing
	SET
		EmailSentAt = GETDATE()
	WHERE
		EmailProcessingId = @EmailProcessingId
RETURN 0
