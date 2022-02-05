CREATE PROCEDURE [dbo].[spGetUserSelectedEmailRecipientsForTicket]
	@TicketID			BIGINT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		a.SqlUserId AS ID,
		a.WindowsUserName,
		a.FirstName,
		a.MiddleInitial,
		a.LastName,
		a.EMail AS EmailAddress,
		a.Extension AS PrimaryExtension,
		a.Extension2 AS SecondaryExtension,
		a.AesAccountId AS AesAccountNumber,
		a.[Role],
		a.[Status],
		a.BusinessUnit,
		a.Title
	FROM  CSYS.dbo.SYSA_DAT_Users a --TODO: Change to CSYS
	join dbo.REF_EMailRecipient b
	on a.SqlUserId = b.SqlUserId
	WHERE b.Ticket = @TicketID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetUserSelectedEmailRecipientsForTicket] TO [db_executor]
    AS [dbo];

