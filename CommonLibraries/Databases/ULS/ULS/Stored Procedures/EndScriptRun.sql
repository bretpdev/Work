CREATE PROCEDURE [dbo].[EndScriptRun]
    @ProcessLogId int,
    @ExceptionId int = null
AS
    UPDATE
        [dbo].[ProcessLogs]
    SET 
        EndedOn = GETDATE()
    WHERE 
        ProcessLogId = @ProcessLogId
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EndScriptRun] TO [db_executor]
    AS [dbo];

