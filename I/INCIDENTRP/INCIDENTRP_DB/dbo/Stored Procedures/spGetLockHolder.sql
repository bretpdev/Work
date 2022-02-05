-- =============================================
-- Author:		Daren Beattie
-- Create date: September 22, 2011
-- Description:	Retrieves the Active Directory security ID of the user holding the lock for a given ticket.
-- =============================================
CREATE PROCEDURE [dbo].[spGetLockHolder]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@TicketType VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SqlUserId
	FROM DAT_Lock
	WHERE TicketNumber = @TicketNumber
	AND TicketType = @TicketType
END