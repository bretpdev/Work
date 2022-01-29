USE CSYS
GO

SELECT
	U.WindowsUserName,
	R.RoleName,
	UK.UserKey
FROM
	CSYS..SYSA_DAT_RoleKeyAssignment RK
	LEFT JOIN CSYS..SYSA_LST_UserKeys UK
		ON RK.UserKeyID = UK.ID
	LEFT JOIN CSYS..SYSA_DAT_Users U
		ON RK.RoleID = U.[Role]
	LEFT JOIN CSYS..SYSA_LST_Role R
		ON RK.RoleID = R.RoleID
WHERE
	UK.UserKey = 'Need Help Submit'
	AND U.[Status] = 'Active'
ORDER BY
	R.RoleName,
	U.WindowsUserName,
	UK.UserKey