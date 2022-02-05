CREATE PROCEDURE [complaints].[ComplaintPartyDelete]
	@ComplaintPartyId int
AS

	update [complaints].ComplaintParties
	   set DeletedOn = getdate(), DeletedBy = SYSTEM_USER
	 where ComplaintPartyId = @ComplaintPartyId


RETURN 0