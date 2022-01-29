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
