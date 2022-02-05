CREATE PROCEDURE [complaints].[ComplaintUpdate]
	@ComplaintId int,
	@BorrowerName nvarchar(50),
	@ComplaintPartyId int,
	@ComplaintTypeId int,
	@ComplaintGroupId int,
	@ComplaintDescription nvarchar(max),
	@ComplaintDate datetime,
	@ControlMailNumber nvarchar(100),
	@NeedHelpTicketNumber nvarchar(8),
	@DaysToRespond int
AS
	
	update [complaints].Complaints 
	   set BorrowerName = @BorrowerName, ComplaintPartyId = @ComplaintPartyId, ComplaintTypeId = @ComplaintTypeId, ComplaintDescription = @ComplaintDescription, 
	       ComplaintDate = @ComplaintDate, ControlMailNumber = @ControlMailNumber, NeedHelpTicketNumber = @NeedHelpTicketNumber, DaysToRespond = @DaysToRespond,
		   ComplaintGroupId = @ComplaintGroupId
	 where ComplaintId = @ComplaintId

RETURN 0