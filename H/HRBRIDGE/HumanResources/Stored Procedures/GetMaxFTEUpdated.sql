CREATE PROCEDURE [hrbridge].[GetMaxFTEUpdated]
AS
	SELECT
		ISNULL(MAX(UpdatedAt), CAST('01-01-1900' AS DATETIME))
	FROM
		hrbridge.FTE_BambooHR
RETURN 0
