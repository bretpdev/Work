-- =============================================
-- Author:		Daren Beattie
-- Create date: September 7, 2011
-- Description:	Creates or updates a record in the DAT_ThreatDialect table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetThreatDialect]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@English BIT = 0,
	@RegionalAmerican BIT = 0,
	@RegionalAmericanDescription VARCHAR(50) = NULL,
	@ForeignAccent BIT = 0,
	@ForeignAccentDescription VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_ThreatDialect WHERE TicketNumber = @TicketNumber) = 0
		-- Insert a new record.
		BEGIN
			INSERT INTO DAT_ThreatDialect (TicketNumber, TicketType, English, RegionalAmerican, RegionalAmericanDescription, ForeignAccent, ForeignAccentDescription)
			VALUES (@TicketNumber, 'Threat', @English, @RegionalAmerican, @RegionalAmericanDescription, @ForeignAccent, @ForeignAccentDescription)
		END
	ELSE
		-- Update the existing record.
		BEGIN
			UPDATE DAT_ThreatDialect
			SET English = @English,
				RegionalAmerican = @RegionalAmerican,
				RegionalAmericanDescription = @RegionalAmericanDescription,
				ForeignAccent = @ForeignAccent,
				ForeignAccentDescription = @ForeignAccentDescription
			WHERE TicketNumber = @TicketNumber
		END
END