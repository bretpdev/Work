use CLS
go

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXX--XXX records to be deleted.

UPDATE
	PQ
SET
	PQ.DeletedOn = GETDATE(),
	PQ.DeletedBy = 'CNH XXXXX',
	PQ.DeletedNotes = 'Removed due to double load'
FROM
	CLS.dasforbfed.ProcessQueue PQ
	INNER JOIN
	(
		SELECT
			MIN(ProcessQueueId) AS PQID, 
			AccountNumber, 
			BeginDate, 
			EndDate
		FROM 
			CLS.dasforbfed.ProcessQueue 
		WHERE 
			ForbearanceAddedOn IS NULL 
			AND ArcAddProcessingId IS NULL 
			AND DeletedOn IS NULL 
		GROUP BY 
			AccountNumber, 
			BeginDate, 
			EndDate
	) MinProcessQueue
		ON MinProcessQueue.PQID = PQ.ProcessQueueId

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR  --XXX

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
