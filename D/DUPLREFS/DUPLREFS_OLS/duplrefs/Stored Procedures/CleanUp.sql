CREATE PROCEDURE [duplrefs].[CleanUp]
	
AS

	UPDATE
		RefQueue
	SET
		RefQueue.DeletedAt = GETDATE(),
		RefQueue.DeletedBy = SUSER_NAME()
	FROM
		(
			SELECT	
				RQ.*
			FROM
				duplrefs.BorrowerQueue BQ
				INNER JOIN duplrefs.ReferenceQueue RQ
					ON BQ.BorrowerQueueId = RQ.BorrowerQueueId
			WHERE
				RQ.DeletedAt IS NULL
				AND 
				(	
					RQ.ProcessedAt IS NULL
					OR RQ.Lp2fProcessedAt IS NULL
				)
				AND 
				(	
					BQ.CreatedAt < DATEADD(DAY, -3, GETDATE()) --If task is more than three days old, or if it was already processed or deleted
					OR BQ.ProcessedAt IS NOT NULL
					OR BQ.DeletedAt IS NOT NULL
				)

		) RefQueue

	UPDATE
		duplrefs.BorrowerQueue 
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_NAME()
	WHERE
		DeletedAt IS NULL
		AND ProcessedAt IS NULL
		AND CreatedAt < DATEADD(DAY, -3, GETDATE())

RETURN 0
