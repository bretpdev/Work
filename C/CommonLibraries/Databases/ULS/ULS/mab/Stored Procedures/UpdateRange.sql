
CREATE PROCEDURE [mab].[UpdateRange]
	@AesId varchar(7),
	@UserId int,
	@BeginRange int,
	@EndRange int
AS
	INSERT INTO RangeAssignment(AesId, UserId, BeginRange, EndRange)
	VALUES(@AesId, @UserId, @BeginRange, @EndRange)

GRANT EXECUTE ON [mab].[UpdateRange] TO db_executor