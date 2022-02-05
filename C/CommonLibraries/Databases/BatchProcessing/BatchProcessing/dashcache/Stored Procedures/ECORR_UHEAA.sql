CREATE PROCEDURE [dashcache].[ECORR_UHEAA]
AS

	DECLARE @Date DATETIME = [CentralData].dbo.AddBusinessDays(GETDATE(), -2)

	SELECT
		COUNT(*)
	FROM
		ECorrUheaa..[DocumentDetails] DD
	WHERE
		DD.Active = 1
		AND
		(
			( -- document has not been printed (really means processed) within 2 business days of DocDate
				DD.DocDate < @Date
				AND
				DD.Printed IS NULL
			)
			OR
			( -- document was created (really means processed) but has not been sent to AES
				DD.Printed BETWEEN '2016-1-1' /*exclude records before tracking enhancement*/ AND DATEADD(DAY, -3, GETDATE())
				AND
				DD.ZipFileName IS NULL
			)
		)
		AND
		DD.AddedAt < [CentralData].dbo.AddBusinessDays(GETDATE(), -2)

RETURN 0