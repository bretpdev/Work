-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_RegularMailDelivery table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetRegularMailDelivery]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		Problem,
		Address1,
		Address2,
		City,
		[State],
		Zip,
		TrackingNumber
	FROM DAT_RegularMailDelivery
	WHERE TicketNumber = @TicketNumber
END