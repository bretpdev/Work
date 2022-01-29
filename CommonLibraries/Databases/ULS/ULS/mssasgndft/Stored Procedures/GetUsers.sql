CREATE PROCEDURE [mssasgndft].[GetUsers]
AS
	SELECT
		AesId,
		UserId,
		BeginRange,
		EndRange,
		AddedOn,
		AddedBy
	FROM
		mssasgndft.RangeAssignment

GRANT EXECUTE
    ON OBJECT::[mssasgndft].[GetUsers] TO [db_executor]
    AS [dbo];
GO
GRANT EXECUTE
    ON OBJECT::[mssasgndft].[GetUsers] TO [db_executor]
    AS [dbo];

