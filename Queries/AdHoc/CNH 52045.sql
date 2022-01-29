BEGIN TRANSACTION
DECLARE @ERROR INT = X
DECLARE @ROWCOUNT INT = X
DECLARE @ExpectedRowCount INT = X

--Query Body
UPDATE
	[CLS].[dbo].[QueueBuilderFile]
SET
	MissingFileIsOk = X
WHERE
	[FileName] IN
	(
		'ULWGXX.LWGXXRX*',
		'UNWSXX.NWSXXRX*',
		'UNWSXX.NWSXXRX*'
	)

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT

DELETE
FROM
	[CLS].[dbo].[QueueBuilderFile]
WHERE
	[FileName] IN
	(
		'UNWSXX.NWSXXRX*'
	)

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT

UPDATE
	[CLS].[dbo].[QueueBuilderFile]
SET
	[FileName] = 'UNWSXX.LWSXXRX*'
WHERE
	[FileName] IN
	(
		'ULWSXX.LWSXXRX*'
	)
--Query Body

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @@ERROR

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



