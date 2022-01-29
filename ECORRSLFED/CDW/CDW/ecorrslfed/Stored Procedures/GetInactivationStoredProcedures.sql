CREATE PROCEDURE [ecorrslfed].[GetInactivationStoredProcedures]
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT DISTINCT
	StoredProcedureName
FROM
	ecorrslfed.InactivationStoredProcedures
WHERE
	DeletedAt IS NULL