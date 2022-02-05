CREATE PROCEDURE [dbo].[GetSystems]
AS
	SELECT
		*
	FROM
		SystemType

RETURN 0

GRANT EXECUTE ON [dbo].[GetSystems] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSystems] TO [db_executor]
    AS [dbo];

