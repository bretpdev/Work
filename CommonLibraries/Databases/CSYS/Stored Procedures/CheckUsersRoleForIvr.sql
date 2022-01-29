CREATE PROCEDURE [dbo].[CheckUsersRoleForIvr]
	@WindowsUserId varchar(50)
AS
	
	SELECT
		COUNT(*)
	FROM
		SYSA_DAT_Users U
		INNER JOIN SYSA_Ivr I
			ON I.RoleID = U.[Role]
	WHERE
		WindowsUserName = @WindowsUserId
RETURN 0
