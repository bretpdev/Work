CREATE PROCEDURE [dbo].[ExceptionLogResolve]
	@ExceptionLogId int
AS
	update ExceptionLogs 
	   set ResolutionDate = getdate(), ResolvedBy = SYSTEM_USER
	 where ExceptionLogId = @ExceptionLogId
RETURN 0
