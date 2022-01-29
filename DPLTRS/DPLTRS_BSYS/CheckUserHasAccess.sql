CREATE PROCEDURE [dbo].[CheckUserHasAccess]
	@UID VARCHAR(7),
	@TypeKey VARCHAR(50)
AS
	SELECT
		CAST(MAX(CASE WHEN Info.UserID IS NOT NULL THEN 1 ELSE 0 END) AS BIT) AS HasAccess
	FROM
		GENR_REF_AuthAccess Auth
		LEFT JOIN SYSA_LST_UserIDInfo Info
			ON Auth.WinUName = Info.WindowsUserName
			AND Info.UserID = @UID
	WHERE
		Auth.TypeKey = @TypeKey
RETURN 0
