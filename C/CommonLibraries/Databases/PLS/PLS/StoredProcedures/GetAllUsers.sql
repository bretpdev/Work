CREATE PROCEDURE [crpqassign].[GetAllUsers]
	
AS
	SELECT
		U.UserName,
		U.AgentName
	FROM 
		crpqassign.Users U	
RETURN 0
