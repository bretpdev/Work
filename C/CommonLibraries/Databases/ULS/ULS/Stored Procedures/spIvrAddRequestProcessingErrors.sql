
CREATE PROCEDURE [dbo].[spIvrAddRequestProcessingErrors] 
	@AccountNumber		CHAR(10),
	@ARC				CHAR(5),
	@Desc				VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO IvrRequestProcessingErrors (AccountNumber, Request) 
	VALUES (@AccountNumber, @ARC+' - '+@Desc)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrAddRequestProcessingErrors] TO [UHEAA\UHEAAUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrAddRequestProcessingErrors] TO [db_executor]
    AS [dbo];

