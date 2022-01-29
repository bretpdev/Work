CREATE PROCEDURE [Noble].[GetAgentOtherId]
(
	@AgentCode VARCHAR(50)
)

AS
BEGIN

	SELECT DISTINCT
		LEFT(CAST(( 
				SELECT 
					Us.UserID + ','
				FROM 
					BSYS.dbo.SYSA_LST_UserIDInfo Us 
				WHERE 
					Us.WindowsUserName = U.WindowsUserName
         FOR XML PATH(''))AS VARCHAR(MAX)),
		 LEN(CAST(( 
				SELECT 
					Us.UserID + ','
				FROM 
					BSYS.dbo.SYSA_LST_UserIDInfo Us 
				WHERE 
					Us.WindowsUserName = U.WindowsUserName
         FOR XML PATH(''))AS VARCHAR(MAX))) -1)
	FROM CSYS.Noble.UserList UL 
		INNER JOIN BSYS.dbo.SYSA_LST_UserIDInfo U
		ON UL.Username = U.WindowsUserName
	WHERE UL.TSR = @AgentCode
END;
GO
GRANT EXECUTE
    ON OBJECT::[Noble].[GetAgentOtherId] TO [db_executor]
    AS [dbo];

