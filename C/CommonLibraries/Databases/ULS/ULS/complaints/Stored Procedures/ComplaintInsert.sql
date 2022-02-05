CREATE PROCEDURE [complaints].[ComplaintInsert]
	@AccountNumber char(10),
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

	insert into [complaints].Complaints (AccountNumber, BorrowerName, ComplaintPartyId, ComplaintTypeId, ComplaintDescription, ComplaintDate, ControlMailNumber, NeedHelpTicketNumber, DaysToRespond, ComplaintGroupId)
	values (@AccountNumber, @BorrowerName, @ComplaintPartyId, @ComplaintTypeId, @ComplaintDescription, @ComplaintDate, @ControlMailNumber, @NeedHelpTicketNumber, @DaysToRespond, @ComplaintGroupId)

	select cast(SCOPE_IDENTITY() as int) as ComplaintId

RETURN 0