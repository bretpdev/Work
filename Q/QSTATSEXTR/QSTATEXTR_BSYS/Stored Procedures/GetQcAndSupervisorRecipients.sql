CREATE PROCEDURE [qstatsextr].[GetQcAndSupervisorRecipients]
	@BusinessUnit VARCHAR(50)
AS
	
	SELECT 
		WindowsUserID + '@utahsbr.edu' as Email 
	FROM 
		GENR_REF_BU_Agent_Xref 
	WHERE 
		BusinessUnit = @BusinessUnit 
		AND 
		[Role] IN ('Manager')


RETURN 0
