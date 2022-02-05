-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_OddComputerBehavior table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetOddComputerBehavior]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		EmailPhishingOrHoax,
		DenialOfService,
		UnexplainedAttemptToWriteToSystemFiles,
		UnexplainedModificationOrDeletionOfDate,
		UnexplainedModificationToFileLengthOrDate,
		UnexplainedNewFilesOrUnfamiliarFileNames,
		Malware
	FROM DAT_OddComputerBehavior
	WHERE TicketNumber = @TicketNumber
END