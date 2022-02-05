
CREATE PROCEDURE [dbo].[spFILE_UpdateLastProcessed]
	@FileName	Varchar(Max)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

UPDATE	dbo.FILE_DAT_FilesToMove
SET		LastProcessed = CURRENT_TIMESTAMP
WHERE	FileNameDescription = @FileName
	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spFILE_UpdateLastProcessed] TO [db_executor]
    AS [dbo];

