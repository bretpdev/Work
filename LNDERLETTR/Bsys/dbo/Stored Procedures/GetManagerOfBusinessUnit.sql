

CREATE PROCEDURE [dbo].[GetManagerOfBusinessUnit]
	@BusinessUnit NVARCHAR(50)
AS
	SELECT 
		TOP 1 utid.UserID
	  FROM 
		BSYS.dbo.GENR_REF_BU_Agent_Xref agent
	  JOIN 
		BSYS.dbo.SYSA_LST_UserIDInfo utid 
			ON utid.WindowsUserName = agent.WindowsUserID
	 WHERE 
		BusinessUnit = @BusinessUnit
		AND [Role] = 'Manager'
		AND utid.[Date Access Removed] is null
	    AND utid.UserID not like 'PH%'
RETURN 0