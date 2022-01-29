
CREATE PROCEDURE spIvrGetRequestProcessingErrors 

AS
BEGIN
	SET NOCOUNT ON;
	SELECT AccountNumber, Request FROM IvrRequestProcessingErrors
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrGetRequestProcessingErrors] TO [UHEAA\UHEAAUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrGetRequestProcessingErrors] TO [db_executor]
    AS [dbo];

