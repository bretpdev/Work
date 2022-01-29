
CREATE PROCEDURE spGENR_GetFileSystemObject 
	-- Add the parameters for the stored procedure here
	@Key		VARCHAR(50),
	@TestMode	BIT,
	@Region		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	Path
	FROM	dbo.GENR_DAT_EnterpriseFileSystem 
	WHERE	[Key] = @Key
			AND TestMode = @TestMode
			AND Region = @Region
END

GO
GRANT VIEW DEFINITION
    ON OBJECT::[dbo].[spGENR_GetFileSystemObject] TO [UHEAA\SystemAnalysts]
    AS [dbo];


GO
GRANT TAKE OWNERSHIP
    ON OBJECT::[dbo].[spGENR_GetFileSystemObject] TO [UHEAA\SystemAnalysts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetFileSystemObject] TO [UHEAA\SystemAnalysts]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetFileSystemObject] TO [db_executor]
    AS [dbo];


GO
GRANT CONTROL
    ON OBJECT::[dbo].[spGENR_GetFileSystemObject] TO [UHEAA\SystemAnalysts]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[spGENR_GetFileSystemObject] TO [UHEAA\SystemAnalysts]
    AS [dbo];

