-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_DataEntry table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetDataEntry]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@IncorrectInformationWasAdded BIT,
	@InformationWasIncorrectlyChanged BIT,
	@InformationWasIncorrectlyDeleted BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_DataEntry WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_DataEntry (
				TicketNumber,
				TicketType,
				IncorrectInformationWasAdded,
				InformationWasIncorrectlyChanged,
				InformationWasIncorrectlyDeleted
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@IncorrectInformationWasAdded,
				@InformationWasIncorrectlyChanged,
				@InformationWasIncorrectlyDeleted
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_DataEntry
			SET IncorrectInformationWasAdded = @IncorrectInformationWasAdded,
				InformationWasIncorrectlyChanged = @InformationWasIncorrectlyChanged,
				InformationWasIncorrectlyDeleted = @InformationWasIncorrectlyDeleted
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END