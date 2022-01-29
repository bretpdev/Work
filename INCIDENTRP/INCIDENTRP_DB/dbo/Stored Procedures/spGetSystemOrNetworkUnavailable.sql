-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_SystemOrNetworkUnavailable table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetSystemOrNetworkUnavailable]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		DenialOrDisruptionOfService,
		UnableToLogIntoAccount
	FROM DAT_SystemOrNetworkUnavailable
	WHERE TicketNumber = @TicketNumber
END