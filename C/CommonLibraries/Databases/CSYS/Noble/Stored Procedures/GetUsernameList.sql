CREATE PROCEDURE [Noble].[GetUsernameList]
AS
BEGIN
	SELECT DISTINCT
		U.Username
	FROM 
		CSYS.Noble.UserList U
END;
GO

