-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_DisposalDestruction table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetDisposalDestruction]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		ElectronicMediaRecordsWereDestroyedInError,
		ElectronicMediaRecordsWereDestroyedUsingIncorrectMethod,
		MicrofilmWithRecordsWasDestroyedInError,
		MicrofilmWithRecordsWasDestroyedUsingIncorrectMethod,
		PaperRecordsWereDestroyedInError,
		PaperRecordsWereDestroyedUsingIncorrectMethod
	FROM DAT_DisposalDestruction
	WHERE TicketNumber = @TicketNumber
END