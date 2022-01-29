CREATE PROCEDURE [complaints].[ComplaintGroupDelete]
	@ComplaintGroupId INT
AS

	UPDATE
		[complaints].ComplaintGroups
	SET
		DeletedOn = GETDATE(),
		DeletedBy = SYSTEM_USER
	WHERE
		ComplaintGroupId = @ComplaintGroupId

RETURN 0