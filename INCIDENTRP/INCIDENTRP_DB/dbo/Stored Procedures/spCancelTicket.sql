-- =============================================
-- Author:		Daren Beattie
-- Create date: September 14, 2011
-- Description:	Creates a record in the DAT_CanceledTickets table.
-- =============================================
CREATE PROCEDURE [dbo].[spCancelTicket]
	-- Add the parameters for the stored procedure here
	@SqlUserId INT,
	@TicketType VARCHAR(50),
	@CreateDateTime DATETIME,
	@CancelDateTime DATETIME,
	@AccountNumber VARCHAR(10) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO DAT_CanceledTickets (
		SqlUserId,
		TicketType,
		CreateDateTime,
		CancelDateTime,
		AccountNumber
	)
	VALUES (
		@SqlUserId,
		@TicketType,
		@CreateDateTime,
		@CancelDateTime,
		@AccountNumber
	)
END