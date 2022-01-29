DECLARE @USER VARCHAR(128) = (SELECT SqlUserId FROM SYSA_DAT_Users WHERE WindowsUserName = (SELECT REPLACE(USER_NAME(), 'uheaa\', '')))
DECLARE @USERKEY INT = (SELECT ID FROM SYSA_LST_UserKeys WHERE UserKey = 'Need Help Submit FAR')

INSERT INTO SYSA_DAT_RoleKeyAssignment(RoleID, UserKeyID, AddedBy, StartDate)
SELECT DISTINCT
	R.RoleID [RoleId],
	@USERKEY [UserKeyId],
	@USER [AddedBy],
	GETDATE() [StartDate]
FROM
	SYSA_DAT_Users U
	LEFT JOIN SYSA_LST_Role R
		ON R.RoleID = U.[Role]
	LEFT JOIN GENR_LST_BusinessUnits BU
		ON BU.ID = U.BusinessUnit
WHERE
	U.[Status] = 'Active'
	AND
	U.[Role] IN 
	(
		SELECT
			R.RoleID
		FROM
			SYSA_DAT_RoleKeyAssignment RA
			LEFT JOIN SYSA_LST_Role R
				ON R.RoleID = RA.RoleID
			LEFT JOIN SYSA_LST_UserKeys UK
				ON UK.ID = RA.UserKeyID
		WHERE
			UK.UserKey = 'Need Help Submit'
			AND R.RoleID IS NOT NULL
			AND RA.EndDate IS NULL
	)