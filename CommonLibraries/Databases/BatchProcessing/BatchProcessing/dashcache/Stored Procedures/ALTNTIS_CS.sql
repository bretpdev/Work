CREATE PROCEDURE [dashcache].[ALTNTIS_CS]
AS

	SELECT
		COUNT(*)
	FROM
		ECorrFed..[DocumentDetails] DD
	WHERE
		DD.DocDate < [CentralData].dbo.AddBusinessDays(GETDATE(), -3)  -- document has not been sent to NTIS within 3 business days of DocDate
		AND
		DD.CorrespondenceFormatSentDate IS NULL
		AND
		DD.CorrespondenceFormatId != 1 -- not standard format
		AND
		DD.Active = 1


RETURN 0