
/********************************************************
*Routine Name	: [dbo].[spAddCommonExceptionTicket]
*Purpose		: 
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		07/31/2012  Bret Pehrson
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spAddCommonExceptionTicket]
	-- Add the parameters for the stored procedure here
	  @TicketCode Varchar(50) = ''
	, @Subject Varchar(50) = ''
	, @BusinessUnit int = 0
	, @Issue text = ''
	, @Status Varchar(50) = ''
	, @UrgencyOption Varchar(200) = ''
	, @CatOption Varchar(200) = ''
	, @Requester int = 0
	, @Court int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @TicketNumber AS BIGINT
	DECLARE @Requested AS DATETIME = GetDate()

    -- Insert statements for procedure here
	INSERT INTO DAT_Ticket(TicketCode, [Subject], Requested, Unit, Issue, [Status], UrgencyOption, CatOption)
	VALUES(@TicketCode, @Subject, @Requested, @BusinessUnit, @Issue, @Status, @UrgencyOption, @CatOption)
	
	SET @TicketNumber = @@IDENTITY
	
	INSERT INTO dbo.DAT_TicketsAssociatedUserID VALUES (@TicketNumber, 'Requester', @Requester)
	INSERT INTO dbo.DAT_TicketsAssociatedUserID VALUES (@TicketNumber, 'Court', @Court)
	INSERT INTO dbo.DAT_TicketsAssociatedUserID VALUES (@TicketNumber, 'AssignedTo', null)
	INSERT INTO dbo.DAT_TicketsAssociatedUserID VALUES (@TicketNumber, 'PreviousCourt', null)

	SET NOCOUNT OFF;
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spAddCommonExceptionTicket] TO [db_executor]
    AS [dbo];

