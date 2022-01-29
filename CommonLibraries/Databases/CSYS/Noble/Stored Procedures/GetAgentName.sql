CREATE PROCEDURE [Noble].GetAgentName
(
	@AgentCode VARCHAR(7)
)
AS
BEGIN
	SELECT U.Username
	FROM CSYS.Noble.UserList U
	WHERE U.TSR = @AgentCode
END;
GO
GRANT EXECUTE
    ON OBJECT::[Noble].[GetAgentName] TO [db_executor]
    AS [dbo];

