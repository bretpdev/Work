-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Removes a record from the DAT_OddComputerBehavior table.
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteOddComputerBehavior]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    DELETE FROM DAT_OddComputerBehavior
    WHERE TicketNumber = @TicketNumber
END