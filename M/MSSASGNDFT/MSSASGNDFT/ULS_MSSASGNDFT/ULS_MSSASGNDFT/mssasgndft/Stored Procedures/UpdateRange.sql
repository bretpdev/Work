CREATE PROCEDURE [mssasgndft].[UpdateRange]
	@AesId VARCHAR(7),
	@UserId INT,
	@BeginRange INT,
	@EndRange INT
AS
	INSERT INTO mssasgndft.RangeAssignment(AesId, UserId, BeginRange, EndRange)
	VALUES(@AesId, @UserId, @BeginRange, @EndRange)