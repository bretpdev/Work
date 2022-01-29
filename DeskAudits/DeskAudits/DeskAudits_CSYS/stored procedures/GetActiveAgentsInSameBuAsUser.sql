CREATE PROCEDURE [deskaudits].[GetActiveAgentsInSameBuAsUser]
	@UserName VARCHAR(50)

AS
	
	/*
		Gets the users who are in an active status
		in the same BU as the passed-in user.
		Excludes the passed-in user, as they should
		not be allowed to submit an audit for themselves.
	*/
	DECLARE @BuName VARCHAR(50) = (SELECT B.[Name] FROM GENR_LST_BusinessUnits B INNER JOIN SYSA_DAT_Users U ON U.BusinessUnit = B.ID WHERE U.[Status] = 'Active' AND U.WindowsUserName = @UserName)

	SELECT
		RTRIM(FirstName) AS FirstName,
		RTRIM(LastName) AS LastName
	FROM
		SYSA_DAT_Users U
		INNER JOIN GENR_LST_BusinessUnits B
			ON B.ID = U.BusinessUnit
		WHERE
			U.[Status] = 'Active'
			AND B.[Name] = @BuName 
			AND U.WindowsUserName != @UserName

RETURN 0
