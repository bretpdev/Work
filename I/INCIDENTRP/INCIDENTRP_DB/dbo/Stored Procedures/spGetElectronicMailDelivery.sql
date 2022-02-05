-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_ElectronicMailDelivery table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetElectronicMailDelivery]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		EmailAddressWasDisclosed,
		FtpTransmissionWasSentToIncorrectDestination,
		IncorrectAttachmentContainedPii,
		EmailWasDeliveredToIncorrectAddress,
		Detail
	FROM DAT_ElectronicMailDelivery
	WHERE TicketNumber = @TicketNumber
END