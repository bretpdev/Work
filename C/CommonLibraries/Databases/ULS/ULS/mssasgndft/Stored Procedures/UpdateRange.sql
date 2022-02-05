CREATE PROCEDURE [mssasgndft].[UpdateRange]
	@AesId varchar(7),
	@UserId int,
	@BeginRange int,
	@EndRange int
AS
	INSERT INTO mssasgndft.RangeAssignment(AesId, UserId, BeginRange, EndRange)
	VALUES(@AesId, @UserId, @BeginRange, @EndRange)

GRANT EXECUTE
    ON OBJECT::[mssasgndft].[UpdateRange] TO [db_executor]
    AS [dbo];
GO
GRANT EXECUTE
    ON OBJECT::[mssasgndft].[UpdateRange] TO [db_executor]
    AS [dbo];

