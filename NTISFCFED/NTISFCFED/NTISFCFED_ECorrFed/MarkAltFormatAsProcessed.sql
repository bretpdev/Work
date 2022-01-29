CREATE PROCEDURE [dbo].[MarkAltFormatAsProcessed]
	@DocumentDetailsId int
AS
	UPDATE
		DocumentDetails
	SET
		CorrespondenceFormatSentDate = GETDATE()
	WHERE
		DocumentDetailsId = @DocumentDetailsId
RETURN 0
