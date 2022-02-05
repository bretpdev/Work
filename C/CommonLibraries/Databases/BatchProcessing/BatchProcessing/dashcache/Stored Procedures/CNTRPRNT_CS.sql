CREATE PROCEDURE [dashcache].[CNTRPRNT_CS]
AS

SELECT
	COUNT(*)
FROM
	CLS..CentralizedPrintingLetter CPL
WHERE
	CPL.DeletedAt IS NULL
	AND
	CPL.Requested <= [CentralData].dbo.AddBusinessDays(GETDATE(), -2)
	AND
	CPL.PrintedAt IS NULL

RETURN 0