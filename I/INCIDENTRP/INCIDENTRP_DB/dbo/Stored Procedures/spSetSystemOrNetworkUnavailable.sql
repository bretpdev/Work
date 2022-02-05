-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_SystemOrNetworkUnavailable table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetSystemOrNetworkUnavailable]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@DenialOrDisruptionOfService BIT,
	@UnableToLogIntoAccount BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_SystemOrNetworkUnavailable WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_SystemOrNetworkUnavailable (
				TicketNumber,
				TicketType,
				DenialOrDisruptionOfService,
				UnableToLogIntoAccount
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@DenialOrDisruptionOfService,
				@UnableToLogIntoAccount
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_SystemOrNetworkUnavailable
			SET DenialOrDisruptionOfService = @DenialOrDisruptionOfService,
				UnableToLogIntoAccount = @UnableToLogIntoAccount
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END