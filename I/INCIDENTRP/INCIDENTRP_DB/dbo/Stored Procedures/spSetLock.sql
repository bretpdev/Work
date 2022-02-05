-- =============================================
-- Author:		Daren Beattie
-- Create date: September 22, 2011
-- Description:	Attempts to insert a record into the DAT_Lock table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetLock]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@TicketType VARCHAR(50),
	@SqlUserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO DAT_Lock (
		TicketNumber,
		TicketType,
		SqlUserId
	)
	VALUES (
		@TicketNumber,
		@TicketType,
		@SqlUserId
	)
END