-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_Fax table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetFax]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@FaxNumber VARCHAR(20),
	@Recipient VARCHAR(50),
	@IncorrectDocumentsWereFaxed BIT,
	@FaxNumberWasIncorrect BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_Fax WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_Fax (
				TicketNumber,
				TicketType,
				FaxNumber,
				Recipient,
				IncorrectDocumentsWereFaxed,
				FaxNumberWasIncorrect
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@FaxNumber,
				@Recipient,
				@IncorrectDocumentsWereFaxed,
				@FaxNumberWasIncorrect
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_Fax
			SET FaxNumber = @FaxNumber,
				Recipient = @Recipient,
				IncorrectDocumentsWereFaxed = @IncorrectDocumentsWereFaxed,
				FaxNumberWasIncorrect = @FaxNumberWasIncorrect
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END