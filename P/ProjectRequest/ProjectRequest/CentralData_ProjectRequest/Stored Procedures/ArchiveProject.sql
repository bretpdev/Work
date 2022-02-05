CREATE PROCEDURE [projectrequest].[ArchiveProject]
(
	@ProjectId INT
)
AS

	UPDATE
	[projectrequest].[Projects]
	SET
	[ArchivedAt] = GETDATE(),
	[ArchivedBy] = SUSER_NAME()
	WHERE
	[ProjectId] = @ProjectId
