-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_ThreatLanguage table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetThreatLanguage]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@Educated BIT = 0,
	@Uneducated BIT = 0,
	@FoulOrProfane BIT = 0,
	@Other BIT = 0,
	@OtherDescription VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ThreatLanguage WHERE TicketNumber = @TicketNumber) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_ThreatLanguage (TicketNumber, TicketType, Educated, Uneducated, FoulOrProfane, Other, OtherDescription)
			VALUES (@TicketNumber, 'Threat', @Educated, @Uneducated, @FoulOrProfane, @Other, @OtherDescription)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_ThreatLanguage
			SET Educated = @Educated,
				Uneducated = @Uneducated,
				FoulOrProfane = @FoulOrProfane,
				Other = @Other,
				OtherDescription = @OtherDescription
			WHERE TicketNumber = @TicketNumber
		END
END