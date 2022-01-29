-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_Telephone table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetTelephone]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@RevealedInformationOnVoicemail BIT,
	@RevealedInformationToUnauthorizedIndividual BIT,
	@UnauthorizedIndividual VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_Telephone WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_Telephone (
				TicketNumber,
				TicketType,
				RevealedInformationOnVoicemail,
				RevealedInformationToUnauthorizedIndividual,
				UnauthorizedIndividual
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@RevealedInformationOnVoicemail,
				@RevealedInformationToUnauthorizedIndividual,
				@UnauthorizedIndividual
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_Telephone
			SET RevealedInformationOnVoicemail = @RevealedInformationOnVoicemail,
				RevealedInformationToUnauthorizedIndividual = @RevealedInformationToUnauthorizedIndividual,
				UnauthorizedIndividual = @UnauthorizedIndividual
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END