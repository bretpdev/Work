

/********************************************************
*Routine Name	: [dbo].[spBankoAddAdditionalEvents]
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		08/06/2012	Jarom Ryan
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spBankoAddAdditionalEvents]

	@CaseNumber as Varchar(12),
	@DateOfEvent as Varchar(8),
	@EventCode as Varchar(2),
	@EventDescription as Varchar(200) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

   Insert into dbo.BankoReceiveAdditionalEvents(CaseNumber,DateOfEvent,EventCode,EventDescription)
   values(@CaseNumber, @DateOfEvent, @EventCode, @EventDescription)
   
   select RecordNumber
   From dbo.BankoReceiveAdditionalEvents
   Where CaseNumber = @CaseNumber and DateOfEvent = @DateOfEvent

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBankoAddAdditionalEvents] TO [db_executor]
    AS [dbo];



