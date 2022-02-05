CREATE PROCEDURE [dbo].[ExceptionLogInsert]
    @ProcessLogId INT,
	@ProcessNotificationId int = null,
	@ExceptionType nvarchar(max)= 'Not Provided',
	@AssemblyLocation nvarchar(max) = 'Not Provided',
	@AssemblyFullName nvarchar(max) = 'Not Provided',
	@AssemblyLastModified datetime = null,
	@ExceptionSource nvarchar(max)= 'Not Provided',
	@ExceptionMessage nvarchar(max)= 'Not Provided',
	@StackTrace nvarchar(max)= 'Not Provided',
	@FullDetails nvarchar(max)= 'Not Provided'

AS
	SET NOCOUNT ON

	IF @AssemblyLastModified IS NULL
		SET @AssemblyLastModified = GETDATE()

	INSERT INTO ExceptionLogs (ProcessLogId, ProcessNotificationId, ExceptionType, ExceptionSource, AssemblyLocation, AssemblyFullName, AssemblyLastModified, ExceptionMessage, StackTrace, FullDetails)
	VALUES (@ProcessLogId, @ProcessNotificationId, @ExceptionType, @ExceptionSource, @AssemblyLocation, @AssemblyFullName, @AssemblyLastModified, @ExceptionMessage, @StackTrace, @FullDetails)

	SET NOCOUNT OFF
	SELECT CAST(SCOPE_IDENTITY() AS INT)
RETURN 0
