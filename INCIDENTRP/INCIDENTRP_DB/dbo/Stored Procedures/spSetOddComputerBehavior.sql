-- =============================================
-- Author:		Daren Beattie
-- Create date: September 12, 2011
-- Description:	Creates a record in the DAT_OddComputerBehavior table.
-- =============================================
CREATE PROCEDURE [dbo].[spSetOddComputerBehavior]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@EmailPhishingOrHoax BIT,
	@DenialOfService BIT,
	@UnexplainedAttemptToWriteToSystemFiles BIT,
	@UnexplainedModificationOrDeletionOfDate BIT,
	@UnexplainedModificationToFileLengthOrDate BIT,
	@UnexplainedNewFilesOrUnfamiliarFileNames BIT,
	@Malware BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM DAT_OddComputerBehavior WHERE TicketNumber = @TicketNumber AND TicketType = 'Incident') = 0
		BEGIN
			INSERT INTO DAT_OddComputerBehavior (
				TicketNumber,
				TicketType,
				EmailPhishingOrHoax,
				DenialOfService,
				UnexplainedAttemptToWriteToSystemFiles,
				UnexplainedModificationOrDeletionOfDate,
				UnexplainedModificationToFileLengthOrDate,
				UnexplainedNewFilesOrUnfamiliarFileNames,
				Malware
			)
			VALUES (
				@TicketNumber,
				'Incident',
				@EmailPhishingOrHoax,
				@DenialOfService,
				@UnexplainedAttemptToWriteToSystemFiles,
				@UnexplainedModificationOrDeletionOfDate,
				@UnexplainedModificationToFileLengthOrDate,
				@UnexplainedNewFilesOrUnfamiliarFileNames,
				@Malware
			)
		END
	ELSE
		BEGIN
			UPDATE DAT_OddComputerBehavior
			SET EmailPhishingOrHoax = @EmailPhishingOrHoax,
				DenialOfService = @DenialOfService,
				UnexplainedAttemptToWriteToSystemFiles = @UnexplainedAttemptToWriteToSystemFiles,
				UnexplainedModificationOrDeletionOfDate = @UnexplainedModificationOrDeletionOfDate,
				UnexplainedModificationToFileLengthOrDate = @UnexplainedModificationToFileLengthOrDate,
				UnexplainedNewFilesOrUnfamiliarFileNames = @UnexplainedNewFilesOrUnfamiliarFileNames,
				Malware = @Malware
			WHERE TicketNumber = @TicketNumber
				AND TicketType = 'Incident'
		END
END