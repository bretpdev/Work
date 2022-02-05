-- =============================================
-- Author:		Daren Beattie
-- Create date: September 8, 2011
-- Description:	Retrieves a record from the DAT_Ticket table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetTicket]
	-- Add the parameters for the stored procedure here
	@TicketNumber BIGINT,
	@TicketType VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		IncidentDateTime,
		CreateDateTime,
		Requester,
		FunctionalArea,
		Priority,
		[Status],
		COALESCE(Court, 0) AS Court,
		COALESCE(AssignedTo, 0) AS AssignedTo
	FROM DAT_Ticket
	WHERE TicketNumber = @TicketNumber
	AND TicketType = @TicketType
END