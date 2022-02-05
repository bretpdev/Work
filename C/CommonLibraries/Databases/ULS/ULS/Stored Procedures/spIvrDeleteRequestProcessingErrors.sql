
CREATE PROCEDURE spIvrDeleteRequestProcessingErrors 

AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM IvrRequestProcessingErrors WHERE RecNum > 0
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrDeleteRequestProcessingErrors] TO [UHEAA\UHEAAUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrDeleteRequestProcessingErrors] TO [db_executor]
    AS [dbo];

