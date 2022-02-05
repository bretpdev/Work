-- =============================================
-- Author:		Daren Beattie
-- Create date: September 22, 2011
-- Description:	Removes any records from the DAT_Lock table matching the given Active Directory SID and ticket type.
-- =============================================
CREATE PROCEDURE [dbo].[spReleaseLock]
	-- Add the parameters for the stored procedure here
	@SqlUserId INT,
	@TicketType VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM DAT_Lock
	WHERE SqlUserId = @SqlUserId
	AND TicketType = @TicketType
END