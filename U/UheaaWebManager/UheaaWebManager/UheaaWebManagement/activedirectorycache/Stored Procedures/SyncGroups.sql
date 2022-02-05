CREATE PROCEDURE [activedirectorycache].[SyncGroups]
AS
	
	MERGE activedirectorycache.Groups AS t
	USING
	(
		SELECT
			r.ActiveDirectoryRoleName, r.RoleId
		FROM
			webapi.Roles r
		WHERE
			r.InactivatedAt IS NULL
	) AS s
	ON t.GroupName = s.ActiveDirectoryRoleName
	WHEN MATCHED THEN
		UPDATE SET RoleId = s.RoleId
	WHEN NOT MATCHED BY SOURCE THEN
		UPDATE SET DeletedAt = GETDATE()
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (GroupName, RoleId)
		VALUES (ActiveDirectoryRoleName, RoleId)
	;

	UPDATE
		UG
	SET
		UG.DeletedAt = G.DeletedAt
	FROM
		UserGroups UG
		INNER JOIN Groups G ON G.GroupId = UG.GroupId
	WHERE
		G.DeletedAt IS NOT NULL

RETURN 0
