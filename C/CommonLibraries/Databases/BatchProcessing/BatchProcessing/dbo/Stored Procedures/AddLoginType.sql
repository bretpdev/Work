CREATE PROCEDURE [dbo].[AddLoginType]
	@LoginType varchar(150),
	@MaxInUse int
AS
	INSERT INTO LoginType(LoginType, MaxInUse)
	VALUES(@LoginType, @MaxInUse)
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddLoginType] TO [db_executor]
    AS [dbo];

