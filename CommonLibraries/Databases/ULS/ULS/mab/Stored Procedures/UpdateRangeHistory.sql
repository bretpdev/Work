
CREATE PROCEDURE [mab].[UpdateRangeHistory]
AS
	INSERT INTO RangeHistory(RangeAssignmentId, AesId, UserId, BeginRange, EndRange, AddedOn, AddedBy)
	SELECT
		RangeAssignmentId, AesId, UserId, BeginRange, EndRange, AddedOn, AddedBy
	FROM
		RangeAssignment

	DELETE FROM RangeAssignment
RETURN 0

GRANT EXECUTE ON [mab].[UpdateRangeHistory] TO db_executor