CREATE PROCEDURE [dbo].[UpdateEmailedAt]
	@DocumentDetailsId int 
AS
	
	UPDATE 
		DD
	SET 
		DD.EmailSent = GETDATE()
	FROM 
		DocumentDetails DD
	WHERE 
		DD.DocumentDetailsId = @DocumentDetailsId
		
RETURN 0