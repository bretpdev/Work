CREATE PROCEDURE [dbo].[UpdateLoginType]
	@LoginTypeId int,
	@LoginType varchar(150),
	@MaxInUse int
AS
	UPDATE 
		LoginType
	SET 
		LoginType = @LoginType,
		MaxInUse = @MaxInUse
	WHERE
		LoginTypeId = @LoginTypeId

RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateLoginType] TO [db_executor]
    AS [dbo];

