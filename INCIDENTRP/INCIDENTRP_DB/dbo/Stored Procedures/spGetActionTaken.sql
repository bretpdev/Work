-- =============================================
-- Author:		Daren Beattie
-- Create date: September 8, 2011
-- Description:	Retrieves a record from the DAT_ActionTaken table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetActionTaken]
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
	SELECT
		ActionDateTime,
		PersonContacted
	FROM DAT_ActionTaken
	WHERE TicketNumber = @TicketNumber
	AND TicketType = @TicketType
	AND [Action] = @Action
END