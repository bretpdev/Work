CREATE PROCEDURE [i1i2schltr].[MarkCommentProcessed]
	@CommentDataId INT
AS
	UPDATE
		i1i2schltr.CommentData
	SET
		CommentProcessedAt = GETDATE()
	WHERE
		CommentDataId = @CommentDataId
		AND DeletedAt IS NULL
		AND DeletedBy IS NULL
RETURN 0
