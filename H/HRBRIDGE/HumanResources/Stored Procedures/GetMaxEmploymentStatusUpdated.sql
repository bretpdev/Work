CREATE PROCEDURE [hrbridge].[GetMaxEmploymentStatusUpdated]
AS
	SELECT
		ISNULL(MAX(UpdatedAt), CAST('01-01-1900' AS DATETIME))
	FROM
		hrbridge.EmploymentStatus
