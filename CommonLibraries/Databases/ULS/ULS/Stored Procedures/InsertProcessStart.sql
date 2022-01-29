CREATE PROCEDURE [dbo].[InsertProcessStart]
    @ScriptId varchar(10),
    @Region varchar(11),
    @RunBy varchar(50)
AS
    INSERT INTO [dbo].[ProcessLogs]([StartedOn],[ScriptId],[Region],[RunBy])
    VALUES(GETDATE(), @ScriptId, @Region, @RunBy)

    SELECT 
        [ProcessLogId],
        [StartedOn] AS StartTime
    FROM
        [dbo].[ProcessLogs]
    WHERE 
        [ProcessLogId] = SCOPE_IDENTITY()
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertProcessStart] TO [db_executor]
    AS [dbo];

