CREATE PROCEDURE [dbo].[GetEmailNotificationData]

AS
	SELECT 
		DD.DocumentDetailsId AS EmailId,
		ADDR_ACCT_NUM AS AccountNumber,
		AddresseeEmail AS EmailAddress,
		L.Letter AS LetterId,
		L.DocComment AS EmailSubjectLine
	FROM
		DocumentDetails DD
	INNER JOIN Letters L
		ON L.LetterId = DD.LetterId
	WHERE
		DD.Printed IS NOT NULL
		AND DD.CorrMethod = 'Email Notify'
		and DD.EmailSent IS NULL
		AND (DATEDIFF(hour, DD.Printed, GETDATE())) > 4
		AND dd.LetterId not in (1027,1028,1029)

	UNION ALL

	SELECT 
		DD.DocumentDetailsId AS EmailId,
		ADDR_ACCT_NUM AS AccountNumber,
		AddresseeEmail AS EmailAddress,
		L.Letter AS LetterId,
		L.DocComment AS EmailSubjectLine
	FROM
		DocumentDetails DD
	INNER JOIN Letters L
		ON L.LetterId = DD.LetterId
	WHERE
		DD.Printed IS NOT NULL
		AND DD.CorrMethod = 'Email Notify'
		and DD.EmailSent IS NULL
		AND (DATEDIFF(DAY, DD.Printed, GETDATE())) > 5
		AND dd.LetterId in (1027,1028,1029)

RETURN 0
