﻿CREATE PROCEDURE [hrbridge].[GetMaxCompensationUpdated]
AS
	SELECT
		ISNULL(MAX(UpdatedAt), CAST('01-01-1900' AS DATETIME))
	FROM
		hrbridge.Compensation_BambooHR