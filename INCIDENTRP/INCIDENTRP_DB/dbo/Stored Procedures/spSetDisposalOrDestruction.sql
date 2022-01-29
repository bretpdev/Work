-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_DisposalDestruction table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetDisposalOrDestruction]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@ElectronicMediaRecordsWereDestroyedInError BIT,
	@ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod BIT,
	@MicrofilmWithRecordsWasDestroyedInError BIT,
	@MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod BIT,
	@PaperRecordsWereDestroyedInError BIT,
	@PaperRecordsWereDestroyedUsingIncorrectMethod BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_DisposalDestruction WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_DisposalDestruction (
				TicketNumber,
				TicketType,
				ElectronicMediaRecordsWereDestroyedInError,
				ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod,
				MicrofilmWithRecordsWasDestroyedInError,
				MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod,
				PaperRecordsWereDestroyedInError,
				PaperRecordsWereDestroyedUsingIncorrectMethod
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@ElectronicMediaRecordsWereDestroyedInError,
				@ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod,
				@MicrofilmWithRecordsWasDestroyedInError,
				@MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod,
				@PaperRecordsWereDestroyedInError,
				@PaperRecordsWereDestroyedUsingIncorrectMethod
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_DisposalDestruction
			SET ElectronicMediaRecordsWereDestroyedInError = @ElectronicMediaRecordsWereDestroyedInError,
				ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod = @ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod,
				MicrofilmWithRecordsWasDestroyedInError = @MicrofilmWithRecordsWasDestroyedInError,
				MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod = @MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod,
				PaperRecordsWereDestroyedInError = @PaperRecordsWereDestroyedInError,
				PaperRecordsWereDestroyedUsingIncorrectMethod = @PaperRecordsWereDestroyedUsingIncorrectMethod
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END