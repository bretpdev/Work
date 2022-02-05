CREATE PROCEDURE [deskaudits].[GetUserAccess]
	@UserName VARCHAR(50)

AS

	SELECT
		RAA.SearchAccess,
		RAA.SubmitAccess
	FROM
		deskaudits.RolesAndAccess RAA
		INNER JOIN dbo.SYSA_DAT_Users U
			ON U.WindowsUserName = @UserName
			AND U.[Status] = 'Active'
			AND U.[Role] = RAA.RoleId
			AND RAA.DeletedAt IS NULL
		INNER JOIN dbo.SYSA_LST_Role R
			ON R.RoleID = RAA.RoleId
			AND R.RemovedBy IS NULL

RETURN 0
