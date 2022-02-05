
CREATE PROCEDURE [mssasgndft].[GetRanges]
AS
	SELECT
		UserId,
		BeginRange,
		EndRange,
		AddedOn,
		AddedBy
	FROM
		mssasgndft.RangeAssignment