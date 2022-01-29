BEGIN TRANSACTION
DECLARE @ERROR INT = X
DECLARE @ROWCOUNT INT = X
DECLARE @ExpectedRowCount INT = X + XX --XX

--Query Body

--Deletions--
UPDATE
	NobleCalls.dbo.NobleCallHistory
SET
	Deleted = X, DeletedBy = 'DCR NHXXXX', DeletedAt = GETDATE()
WHERE
	NobleCallHistoryId IN (XXXXXXX,XXXXXXX,XXXXXXX,XXXXXXX) -- IDs
						   
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT						   

DECLARE @Updates TABLE(
	NobleCallHistoryId int,
	NewIdentifier varchar(XX)
)

INSERT INTO @Updates
VALUES(XXXXXXX,'XXXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX')
,(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX')
,(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX'),(XXXXXXX,'XXXXXXXXXX') --XX Rows

UPDATE
	nch
SET
	nch.AccountIdentifier = u.NewIdentifier,
	nch.ArcAddProcessingId = null
FROM
	NobleCalls.dbo.NobleCallHistory nch
	JOIN @Updates u ON nch.NobleCallHistoryId = u.NobleCallHistoryId

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT						   


--Query Body

SELECT @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END

