CREATE PROCEDURE [dbo].[GetAllUsers]

AS
	SELECT
		FirstName,
		LastName,
		WindowsUserName,
		B.Name as BUName
	FROM
		SYSA_DAT_Users U
		INNER JOIN GENR_LST_BusinessUnits B
			ON B.ID = U.BusinessUnit
	WHERE
		U.[Status] = 'Active'
RETURN 0
