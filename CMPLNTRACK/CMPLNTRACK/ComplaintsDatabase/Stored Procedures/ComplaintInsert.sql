CREATE PROCEDURE [complaints].[ComplaintInsert]
	@AccountNumber CHAR(10),
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

	INSERT INTO [complaints].Complaints (AccountNumber, BorrowerName, ComplaintPartyId, ComplaintTypeId, ComplaintDescription, ComplaintDate, ControlMailNumber, NeedHelpTicketNumber, DaysToRespond, ComplaintGroupId)
	VALUES (@AccountNumber, @BorrowerName, @ComplaintPartyId, @ComplaintTypeId, @ComplaintDescription, @ComplaintDate, @ControlMailNumber, @NeedHelpTicketNumber, @DaysToRespond, @ComplaintGroupId)

	SELECT CAST(SCOPE_IDENTITY() AS INT) AS ComplaintId

RETURN 0