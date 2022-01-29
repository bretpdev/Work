CREATE PROCEDURE [i1i2schltr].[GetUnprocessedCommentData]
AS
	UPDATE CD
	SET 
		ProcessingAttempts = ProcessingAttempts + 1,
		DeletedAt = CASE WHEN ProcessingAttempts > 3 THEN GETDATE() ELSE NULL END,
		DeletedBy = CASE WHEN ProcessingAttempts > 3 THEN SUSER_NAME() ELSE NULL END
	FROM
		i1i2schltr.CommentData CD
	WHERE
		(CD.CommentProcessedAt IS NULL OR CD.TaskProcessedAt IS NULL)
		AND CD.DeletedAt IS NULL
		AND CD.DeletedBy IS NULL

	--This maps to the QueueTaskData object in c#
	SELECT
		CD.CommentDataId,
		CD.SSN,
		CD.RunDateId,
		CD.CommentProcessedAt,
		CD.TaskProcessedAt
	FROM
		i1i2schltr.CommentData CD
	WHERE
		(CD.CommentProcessedAt IS NULL OR CD.TaskProcessedAt IS NULL)
		AND CD.DeletedAt IS NULL
		AND CD.DeletedBy IS NULL
RETURN 0
