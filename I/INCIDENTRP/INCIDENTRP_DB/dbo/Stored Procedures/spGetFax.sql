-- =============================================
-- Author:		Daren Beattie
-- Create date: September 13, 2011
-- Description:	Retrieves a record from the DAT_Fax table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetFax]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		FaxNumber,
		Recipient,
		IncorrectDocumentsWereFaxed,
		FaxNumberWasIncorrect
	FROM DAT_Fax
	WHERE TicketNumber = @TicketNumber
END