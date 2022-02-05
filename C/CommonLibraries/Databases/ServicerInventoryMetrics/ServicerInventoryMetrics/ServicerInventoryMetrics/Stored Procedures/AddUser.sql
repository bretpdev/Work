CREATE PROCEDURE [dbo].[AddUser]
	@CurrentUserId int,
	@NewUserName varchar(50)
AS
	DECLARE @NewUserid INT
	
	INSERT INTO AllowedUsers(AllowedUser)
	VALUES(@NewUserName)

	SET @NewUserid = (Select SCOPE_IDENTITY())

	INSERT INTO AllowedUserAccessGroupMapping(AccessGroupId, AllowedUserId)
	SELECT
		AccessGroupId,
		@NewUserid
	FROM
		AllowedUserAccessGroupMapping
	WHERE
		AllowedUserId = @CurrentUserId
RETURN 0
