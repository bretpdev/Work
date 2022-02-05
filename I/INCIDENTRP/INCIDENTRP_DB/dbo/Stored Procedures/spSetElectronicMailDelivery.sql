-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_ElectronicMailDelivery table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetElectronicMailDelivery]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@EmailAddressWasDisclosed BIT,
	@FtpTransmissionWasSentToIncorrectDestination BIT,
	@IncorrectAttachmentContainedPii BIT,
	@EmailWasDeliveredToIncorrectAddress BIT,
	@Detail VARCHAR(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ElectronicMailDelivery WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_ElectronicMailDelivery (
				TicketNumber,
				TicketType,
				EmailAddressWasDisclosed,
				FtpTransmissionWasSentToIncorrectDestination,
				IncorrectAttachmentContainedPii,
				EmailWasDeliveredToIncorrectAddress,
				Detail
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@EmailAddressWasDisclosed,
				@FtpTransmissionWasSentToIncorrectDestination,
				@IncorrectAttachmentContainedPii,
				@EmailWasDeliveredToIncorrectAddress,
				@Detail
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_ElectronicMailDelivery
			SET EmailAddressWasDisclosed = @EmailAddressWasDisclosed,
				FtpTransmissionWasSentToIncorrectDestination = @FtpTransmissionWasSentToIncorrectDestination,
				IncorrectAttachmentContainedPii = @IncorrectAttachmentContainedPii,
				EmailWasDeliveredToIncorrectAddress = @EmailWasDeliveredToIncorrectAddress,
				Detail = @Detail
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END