-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_ActionTaken table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetActionTaken]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@TicketType VARCHAR(50),
	@Action VARCHAR(100),
	@ActionDateTime DATETIME,
	@PersonContacted VARCHAR(100) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ActionTaken WHERE TicketNumber = @TicketNumber AND TicketType = @TicketType AND [Action] = @Action) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_ActionTaken (TicketNumber, TicketType, [Action], ActionDateTime, PersonContacted)
			VALUES (@TicketNumber, @TicketType, @Action, @ActionDateTime, @PersonContacted)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_ActionTaken
			SET ActionDateTime = @ActionDateTime,
				PersonContacted = @PersonContacted
			WHERE TicketNumber = @TicketNumber
				AND TicketType = @TicketType
				AND [Action] = @Action
		END
END