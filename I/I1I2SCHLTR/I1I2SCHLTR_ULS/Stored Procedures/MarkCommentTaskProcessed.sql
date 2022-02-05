CREATE PROCEDURE [i1i2schltr].[MarkCommentTaskProcessed]
	@CommentDataId INT
AS
	UPDATE
		i1i2schltr.CommentData
	SET
		TaskProcessedAt = GETDATE()
	WHERE
		CommentDataId = @CommentDataId
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL
RETURN 0
