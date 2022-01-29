use NobleCalls
go

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XXXXXX --might be half this if temp table inserts dont count 

DECLARE @BadCallIds TABLE(NobleCallHistoryId INT)
INSERT INTO @BadCallIds
SELECT
	NobleCallHistoryId
FROM
(
	SELECT 
		NobleRowId, 
		NobleCallHistoryId, 
		ROW_NUMBER() OVER(PARTITION BY NobleRowId ORDER BY NobleCallHistoryId) AS CallRank
	from 
		NobleCalls..NobleCallHistory 
	WHERE
		CAST(ActivityDate AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
) RankedCalls
WHERE
	RankedCalls.CallRank != X --Get all duplicates except the earliest one, as we want to keep that one

select * from @BadCallIds


SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR  --XXXXXX or X depending on how inserts into temp tables are counted

UPDATE
	NCH
SET
	NCH.Deleted = X,
	NCH.DeletedBy = 'CNH XXXXX',
	NCH.DeletedAt = GETDATE()
FROM
	NobleCalls..NobleCallHistory NCH
	INNER JOIN @BadCallIds B
		ON B.NobleCallHistoryId = NCH.NobleCallHistoryId

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR --XXXXXX

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
