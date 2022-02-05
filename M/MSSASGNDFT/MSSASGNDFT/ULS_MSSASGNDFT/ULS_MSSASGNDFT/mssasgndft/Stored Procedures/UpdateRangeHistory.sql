CREATE PROCEDURE [mssasgndft].[UpdateRangeHistory]
AS
	INSERT INTO mssasgndft.RangeHistory(RangeAssignmentId, AesId, UserId, BeginRange, EndRange, AddedOn, AddedBy)
	SELECT
		RangeAssignmentId, AesId, UserId, BeginRange, EndRange, AddedOn, AddedBy
	FROM
		mssasgndft.RangeAssignment

	DELETE FROM mssasgndft.RangeAssignment
RETURN 0