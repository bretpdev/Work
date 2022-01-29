CREATE PROCEDURE [batchesp].[GetNonSelectionReasons]
AS

	SELECT
		NonSelectionReasonId,
		Reason,
		Course
	FROM
		[batchesp].NonSelectionReasons
	WHERE
		DeletedAt IS NULL

RETURN 0
