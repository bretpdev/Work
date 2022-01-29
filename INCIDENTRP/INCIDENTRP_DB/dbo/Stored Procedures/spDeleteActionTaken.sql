-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Removes a record from the DAT_ActionTaken table.
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteActionTaken]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@TicketType VARCHAR(50),
	@Action VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM DAT_ActionTaken
	WHERE TicketNumber = @TicketNumber
		AND TicketType = @TicketType
		AND [Action] = @Action
END