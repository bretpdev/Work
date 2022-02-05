CREATE PROCEDURE [dbo].[GetEmailNotificationData]

AS
	SELECT 
		DD.DocumentDetailsId AS EmailId,
		ADDR_ACCT_NUM AS AccountNumber,
		AddresseeEmail AS EmailAddress,
		L.DocComment AS EmailSubjectLine
	FROM
		DocumentDetails DD
	INNER JOIN Letters L
		ON L.LetterId = DD.LetterId
	WHERE
		DD.Printed IS NOT NULL
		AND DD.CorrMethod = 'EmailNotify'
		and DD.EmailSent IS NULL
		AND (DATEDIFF(hour, DD.Printed, GETDATE())) > 4
		AND DD.Active = 1

RETURN 0