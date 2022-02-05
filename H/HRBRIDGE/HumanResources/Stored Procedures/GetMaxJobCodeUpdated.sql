CREATE PROCEDURE [hrbridge].[GetMaxJobCodeUpdated]
AS
	SELECT
		ISNULL(MAX(UpdatedAt), CAST('01-01-1900' AS DATETIME))
	FROM
		hrbridge.JobCodes_BambooHR
RETURN 0
