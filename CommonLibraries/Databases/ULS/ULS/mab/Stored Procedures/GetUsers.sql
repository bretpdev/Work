
CREATE PROCEDURE [mab].[GetUsers]
AS
	SELECT
		AesId,
		UserId,
		BeginRange,
		EndRange,
		AddedOn,
		AddedBy
	FROM
		RangeAssignment

GRANT EXECUTE ON [mab].[GetUsers] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[mab].[GetUsers] TO [db_executor]
    AS [dbo];

