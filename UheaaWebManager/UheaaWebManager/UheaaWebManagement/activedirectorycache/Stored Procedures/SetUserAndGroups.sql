CREATE PROCEDURE [activedirectorycache].[SetUserAndGroups]
	@AssociatedWindowsUsername VARCHAR(100),
	@GroupIds GroupIds readonly
AS

	DECLARE @UserId INT
	SELECT
		@UserId = UserId
	FROM
		activedirectorycache.Users
	WHERE
		AssociatedWindowsUsername = @AssociatedWindowsUsername

	IF (@UserId IS NULL)
	BEGIN
		INSERT INTO activedirectorycache.Users (AssociatedWindowsUsername) VALUES (@AssociatedWindowsUsername)
		SET @UserId = SCOPE_IDENTITY()
	END

	UPDATE
		activedirectorycache.Users
	SET
		UpdatedAt = GETDATE()
	WHERE
		UserId = @UserId

	--add new groups to user
	INSERT INTO activedirectorycache.UserGroups (UserId, GroupId)
	SELECT
		@UserId, GroupId
	FROM
		@GroupIds
	WHERE
		GroupId NOT IN (SELECT GroupId FROM activedirectorycache.UserGroups WHERE UserId = @UserId)

	--update existing groups for user
	UPDATE
		UG
	SET
		UpdatedAt = GETDATE()
	FROM
		activedirectorycache.UserGroups UG
		INNER JOIN @GroupIds G ON G.GroupId = UG.GroupId
	WHERE
		UG.DeletedAt IS NULL

	--mark missing groups with a DeletedAt date
	UPDATE
		UG
	SET
		DeletedAt = GETDATE()
	FROM
		activedirectorycache.UserGroups UG
	WHERE
		UserId = @UserId
		AND
		GroupId NOT IN (SELECT GroupId FROM @GroupIds)

RETURN 0
