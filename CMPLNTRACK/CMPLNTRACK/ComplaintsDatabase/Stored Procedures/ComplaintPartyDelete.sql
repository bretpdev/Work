CREATE PROCEDURE [complaints].[ComplaintPartyDelete]
	@ComplaintPartyId INT
AS

	UPDATE
		[complaints].ComplaintParties
	SET
		DeletedOn = GETDATE(),
		DeletedBy = SYSTEM_USER
	WHERE
		ComplaintPartyId = @ComplaintPartyId


RETURN 0