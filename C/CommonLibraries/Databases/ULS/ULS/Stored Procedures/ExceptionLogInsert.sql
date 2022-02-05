CREATE PROCEDURE [dbo].[ExceptionLogInsert]
    @ProcessLogId INT,
	@ProcessNotificationId int,
	@ExceptionType nvarchar(max),
	@AssemblyLocation nvarchar(max),
	@AssemblyFullName nvarchar(max),
	@AssemblyLastModified datetime,
	@ExceptionSource nvarchar(max),
	@ExceptionMessage nvarchar(max),
	@StackTrace nvarchar(max),
	@FullDetails nvarchar(max)

AS
	SET NOCOUNT ON

	INSERT INTO ExceptionLogs (ProcessLogId, ProcessNotificationId, ExceptionType, ExceptionSource, AssemblyLocation, AssemblyFullName, AssemblyLastModified, ExceptionMessage, StackTrace, FullDetails)
	VALUES (@ProcessLogId, @ProcessNotificationId, @ExceptionType, @ExceptionSource, @AssemblyLocation, @AssemblyFullName, @AssemblyLastModified, @ExceptionMessage, @StackTrace, @FullDetails)

	SET NOCOUNT OFF
	SELECT CAST(SCOPE_IDENTITY() AS INT)
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ExceptionLogInsert] TO [db_executor]
    AS [dbo];

