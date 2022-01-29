
CREATE PROCEDURE [dbo].[spSYSA_AddAppAndTrainCoordError]
	@WindowsUserName		VARCHAR(50),
	@TimeStamp				DATETIME,
	@Message				VARCHAR(MAX),
	@StackTrace				VARCHAR(MAX),
	@FilePathName			VARCHAR(MAX),
	@Action					VARCHAR(50)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO		dbo.SYSA_DAT_AppAndTrainCoordErrors 
					(WindowsUserName, [TimeStamp], [Message], StackTrace, FilePathName, [Action])
	VALUES			(@WindowsUserName, @TimeStamp, @Message, @StackTrace, @FilePathName, @Action)
END

GRANT EXECUTE ON [dbo].[spSYSA_AddAppAndTrainCoordError] TO db_executor