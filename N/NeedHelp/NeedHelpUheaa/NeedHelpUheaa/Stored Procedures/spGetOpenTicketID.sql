
/********************************************************
*Routine Name	: [dbo].[spGetOpenTicketID]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/31/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spGetOpenTicketID]
	-- Add the parameters for the stored procedure here
		@TicketCode Varchar(50) = ''
	  , @Subject Varchar(50) = ''
	  , @FinalStatus Varchar(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Ticket FROM DAT_Ticket
	WHERE TicketCode = @TicketCode
	AND [Subject] = @Subject
	AND [Status] <> @FinalStatus

	SET NOCOUNT OFF;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetOpenTicketID] TO [db_executor]
    AS [dbo];

