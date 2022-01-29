CREATE PROCEDURE [complaints].[ComplaintTypeDelete]
	@ComplaintTypeId INT
AS

	UPDATE
		[complaints].ComplaintTypes
	SET
		DeletedOn = GETDATE(),
		DeletedBy = SYSTEM_USER
	WHERE
		ComplaintTypeId = @ComplaintTypeId


RETURN 0