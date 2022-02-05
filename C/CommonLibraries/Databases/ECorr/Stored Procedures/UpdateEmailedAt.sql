CREATE PROCEDURE [dbo].[UpdateEmailedAt]
	@AccountNumber varchar(10),
	@EmailAddress varchar(254),
	@EmailSubjectLine varchar(255)
AS
	
	UPDATE DD
	SET 
		DD.EmailSent = GETDATE()
	FROM DocumentDetails DD
	INNER JOIN Letters L
		ON L.LetterId = DD.LetterId
	WHERE
		ADDR_ACCT_NUM = @AccountNumber
		AND AddresseeEmail = @EmailAddress
		AND L.DocComment = @EmailSubjectLine
		AND (DATEDIFF(second, DD.Printed, GETDATE()) / 60.0 / 60.0) > 4
		
RETURN 0
