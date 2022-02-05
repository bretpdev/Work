CREATE PROCEDURE [dbo].[GetDueDateChangeRecordCount]

AS
	SELECT
		COUNT(*)
	FROM 
		DueDateChange
	WHERE ProcessedAt IS NULL
RETURN 0
