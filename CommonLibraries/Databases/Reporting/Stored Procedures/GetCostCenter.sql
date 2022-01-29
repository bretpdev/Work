CREATE PROCEDURE [dbo].[GetCostCenter]
AS

	SELECT
		*
	FROM
		CostCenter

RETURN 0

GRANT EXECUTE ON [dbo].[GetCostCenter] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCostCenter] TO [db_executor]
    AS [dbo];

