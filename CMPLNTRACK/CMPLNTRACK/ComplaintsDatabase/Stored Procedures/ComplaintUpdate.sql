CREATE PROCEDURE [complaints].[ComplaintUpdate]
	@ComplaintId INT,
	@BorrowerName NVARCHAR(50),
	@ComplaintPartyId INT,
	@ComplaintTypeId INT,
	@ComplaintGroupId INT,
	@ComplaintDescription NVARCHAR(MAX),
	@ComplaintDate DATETIME,
	@ControlMailNumber NVARCHAR(100),
	@NeedHelpTicketNumber NVARCHAR(8),
	@DaysToRespond INT
AS
	
	UPDATE
		[complaints].Complaints 
	SET
		BorrowerName = @BorrowerName,
		ComplaintPartyId = @ComplaintPartyId,
		ComplaintTypeId = @ComplaintTypeId,
		ComplaintDescription = @ComplaintDescription, 
	    ComplaintDate = @ComplaintDate,
		ControlMailNumber = @ControlMailNumber,
		NeedHelpTicketNumber = @NeedHelpTicketNumber,
		DaysToRespond = @DaysToRespond,
		ComplaintGroupId = @ComplaintGroupId
	WHERE
		ComplaintId = @ComplaintId

RETURN 0